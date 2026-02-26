using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2.EventsArguments
{
    internal class CountResultArgs : ResultEventArgs
    {
        public int count {  get; }
        public override string description { get; }

        public CountResultArgs(int count, string description)
        {
            this.count = count;
            this.description = description;
        }
    }
}
