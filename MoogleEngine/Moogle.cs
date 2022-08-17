using System;
using System.Diagnostics;

namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query)
    {
        var sw = new Stopwatch();
        sw.Start();
        // Modifique este método para responder a la búsqueda
        string folderPath = @"C:\Users\dalon\source\repos\JavierMoogleProject\Documents";

        string[] files = Directory.GetFiles(folderPath);

        var corpus = Core.GetCorpus(files);
        var queryData = Core.GetQueryData(query, corpus);

        var tfidfMatrix = new List<double[]>();

        var totalDocuments = corpus.First().Value.TFIDF.Length;
        var totalTerms = queryData.Keys.Count;

        var kvp = corpus.Where(x => queryData.ContainsKey(x.Key)).Select(x => x).ToList();

        for (var i = 0; i < totalDocuments; i++)
        {
            var tfidf = new double[totalTerms];
            for (var j = 0; j < kvp.Count; j++)
            {
                tfidf[j] = kvp[j].Value.TFIDF[i];
            }
            tfidfMatrix.Add(tfidf);
        }
        //var queryVector = GetTF_IDFQueryNormalized(query, corpus);
        double[] cosines = GetCosineSimilarity(queryData.Values.Select(x=> x.TFIDF).ToArray(), tfidfMatrix);

        var result = GetSearchItems(files, cosines);

        sw.Stop();
        
        return new SearchResult(GetSearchItems(files, cosines)) { Seconds = sw.Elapsed.Seconds};
    }

    static SearchItem[] GetSearchItems(string[] files, double[] cosines)
    {
        var searchItems = new SearchItem[cosines.Length];

        for (int i = 0; i < files.Length; i++)
        {
            searchItems[i] = new SearchItem(Path.GetFileName(files[i]), "", cosines[i]);
        }

        return searchItems;
    }
    // static string GetSnippet(string fileName, string folderPath, Dictionary<string, double[]> tf_idfMatrix, SortedDictionary<string, WordData> corpus)
    // {
    //     var reader = new StreamReader(Path.Combine(folderPath, fileName));
    //     string word = corpus.Keys.ToList().ElementAt(tf_idfMatrix[fileName].ToList().IndexOf(tf_idfMatrix[fileName].Max()));

    //     var line = reader.ReadLine();
    //     while (reader.ReadLine() != null)
    //     {
    //         if (Tools.Tokenize(line).Contains(word))
    //         {
    //             return line;
    //         }
    //         else
    //         {
    //             line = reader.ReadLine();
    //         }
    //     }
    //     return string.Empty;
    // }

    static double[] GetCosineSimilarity(double[] queryVector, List<double[]> tfidfMatrix)
    {
        var cosineValues = new double[tfidfMatrix.Count];

        // iterate for every document
        for (var i = 0; i < tfidfMatrix.Count; i++)
        {
            var documentTFIDFVector = tfidfMatrix[i];
            cosineValues[i] = DotProduct(queryVector, documentTFIDFVector) / (Norm(queryVector) * Norm(documentTFIDFVector));
        }

        return cosineValues;
    }

    static double DotProduct(double[] firstVector, double[] secondVector)
    {
        double result = 0;
        for (int i = 0; i < firstVector.Length; i++)
        {
            result += firstVector[i] * secondVector[i];
        }

        return result;
    }

    static double Norm(double[] vector)
    {
        double powers = 0;

        for (int i = 0; i < vector.Length; i++)
        {
            powers += (double)Math.Pow(vector[i], 2);
        }

        return (double)Math.Sqrt(powers);
    }
}