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
        public delegate void ConvertedEventHandler(object source,
            ConvertedEventArgs args);
        public delegate void QueueUpdatedEventHandler(object source,
            QueueUpdatedEventArgs args);

        public event QueueUpdatedEventHandler QueueUpdated;

        private List<FileInfo> _queue;
        private readonly string _outputDir;
        private Word.Application _wordApp;
        private Word.Document _wordDoc;
        

        public Converter(string outputDir)
        {
            _queue = new();
            _outputDir = outputDir;
        }

        public ConvertResult Convert()
        {
            ConvertResult convertResult = new();

            if (!Directory.Exists(_outputDir))
                Directory.CreateDirectory(_outputDir);

            foreach(FileInfo file in _queue)
            {
                if (!File.Exists(file.FullName) || file.Name.StartsWith("~"))
                    continue;

                _wordApp = new()
                {
                    Visible = false
                };

                _wordDoc = _wordApp.Documents.Open(file.FullName);

                _wordDoc.ExportAsFixedFormat2($@"{_outputDir}\{file.Name}.pdf",
                    Word.WdExportFormat.wdExportFormatPDF);

                _wordDoc.Close();
                _wordApp.Quit();
            }

            return convertResult;
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
            }
        }

        private void AddToQueue(FileInfo file)
        {
            if (!_queue.Contains(file))
                _queue.Add(file);
        }

        private void RemoveFromQueue(FileInfo file)
        {
            _queue.Remove(file);
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
    }
}
