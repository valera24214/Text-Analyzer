using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp2.TextProcessors;
using WinFormsApp2.TextProvider;

namespace WinFormsApp2
{
    internal class TextAnalysisCoordinator
    {
        private ITextProvider _textProvider;
        private ITextProcessor _textProcessor;
        private SemaphoreSlim _semaphore;

        public TextAnalysisCoordinator(ITextProvider textProvider, ITextProcessor textProcessor, int semaphoreWidth)
        {
            _textProvider = textProvider;
            _textProcessor = textProcessor;
            _semaphore = new SemaphoreSlim(semaphoreWidth, semaphoreWidth);
        }

        private async Task<string> GetResultFromOnePage()
        {
            await _semaphore.WaitAsync();
            try
            {
                var words = await _textProvider.GetWordsFromRandomPage();
                return await Task.Run(() => _textProcessor.Process(words));
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<string> Analyze(int pageCount)
        {
            Task<string>[] taskArray = new Task<string>[pageCount];
            for (int i = 0; i < pageCount; i++)
                taskArray[i] = GetResultFromOnePage();

            var array = await Task.WhenAll(taskArray);
            var rez = array.Aggregate(_textProcessor.InitialValue, (acc, next) => _textProcessor.Aggregate(acc, next));
            
            return rez;
        }
    }
}
