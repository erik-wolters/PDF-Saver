using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFSaverTestUI.Models
{
    public class LogEntry
    {
        public enum EntryType
        {
            Info,
            Warning,
            Error
        }

        public EntryType Type { get; set; }
        public string Message { get; set; }

        public LogEntry() { }

        public override string ToString()
        {
            return $"{Type} - {Message}";
        }
    }
}
