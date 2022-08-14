using System.Diagnostics;

namespace MoogleEngine
{
    [DebuggerDisplay("TF {TF}, IDF {IDF}, TFIDF {TFIDF}")]
    public class QueryData
    {
        public double TF { get; set; }
        public double IDF { get; set; }
        public double TFIDF { get; set; }

        public void CalculateTFIDF()
        {
            TFIDF = TF * IDF;
        }
    }
}