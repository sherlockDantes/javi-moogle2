namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query)
    {
        // Modifique este método para responder a la búsqueda
        string folderPath = @"D:\Work\Businnes\CSharp\Moogle Project\Moogle Project Original 2.0\Content";
        string[] files = Directory.GetFiles(folderPath);
        var corpus = Core.GetCorpus(folderPath);
        var tf_idfMatrix = Core.GetTF_IDF_MatrixNormalized(corpus, files);
        var queryVector = GetTF_IDFQueryNormalized(query, corpus);
        var cosines = GetCosineSimilarity(queryVector, tf_idfMatrix);


        // SearchItem[] items = new SearchItem[3] {
        //     new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.9f),
        //     new SearchItem("Hello World", "Lorem ipsum dolor sit amet", 0.5f),
        //     new SearchItem("Hello world", "Lorem ipsum dolor sit amet", 0.95f),
        // };

        return new SearchResult(GetSearchItems(cosines));
    }

    static SearchItem[] GetSearchItems(SortedDictionary<float, string> cosines)
    {
        var searchItems = new SearchItem[cosines.Count];

        for (int i = 0; i < cosines.Count; i++)
        {
            searchItems[(cosines.Count - 1) - i] = new SearchItem(cosines.Values.ElementAt(i), "",cosines.Keys.ElementAt(i));
        }

        return searchItems;
    }
    // static string GetSnippet(string fileName, string folderPath, Dictionary<string, float[]> tf_idfMatrix, SortedDictionary<string, WordData> corpus)
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
    static float[] GetTF_IDFQueryNormalized(string query, SortedDictionary<string, WordData> corpus)
    {
        var terms = Tools.TokenizeQuery(query);

        float[] tf_idfNormalized = new float[corpus.Count];

        for (int i = 0; i < terms.Length; i++)
        {
            if (corpus.ContainsKey(terms[i]))
            {
                tf_idfNormalized[corpus.Keys.ToList().IndexOf(terms[i])] += corpus[terms[i]].IDF;
            }
        }

        float powers = 0;
        for (int i = 0; i < tf_idfNormalized.Length; i++)
        {
            powers += (float)Math.Pow(tf_idfNormalized[i], 2);
        }

        for (int i = 0; i < tf_idfNormalized.Length; i++)
        {
            if (powers == 0)
            {
                tf_idfNormalized[i] = 0;
            }
            else
            {
                tf_idfNormalized[i] = tf_idfNormalized[i] * (1 / powers);
            }
        }

        return tf_idfNormalized;
    }
    static SortedDictionary<float, string> GetCosineSimilarity(float[] query, Dictionary<string, float[]> tf_idf_Matrix)
    {
        var cosineValues = new SortedDictionary<float, string>();

        foreach (var pair in tf_idf_Matrix)
        {
            float cosine = DotProduct(query, pair.Value) / (Norm(query) * Norm(pair.Value));
            if (cosine == 0 || cosine == float.NaN || cosine < 0.0009f)
            {
                continue;
            }
            else if (cosineValues.ContainsKey(cosine))
            {
                cosine -= 0.0001f;
                cosineValues.Add(cosine, pair.Key);
            }
            else
            {
                cosineValues.Add(cosine, pair.Key);
            }
        }

        return cosineValues;
    }

    static float DotProduct(float[] firstVector, float[] secondVector)
    {
        float result = 0;
        for (int i = 0; i < firstVector.Length; i++)
        {
            result += firstVector[i] * secondVector[i];
        }

        return result;
    }

    static float Norm(float[] vector)
    {
        float powers = 0;

        for (int i = 0; i < vector.Length; i++)
        {
            powers += (float)Math.Pow(vector[i], 2);
        }

        return (float)Math.Sqrt(powers);
    }
}
