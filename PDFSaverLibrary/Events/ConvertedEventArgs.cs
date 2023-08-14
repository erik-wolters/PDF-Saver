using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFSaverLibrary.Events
{
    public class ConvertedEventArgs : EventArgs
    {
        public bool Isssuccessfull { get; set; }
        public string Message { get; set; }
    }
}
