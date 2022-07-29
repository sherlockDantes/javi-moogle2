namespace MoogleEngine
{
    public static class TFIDF
    {
        public static Dictionary<string, List<double>> CalculateTF(SortedDictionary<string, List<int>> vocabulary, int corpusLenght)
        {
            var tfMatrix = new Dictionary<string, List<double>>();
            // itera por palabras
            foreach (var word in vocabulary.Keys)
            {
                // Create una entrada por palabra en el dictionario TF con una lista de TF inicializada en cero por cada documento en el corpus
                var wordTFPerDocuments = Enumerable.Repeat(0d, corpusLenght).ToList();
                tfMatrix.Add(word, wordTFPerDocuments);

                // lista de la frequencia de la palabra por documento
                var frecuencyPerDocument = vocabulary[word];


                // guarda la frecuencia de la palabra en todo el corpus
                var corpusWordFrequency = frecuencyPerDocument.Sum(f => f);

                // i itera por la frecuencia de la palabra en cada documento
                for (int j = 0; j < frecuencyPerDocument.Count; j++)
                {
                    var wordTFPerDocumentJ = frecuencyPerDocument[j] / (double)corpusWordFrequency;
                    wordTFPerDocuments[j] = wordTFPerDocumentJ;
                }
            }

            return tfMatrix;
        }

        public static Dictionary<string, double> CalculateIDF(SortedDictionary<string, List<int>> vocabulary, int corpusLenght)
        {
            var idfMatrix = new Dictionary<string, double>();
            // itera por palabras
            foreach (var word in vocabulary.Keys)
            {
                // lista de la frequencia de la palabra por documento
                var frecuencyPerDocument = vocabulary[word];

                // guarda el numero de documentos que la palabra aparece 
                var documentFrequency = frecuencyPerDocument.Count(f => f > 0);

                var idf = Math.Log10(corpusLenght / (double)documentFrequency + 1);

                idfMatrix.Add(word, idf);
            }

            return idfMatrix;
        }

        public static Dictionary<string, List<double>> CalculateTFIDF(Dictionary<string, List<double>> tfs, Dictionary<string, double> idfs, int corpusLenght)
        {
            var tfidfMatrix = new Dictionary<string, List<double>>();
            // itera por palabras
            foreach (var word in tfs.Keys)
            {
                // Create una entrada por palabra en el dictionario TFIDF con una lista de TFIDF inicializada en cero por cada documento en el corpus
                var tfidfPerDocuments = Enumerable.Repeat(0d, corpusLenght).ToList();

                // lista de TF de la palabra por cada documento 
                var tfPerDocument = tfs[word];

                for (var i = 0; i < corpusLenght; i++)
                {
                    //tf para el documento i
                    var tf = tfPerDocument[i];

                    // idf 
                    var idf = idfs[word];

                    var tfidf = tf * idf;

                    tfidfPerDocuments[i] = tfidf;
                }

                tfidfMatrix.Add(word, tfidfPerDocuments);
            }

            return tfidfMatrix;
        }
    }
}
