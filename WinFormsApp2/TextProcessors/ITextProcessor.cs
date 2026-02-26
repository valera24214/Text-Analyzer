using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp2.EventsArguments;

namespace WinFormsApp2.TextProcessors
{
    internal interface ITextProcessor
    {
        event EventHandler<ResultEventArgs> OnProcessed;
        string Process(string[] words);
    }
}
