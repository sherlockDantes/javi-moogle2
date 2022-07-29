using System;
using System.IO;

namespace Moogle_Project
{
    public class MyFile
    {
        string name;
        string directoryPath;
        int[] frecuencyOfWords;
        int[] totalWordsFrecuency;
        string[] corpusWords;
        public string Name
        {
            get { return name; }
        }
        public int[] FrecuencyOfWords
        {
            get { return frecuencyOfWords; }
        }
        public MyFile(string path)
        {
            this.directoryPath = @"./" + path.Split("./")[1].Split(@"\")[0];

            this.name = path.Split(this.directoryPath)[1].Split(@"\")[1];

            this.corpusWords = Tools.GetAllWords(this.directoryPath);

            this.frecuencyOfWords = Tools.GetFrecuencyOfWords(path, this.directoryPath);

            this.totalWordsFrecuency = Tools.GetAllDocsWordsFrecuency(this.directoryPath);
        }
        private double GetTF(string word)
        {
            for (int i = 0; i < corpusWords.Length; i++)
            {
                if (corpusWords[i] == word)
                {
                    return (double)frecuencyOfWords[i] / (double)totalWordsFrecuency[i];
                }
            }
            return 0;
        }

        public double GetTF_IDF(string word)
        {
            double idf = Tools.GetIDF(word, this.directoryPath);
            double tf = GetTF(word);

            return idf * tf;
        }

        public bool Contains(string word)
        {
            for (int i = 0; i < corpusWords.Length; i++)
            {
                if (corpusWords[i] == word)
                {
                    if(frecuencyOfWords[i] != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}