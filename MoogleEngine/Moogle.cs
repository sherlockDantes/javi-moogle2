using System.Reflection;
using System.Reflection.Metadata;

namespace MoogleEngine;


public static class Moogle
{

    public static SearchResult Query(string query)
    {
        // Modifique este método para responder a la búsqueda
        try
        {

            string[] documentNames;
            var folderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Documents");
            var vocabulary = DocumentManager.ReadAllTextFiles(folderPath, out documentNames);
            var tfMatrix = TFIDF.CalculateTF(vocabulary, documentNames.Length);
            var idfMatrix = TFIDF.CalculateIDF(vocabulary, documentNames.Length);
            var tfidfMatrix = TFIDF.CalculateTFIDF(tfMatrix, idfMatrix, documentNames.Length);

            var searchterms = new List<SearchTerm>();

            var searchTerm = new SearchTerm();
            foreach (var str in query.ToLower().Split().Select(s => s.Trim()))
            {
                if (str.StartsWith('!') || str.StartsWith('^'))
                {
                    searchTerm.Operador = str[..1];
                    searchTerm.Term = str[1..];
                    searchterms.Add(searchTerm);
                }
                else
                {
                    searchTerm.Term = str;
                    searchterms.Add(searchTerm);
                }
            }

            var meaningfulSearchTerms = new List<SearchTerm>();

            foreach (var term in searchterms)
            {
                if (vocabulary.ContainsKey(term.Term) && tfidfMatrix[term.Term].Any(tfidf => tfidf > 0.1))
                {
                    meaningfulSearchTerms.Add(term);
                }
            }

            var items = new List<SearchItem>();

            foreach (var meaningfulSearch in meaningfulSearchTerms)
            {
                if (string.IsNullOrEmpty(meaningfulSearch.Operador))
                {
                    if (vocabulary.ContainsKey(meaningfulSearch.Term))
                    {
                        var tfidfs = tfidfMatrix[meaningfulSearch.Term];
                        for (int i = 0; i < tfidfs.Count; i++)
                        {
                            if (tfidfs[i] > 0)
                            {
                                var snippet = DocumentManager.GetSnippet(folderPath, documentNames[i], meaningfulSearch.Term);
                                items.Add(new SearchItem(documentNames[i], snippet, 1f));
                            }
                        }
                    }
                }
                else if (meaningfulSearch.Operador == "!")
                {
                    // Si algun documento tiene un tfidf > 0, significa que el termino se encuentra al menos en un documento, y sera descartado por el uso del operador !
                    if (vocabulary.ContainsKey(meaningfulSearch.Term) && tfidfMatrix[meaningfulSearch.Term].Any(tfidf => tfidf > 0))
                    {

                    }
                }
                else if (meaningfulSearch.Operador == "^")
                {
                    // El termino se encuentra al todos los documentos
                    if (vocabulary.ContainsKey(meaningfulSearch.Term) && tfidfMatrix[meaningfulSearch.Term].All(tfidf => tfidf > 0))
                    {
                        foreach (var document in documentNames)
                        {
                            var snippet = DocumentManager.GetSnippet(folderPath, document, meaningfulSearch.Term);
                            items.Add(new SearchItem(document, snippet, documentNames.Length));
                        }
                    }
                }
                else
                {

                }
            }
            return new SearchResult(items, query)
            {
                DocumentNames = documentNames,
                Vocabulary = vocabulary,
                TFMatrix = tfMatrix,
                IDFMatrix = idfMatrix,
                TFIDFMatrix = tfidfMatrix
            };
        }
        catch (Exception ex)
        {

            return null;
        }
    }

    private static string[] ProcessQuery(string query)
    {
        string[] terms = query.ToLower().Split();
        return terms;
    }
}