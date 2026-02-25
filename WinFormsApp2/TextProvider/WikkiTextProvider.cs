using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WinFormsApp2.TextProvider
{
    internal class WikkiTextProvider : ITextProvider
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public WikkiTextProvider()
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "MyLearningBot/1.0 (contact@example.com)");
        }

        private async Task<string> GetRandomWikiPageTitleAsync()
        {
            string apiUrl = "https://en.wikipedia.org/api/rest_v1/page/random/summary";

            var responce = await _httpClient.GetAsync(apiUrl);
            responce.EnsureSuccessStatusCode();
            var json = await responce.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeAnonymousType(json, new { title = "" });

            return result.title;
        }

        private string ClearHtmlTags(string html)
        {
            html = Regex.Replace(html, @"<(style|script)[^>]*>[\s\S]*?</\1>", " ", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"\{[\s\S]*?\}", " ");
            html = Regex.Replace(html, @"<[^>]*>", " ");
            return System.Net.WebUtility.HtmlDecode(html);
        }

        private string[] BreakForWords(string text)
        {
            char[] separators = {
                ' ', '\n', '\r', '\t', // Пробельные
                '.', ',', '!', '?', ':', ';', '-', '—', '"', '\'', // Пунктуация
                '{', '}', '[', ']', '(', ')', '<', '>', '/', '\\', '|', // Скобки и слэши
                '=', '+', '*', '#', '@', '$', '%', '^', '&', '_', '~', // Математика и спецсимволы
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' // Цифры (нам же нужны СЛОВА)
            };

            return text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        public async Task<string[]> GetWordsFromRandomPage()
        {
            string title = await GetRandomWikiPageTitleAsync();
            string url = $"https://en.wikipedia.org/api/rest_v1/page/html/{Uri.EscapeDataString(title)}";
            var html = await _httpClient.GetStringAsync(url);
            html = ClearHtmlTags(html);
            var words = BreakForWords(html);
            return words.Where(w => w.All(char.IsLetter)).ToArray();
        }
    }
}
