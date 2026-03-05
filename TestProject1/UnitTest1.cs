using WinFormsApp2.TextProcessors;
using WinFormsApp2;

namespace TestProject1
{
    public class UnitTest1
    {
        [Theory]
        [InlineData('a', new string[]{"apple", "bAnanA", "cat" }, 5)]
        [InlineData('B', new string[]{"barrell", "believable", "BigBetty", "able"}, 6)]
        public void CharCountingTest(char ch, string[] words, int expectedResult)
        {
            SettedCharCountProcessor countProcessor = new SettedCharCountProcessor(ch);
            bool success = int.TryParse(countProcessor.Process(words), out int result);

            Assert.True(success);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void AggregateTest()
        {
            //Arrange
            string[] words =
            {
                "ancestor", "harbor", "right",
                "journal", "channel", "meal", 
                "infrastructure", "final", "digress" 
            };

            var targetChar = 'a';

            ExtremumWordProcessor maxProcessor = new ExtremumWordProcessor(ExtremumMode.Maximum);
            ExtremumWordProcessor minProcessor = new ExtremumWordProcessor(ExtremumMode.Minimum);
            SettedCharCountProcessor charCountProcessor = new SettedCharCountProcessor(targetChar);

            //Act

            string[] charCountsArray = new string[words.Length];
            for (int i = 0; i < words.Length; i++)
            {
                charCountsArray[i] = words[i].Count(ch => char.ToLower(ch) == char.ToLower(targetChar)).ToString();
            }

            var max = words.Aggregate(maxProcessor.InitialValue, (acc, next) => maxProcessor.Aggregate(acc, next));
            var min = words.Aggregate(minProcessor.InitialValue, (acc, next) => minProcessor.Aggregate(acc, next));
            var count = charCountsArray.Aggregate(charCountProcessor.InitialValue, (acc, next) => charCountProcessor.Aggregate(acc, next));
            
            //Assert
            Assert.Equal("infrastructure", max);
            Assert.Equal("meal", min);
            Assert.Equal("7", count);
            
        }

        [Theory]
        [InlineData(new string[] {"me", "cat", "tree", "threemetilbutan"}, ExtremumMode.Maximum, "threemetilbutan")]
        [InlineData(new string[] {"me", "cat", "tree", "threemetilbutan"}, ExtremumMode.Minimum, "me")]
        public void FindingExtremumTest(string[] words, ExtremumMode mode, string expectedResult)
        {
            ExtremumWordProcessor extremumProcessor = new ExtremumWordProcessor(mode);
            string result = extremumProcessor.Process(words);

            Assert.Equal(expectedResult, result);
        }
    }
}