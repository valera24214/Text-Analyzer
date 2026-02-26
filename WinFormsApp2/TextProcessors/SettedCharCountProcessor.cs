using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp2.EventsArguments;

namespace WinFormsApp2.TextProcessors
{
    internal class SettedCharCountProcessor : ITextProcessor
    {
        public event EventHandler<ResultEventArgs> OnProcessed;
        private char target;
        
        public SettedCharCountProcessor(char target)
        {
            this.target = target; 
        }

        public string Process(string[] words)
        {
            int count = 0;
            foreach (string word in words) 
            {
                count += word.Count(ch => char.ToLower(ch) == char.ToLower(target));
            }

            OnProcessed?.Invoke(this, new CountResultArgs(count, $"Количество букв {target} в тексте"));
            return count.ToString();
        }
    }
}