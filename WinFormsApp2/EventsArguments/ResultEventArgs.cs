using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2.EventsArguments
{
    internal abstract class ResultEventArgs : EventArgs
    {
        public abstract string description {  get; }
    }
}
