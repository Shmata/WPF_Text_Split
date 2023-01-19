using FileProcessing.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;

namespace FileProcessing.Business.Test
{
    [TestClass]
    public class WordPrtocessorTest
    {
        [DataTestMethod]
        [DataRow(new[] { "a", "b", "c" }, new[] { 1, 1, 1 })]
        [DataRow(new[] { "a\nb\r\nc" }, new[] { 1, 1, 1 })]
        [DataRow(new[] { "a\nb\nc" }, new[] { 1, 1, 1 })]
        [DataRow(new[] { "a a\nb a\n  \ra" }, new[] { 4, 1 })]
        [DataRow(new[] { "a a\nb a\n  \ra c" }, new[] { 4, 1, 1 })]
        public void LoadCollectionData(string[] words, int[] frequencies)
        {
            var canceller = new CancellationTokenSource();
            var tokens = new Dictionary<string, Token>();
            var wordProcessor = new WordProcessor();
            wordProcessor.LoadCollectionData(words, canceller, tokens);

            int index = 0;
            foreach (Token token in tokens.Values)
            {
                Assert.AreEqual(token.Occurrence, frequencies[index]);
                index++;
            }
        }
    }
}
