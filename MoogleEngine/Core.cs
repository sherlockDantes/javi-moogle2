namespace MoogleEngine
{
    static class Core
    {
        public static SortedDictionary<string, WordData> GetCorpus(string[] files)
        {
            var corpus = new SortedDictionary<string, WordData>();

            if (files.Length == 0)
                return null;

            int[] documentTotalWords = new int[files.Length];

            double[] wordFrecuency;
            for (int i = 0; i < files.Length; i++)
            {
                string[] words = Tools.Tokenize(File.ReadAllText(files[i]));

                documentTotalWords[i] = words.Length;

                foreach (var word in words)
                {
                    if (corpus.ContainsKey(word))
                    {
                        wordFrecuency = corpus[word].TF;
                        wordFrecuency[i]++;
                    }
                    else
                    {
                        wordFrecuency = new double[files.Length];
                        wordFrecuency[i]++;
                        corpus.Add(word, new WordData(wordFrecuency));
                    }
                }

                // Normalize TF: divide the term frequency by the total number of terms in a document
                foreach (var word in words)
                {
                    wordFrecuency = corpus[word].TF;
                    wordFrecuency[i] = wordFrecuency[i] / words.Length;
                    corpus[word].CalculateIDF();
                    corpus[word].CalculateTFIDF();
                }
            }

            return corpus;
        }

        public static SortedDictionary<string, QueryData> GetQueryData(string query, SortedDictionary<string, WordData> corpus)
        {
            var queryData = new SortedDictionary<string, QueryData>();

            string[] queryTerms = query.Split(" ");

            for (var i = 0; i < queryTerms.Length; i++)
            {
                if (queryData.ContainsKey(queryTerms[i]))
                {
                    queryData[queryTerms[i]].TF++;
                }
                else
                {
                    queryData.Add(queryTerms[i], new QueryData { TF = 1 });
                }
            }

            // Normalize TF: divide the term frequency by the total number of terms in the query
            for (var i = 0; i < queryTerms.Length; i++)
            {
                if (queryData.ContainsKey(queryTerms[i]))
                {
                    queryData[queryTerms[i]].TF = queryData[queryTerms[i]].TF / queryTerms.Length;


                    if (corpus.ContainsKey(queryTerms[i]))
                    {
                        queryData[queryTerms[i]].IDF = corpus[queryTerms[i]].IDF;
                    }

                    queryData[queryTerms[i]].CalculateTFIDF();
                }
            }



            return queryData;
        }

    }
}