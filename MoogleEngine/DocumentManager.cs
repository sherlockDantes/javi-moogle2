using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MoogleEngine
{
    public static class DocumentManager
    {
        public static SortedDictionary<string, List<int>> ReadAllTextFiles(string folderPath, out string[] documentNames)
        {
            documentNames = null;
            try
            {
                var vocabulary = new SortedDictionary<string, List<int>>();
                var fileNames = Directory.GetFiles(folderPath);
                documentNames = fileNames.Select(filename => Path.GetFileName(filename)).ToArray();

                for (var i = 0; i < fileNames.Length; i++)
                {
                    Debug.WriteLine($"Processing {fileNames[i]}");
                    var documentWords = Tokenize(File.ReadAllText(Path.Combine(folderPath, fileNames[i])));

                    foreach (var word in documentWords)
                    {
                        List<int> frequencyByDocument;
                        if (vocabulary.ContainsKey(word))
                        {
                            frequencyByDocument = vocabulary[word];
                        }
                        else
                        {
                            frequencyByDocument = Enumerable.Repeat(0, fileNames.Length).ToList();

                            vocabulary.Add(word, frequencyByDocument);
                        }

                        frequencyByDocument[i]++;
                    }

                }

                return vocabulary;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public static string GetSnippet(string folderPath, string fileName, string term)
        {
            try
            {
                string snippet = string.Empty;

                var filePath = Path.Combine(folderPath, fileName);
                using (var reader = new StreamReader(filePath))
                {
                    var line = reader.ReadLine();
                    while (line != null)
                    {
                        if (line.Contains(term, StringComparison.InvariantCultureIgnoreCase))
                        {
                            snippet = line;
                            break;
                        }
                        line = reader.ReadLine();
                    }
                }

                return snippet;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        // Separa y limpia las palabras
        private static string[] Tokenize(string text)
        {
            // Quita las puntuaciones y los numeros
            return text.Split(" @$/#.-:&+=[]?(){},''\">_<;%\\".ToCharArray()).Select(word => Regex.Replace(word, "[^a-zA-Z]", "").ToLower()).Where(word => word != "").ToArray();
        }
    }
}