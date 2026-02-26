using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2.EventsArguments
{
    internal class WordResultArgs : ResultEventArgs
    {
        public string targetWord { get; }
        public override string description { get; }

        public WordResultArgs(string targetWord, string descryption)
        {
            this.targetWord = targetWord;
            this.description = descryption;
        }
    }
}
