using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2.TextProvider
{
    internal interface ITextProvider
    {
        Task<string[]> GetWordsFromRandomPage();
    }
}
