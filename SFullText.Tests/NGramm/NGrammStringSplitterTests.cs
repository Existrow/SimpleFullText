using System.Linq;
using NUnit.Framework;
using SFullText.NGramm;

namespace SFullText.Tests.NGramm
{
    [TestFixture]
    internal class NGrammStringSplitterTests
    {
        [Test]
        public void TestSplitting()
        {
            var word = "ЛеНинА";

            var parts = NGrammStringSplitter.SplitString(word, 3).ToList();

            Assert.AreEqual(4, parts.Count);
            Assert.IsTrue(parts.Contains("лен"));
            Assert.IsTrue(parts.Contains("ени"));
            Assert.IsTrue(parts.Contains("нин"));
            Assert.IsTrue(parts.Contains("ина"));
        }

        [Test]
        public void TestSplittingShortWord()
        {
            var word = "Б";

            var parts = NGrammStringSplitter.SplitString(word, 3).ToList();

            Assert.IsTrue(parts.Contains("б  "));
        }
    }
}
