namespace SFullText.NGramm
{
    public static class NGrammStringSplitter
    {
        public static IEnumerable<string> SplitString(string? term, int nGrammLength)
        {
            if (string.IsNullOrWhiteSpace(term)) yield break;

            term = NormailzeTerm(term, nGrammLength);

            for (int i = 0; i <= term.Length - nGrammLength; i++)
            {
                yield return term.Substring(i, nGrammLength);
            }
        }

        private static string NormailzeTerm(string term, int nGrammLength)
        {
            var capacity = nGrammLength - term.Length;

            return (capacity > 0
                ? term + string.Join(string.Empty, Enumerable.Repeat(' ', capacity))
                : term).ToLower();
        }
    }
}
