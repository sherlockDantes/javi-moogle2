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

                for  (var j = 0; j < words.Length; j++)
                {
                    var lindex = j - 5 < 0 ? 0 : j - 5;
                    var uindex = j + 5 > words.Length ? words.Length : j + 5;
                    string snippet = string.Join(" ", words.AsSpan()[lindex..uindex].ToArray());
                    if (corpus.ContainsKey(words[j]))
                    {
                        corpus[words[j]].Snippets[i] = snippet;
                        wordFrecuency = corpus[words[j]].TF;
                        wordFrecuency[i]++;
                        if (corpus[words[j]].WordIndexes[i] == null)
                            corpus[words[j]].WordIndexes[i] = new List<int>();
                        corpus[words[j]].WordIndexes[i].Add(j);
                    }
                    else
                    {
                        wordFrecuency = new double[files.Length];
                        wordFrecuency[i]++;
                        var snippets = new string[files.Length];
                        snippets[i] = snippet;
                        corpus.Add(words[j], new WordData(wordFrecuency) { Snippets = snippets, WordIndexes = new List<int>[files.Length] });
                        corpus[words[j]].WordIndexes[i] = new List<int>();
                        corpus[words[j]].WordIndexes[i].Add(j);
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

            string[] queryTerms = query.Split(" ").Select(x => x.Trim()).ToArray();

          
            for (var i = 0; i < queryTerms.Length; i++)
            {
                char? op = null;
                double opMultiplier = 0;
                var term = queryTerms[i].Trim();

                if (term.StartsWith('!') || term.StartsWith('^') || term.StartsWith('*'))
                {
                    op = term[0];
                    if (op == '*')
                    {
                        opMultiplier = 0.1 + Math.Log(term.Count(x => x.Equals(op)));
                    }
                }

                term = Tools.TokenizeWord(term);
                queryTerms[i] = term;

                if (queryData.ContainsKey(term))
                {
                    queryData[term].TF++;
                }
                else
                {
                    queryData.Add(term, new QueryData { TF = 1, Operator = op, OperatorMultiplier = opMultiplier });
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