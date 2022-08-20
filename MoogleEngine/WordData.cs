using System.Diagnostics;

namespace MoogleEngine
{
    [DebuggerDisplay("IDF {IDF}")]
    public class WordData
    {
        public double[] TF { get; set; }
        public double IDF { get; set; }
        public double[] TFIDF { get; set; }

        public string[] Snippets { get; set; }

        public List<int>[] WordIndexes { get; set; }

        // Ineficiente crear un array nuevo copiando uno que ya existe
        //public WordData(int[] termFrecuency)
        //{
        //    this.TermFrecuency = new int[termFrecuency.Length];
        //    Array.Copy(termFrecuency, this.TermFrecuency, termFrecuency.Length);
        //}

        public WordData(double[] termFrecuency)
        {
            this.TF = termFrecuency;
        }

        public void CalculateIDF()
        {
            double result = 0;

            for (int i = 0; i < TF.Length; i++)
            {
                if (TF[i] != 0)
                {
                    result++;
                }
            }

            //return (double)Math.Log10(TermFrecuency.Length / (0.1 + result));
            IDF =  1 + (double)Math.Log( 1 + TF.Length / result);
        }
        public void CalculateTFIDF()
        {
            TFIDF = new double[TF.Length];

            for(int i = 0; i < TFIDF.Length; i++)
            {
                TFIDF[i] = TF[i] * IDF;
            }
        }
    }
}