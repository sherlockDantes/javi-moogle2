namespace MoogleEngine
{
    class WordData
    {
        int[] termFrecuency;
        public int[] TermFrecuency
        {
            get { return termFrecuency; }
            set { termFrecuency = value; }
        }
        public float IDF
        {
            get { return GetIDF(); }
        }
        public float[] TF_IDF
        {
            get { return GetTF_IDF(); }
        }
        public WordData(int[] termFrecuency)
        {
            this.termFrecuency = new int[termFrecuency.Length];
            Array.Copy(termFrecuency, this.termFrecuency, termFrecuency.Length);
        }
        private float GetIDF()
        {
            float result = 0;

            for (int i = 0; i < termFrecuency.Length; i++)
            {
                if (termFrecuency[i] != 0)
                {
                    result++;
                }
            }

            return (float)Math.Log10(termFrecuency.Length / (1 + result));
        }
        private float[] GetTF_IDF()
        {
            float[] result = new float[termFrecuency.Length];

            for(int i = 0; i < result.Length; i++)
            {
                result[i] = termFrecuency[i] * GetIDF();
            }

            return result;
        }
    }
}