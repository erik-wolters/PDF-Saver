using System;
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
        public delegate void ConverterUpdatedEvenHandler(object source,
            ConverterUpdatedEventArgs args);

        public event ConverterUpdatedEvenHandler ConverterUpdated;

        private readonly List<string> _queue;
        private string _outputDir;
        private Word.Application _wordApp;
        private Word.Document _wordDoc;

        public Converter()
        {
            _queue = new();
        }

        public void Convert()
        {
            if (!Directory.Exists(_outputDir))
            {
                Directory.CreateDirectory(_outputDir);
                OnConverterUpdated(true, "Uitvoerdirectory aangemaakt.");
            }

            foreach(string file in _queue)
            {
                FileInfo fileInfo = new(file);

                if (!File.Exists(fileInfo.FullName) || fileInfo.Name.StartsWith("~"))
                {
                    OnConverterUpdated(false, "Autosave- en autoherstelbestanden " +
                        "kunnen niet geconverteerd worden.");

                    continue;
                }

                try
                {
                    _wordApp = new()
                    {
                        Visible = false
                    };

                    _wordDoc = _wordApp.Documents.Open(fileInfo.FullName);

                    _wordDoc.ExportAsFixedFormat2(
                        $@"{_outputDir}\{fileInfo.Name}.pdf",
                        Word.WdExportFormat.wdExportFormatPDF
                        );

                    _wordDoc.Close();
                    _wordApp.Quit();

                    OnConverterUpdated(true,
                        $"Het document {fileInfo.Name} is als pdf opgeslagen");
                }
                catch (Exception e)
                {
                    OnConverterUpdated(false, e.Message);
                }
            }
        }

        /// <summary>
        /// Change the directory in which the pdf files must be saved.
        /// </summary>
        /// <param name="outputDir">The user defined directory</param>
        public void SetOutputDir(string outputDir)
        {
            _outputDir = outputDir;
            OnConverterUpdated(true, $"Uitvoermap ingesteld op: {_outputDir}" );
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
                OnConverterUpdated(true, "Wordprocess gesloten");
            }
        }

        /// <summary>
        /// Add the file to the queue if it meets the requirements.
        /// </summary>
        /// <param name="file">The file to add to the queue</param>
        public void AddToQueue(FileInfo file)
        {
            if(MeetsRequirements(file))
            {
                _queue.Add(file.FullName);
                OnConverterUpdated(true, $"Het bestand {file.Name} is toegevoegd " +
                    $"aan de queue");
            }
        }

        /// <summary>
        /// Check if the file meets the requirements.
        /// </summary>
        /// <param name="file">The file to check</param>
        /// <returns></returns>
        private bool MeetsRequirements(FileInfo file)
        {
            if (!file.Extension.Equals(".docx", StringComparison.OrdinalIgnoreCase) && 
                !file.Extension.Equals(".doc", StringComparison.OrdinalIgnoreCase))
            {
                OnConverterUpdated(false, $"Het bestand {file.Name} kan niet " +
                    $"worden toegevoegd omdat dit geen word bestand is.");

                return false;
            }

            if (_queue.Contains(file.FullName))
            {
                OnConverterUpdated(false, $"Het bestand: {file.Name}" +
                    " staat al in de queue");

                return false;
            }

            return true;
        }

        /// <summary>
        /// Remove a file from the queue if it exists.
        /// </summary>
        /// <param name="file">The file to remove</param>
        public void RemoveFromQueue(FileInfo file)
        {
            if(_queue.Contains(file.FullName))
            {
                _queue.Remove(file.FullName);
                OnConverterUpdated(true, $"Het bestand {file.Name} is uit de " +
                    $"queue verwijderd");
            } 
            else
            {
                OnConverterUpdated(false, $"Het bestand {file.Name} staat niet" +
                    $" in de queue en kan dus niet worden verwijderd.");
            }
            
        }

        /// <summary>
        /// Retrieve all items currently in the queue.
        /// </summary>
        /// <returns>The current queue</returns>
        public List<string> GetQueue()
        { 
            return _queue; 
        }

        /// <summary>
        /// Notify all event listeners about an update.
        /// </summary>
        /// <param name="isSuccessful"></param>
        /// <param name="message"></param>
        protected virtual void OnConverterUpdated(bool isSuccessful, 
            string message)
        {
            if(ConverterUpdated != null)
                ConverterUpdated(this, new ConverterUpdatedEventArgs 
                { 
                    IsSuccessful = isSuccessful, 
                    Message = message 
                });
        }
    }
}
