using MoogleEngine;
using System;
using System.IO;

namespace Moogle_Project
{
    class Program
    {
        static void Main()
        {
            var folderPath = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "Documents");
            string[] documentNames;
            var wordsPerDocument = DocumentManager.ReadAllTextFiles(folderPath, out documentNames);
            //var tfidf = new TFIDF();
            //tfidf.CreateVocabulary(wordMatrix);
        }
    }
}