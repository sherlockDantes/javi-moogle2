namespace MoogleEngine
{
    static class Core
    {
        public static SortedDictionary<string, WordData> GetCorpus(string folderPath)
        {
            SortedDictionary<string, WordData> corpus = new SortedDictionary<string, WordData>();

            string[] files = Directory.GetFiles(folderPath);

            for (int i = 0; i < files.Length; i++)
            {
                string[] words = Tools.Tokenize(File.ReadAllText(files[i]));
                int[] wordFrecuency;

                foreach (var word in words)
                {
                    if (corpus.ContainsKey(word))
                    {
                        wordFrecuency = corpus[word].TermFrecuency;
                        wordFrecuency[i]++;
                    }
                    else
                    {
                        wordFrecuency = new int[files.Length];
                        wordFrecuency[i]++;
                        corpus.Add(word, new WordData(wordFrecuency));
                    }

                }
            }

            return corpus;
        }

        public static Dictionary<string, float[]> GetTF_IDF_MatrixNormalized(SortedDictionary<string, WordData> Corpus, string[] files)
        {
            var matrix = new Dictionary<string, float[]>();

            for (int i = 0; i < files.Length; i++)
            {
                matrix.Add(Path.GetFileName(files[i]), new float[Corpus.Count]);
                float powers = 0;
                foreach (var pair in Corpus)
                {
                    powers += (float)Math.Pow(pair.Value.TF_IDF[i], 2);
                }

                int count = 0;
                foreach (var pair in Corpus)
                {
                    matrix[Path.GetFileName(files[i])][count] = pair.Value.TF_IDF[i] * (float)(1 / Math.Sqrt(powers));
                    count++;
                }
            }

            return matrix;
        }
    }
}