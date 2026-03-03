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

        public string InitialValue => "0";

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

        public string Aggregate(string acc, string value)
        {
            if (int.TryParse(acc, out int result) && int.TryParse(value, out int next))
            {
                return (result + next).ToString();
            }
            else
                throw new ArgumentException("One of this arguments can't be parsed to int");
        }
    }
}