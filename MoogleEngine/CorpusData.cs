namespace MoogleEngine
{
    public class CorpusData
    {
        public int[] TotalWordsPerDocument { get; set; }

        public SortedDictionary<string, WordData> Corpus { get; set; }
    }
}