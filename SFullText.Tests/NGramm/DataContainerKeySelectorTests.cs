using System.Linq;
using NUnit.Framework;
using SFullText.NGramm;

namespace SFullText.Tests.NGramm
{
    [TestFixture]
    internal class DataContainerKeySelectorTests
    {
        [TestCase("", 1, new[] { "def" })]
        [TestCase("drip<=>dip", 2, new[] { "drip", "dip" })]
        [TestCase("drip <=> dip", 2, new[] { "drip", "dip" })]
        public void TestSpliting(string query, int length, string[] expectedKeys)
        {
            var result = DataContainerKeySelector.GetKeysByGroupingQuery(query).ToList();

            Assert.AreEqual(length, result.Count);
            Assert.IsTrue(result.All(key => expectedKeys.Contains(key)));
        }
    }
}
