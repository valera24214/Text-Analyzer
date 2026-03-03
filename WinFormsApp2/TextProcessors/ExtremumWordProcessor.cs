using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WinFormsApp2.EventsArguments;

namespace WinFormsApp2.TextProcessors
{
    public enum ExtremumMode
    {
        Maximum,
        Minimum
    }

    internal class ExtremumWordProcessor : ITextProcessor
    {
        public ExtremumMode mode { get; } = ExtremumMode.Maximum;

        public string InitialValue { get; } = string.Empty;

        public event EventHandler<ResultEventArgs> OnProcessed;

        public ExtremumWordProcessor(ExtremumMode mode)
        {
            this.mode = mode;
        }

        public string Process(string[] words)
        {
            string result = string.Empty; 
            string description = string.Empty;

            switch (mode)
            {
                case ExtremumMode.Maximum:
                    {
                        result = words.MaxBy(w => w.Length);
                        description = "Самое длинное слово в тексте";
                        break;
                    }

                case ExtremumMode.Minimum:
                    {
                        result = words.MinBy(w => w.Length);
                        description = "Самое короткое слово в тексте";
                        break;
                    }

                default:
                    {
                        throw new ArgumentException("Invalid mode value");
                    }
            }

            OnProcessed?.Invoke(this, new WordResultArgs(result, description));

            return result;
        }

        public string Aggregate(string acc, string value)
        {
            string result = null;
            switch (mode)
            {
                case ExtremumMode.Maximum : 
                    {
                        result = acc.Length > value.Length ? acc : value;
                        break;
                    }
                case ExtremumMode.Minimum :
                    {
                        result = acc.Length < value.Length ? acc : value;
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("Invalid mode value");
                    }
            }

            return result;
        }
    }
}
