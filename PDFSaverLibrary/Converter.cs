﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDFSaverLibrary.Events;
using Word = Microsoft.Office.Interop.Word;

namespace PDFSaverLibrary
{
    public class Converter
    {
        public delegate void ConvertedEventHandler(object source,
            ConvertedEventArgs args);
        public delegate void QueueUpdatedEventHandler(object source,
            QueueUpdatedEventArgs args);

        public event QueueUpdatedEventHandler QueueUpdated;
        public event ConvertedEventHandler Converted;

        private readonly List<FileInfo> _queue;
        private string _outputDir;
        private Word.Application _wordApp;
        private Word.Document _wordDoc;
        

        public Converter()
        {
            _queue = new();
        }

        public ConvertResult Convert()
        {
            ConvertResult convertResult = new();

            if (!Directory.Exists(_outputDir))
            {
                Directory.CreateDirectory(_outputDir);
                OnConverted(true, "Uitvoerdirectory aangemaakt.");
            }
                

            foreach(FileInfo file in _queue)
            {
                if (!File.Exists(file.FullName) || file.Name.StartsWith("~"))
                {
                    OnConverted(false, "Autosave- en autoherstelbestanden " +
                        "kunnen niet geconverteerd worden.");

                    continue;
                }

                try
                {
                    _wordApp = new()
                    {
                        Visible = false
                    };

                    _wordDoc = _wordApp.Documents.Open(file.FullName);

                    _wordDoc.ExportAsFixedFormat2(
                        $@"{_outputDir}\{file.Name}.pdf",
                        Word.WdExportFormat.wdExportFormatPDF
                        );

                    _wordDoc.Close();
                    _wordApp.Quit();

                    OnConverted(true,
                        $"Het document {file.Name} is als pdf opgeslagen");
                }
                catch (Exception e)
                {
                    OnConverted(false, e.Message);
                }
            }

            return convertResult;
        }

        public void SetOutputDir(string outputDir)
        {
            _outputDir = outputDir;
            OnConverted(true, $"Uitvoermap ingesteld op: {_outputDir}" );
        }

        /// <summary>
        /// Helper method to close all running wordprocesses.
        /// </summary>
        private void CloseWordProcesses()
        {
            foreach (Process p in Process.GetProcessesByName("WINWORD"))
            {
                p.Kill();
                p.WaitForExit();
                OnConverted(true, "Wordprocess gesloten");
            }
        }

        public void AddToQueue(FileInfo file)
        {
            if(file.Extension != ".docx" ||  file.Extension != ".doc")
            {
                OnQueueUpdated(false, $"Het bestand {file.Name} kan niet " +
                    $"worden toegevoegd omdat dit geen word bestand is.");

                return;
            }

            if (_queue.Contains(file))
            {
                OnQueueUpdated(false, $"Het bestand: {file.Name}" +
                    " staat al in de queue");

                return;
            }

            _queue.Add(file);
            OnQueueUpdated(true, $"Het bestand {file.Name} is toegevoegd " +
                $"aan de queue");

        }

        public void RemoveFromQueue(FileInfo file)
        {
            if(_queue.Contains(file))
            {
                _queue.Remove(file);
                OnQueueUpdated(true, $"Het bestand {file.Name} is uit de " +
                    $"queue verwijderd");
            } 
            else
            {
                OnQueueUpdated(false, $"Het bestand {file.Name} staat niet" +
                    $" in de queue en kan dus niet worden verwijderd.");
            }
            
        }

        public List<FileInfo> GetQueue() 
        { 
            return _queue; 
        }

        protected virtual void OnQueueUpdated( bool isSuccessful,
            string message)
        {
            if (QueueUpdated != null)
                QueueUpdated(this, new QueueUpdatedEventArgs {
                    Issuccessful = isSuccessful,
                    Message = message
                });
        }

        protected virtual void OnConverted( bool isSuccesful, 
            string message)
        {
            if (Converted != null)
                Converted(this, new ConvertedEventArgs
                {
                    Isssuccessfull = isSuccesful,
                    Message = message
                });
        }
    }
}
