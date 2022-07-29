using System;

namespace Moogle_Project
{
    static class Tools
    {
        public static double GetIDF(string word, string directoryPath)
        {
            string[] myFilesDirectory = Directory.GetFiles(directoryPath);

            string path = myFilesDirectory[0];

            MyFile[] allFiles = new MyFile[myFilesDirectory.Length];

            for (int i = 0; i < myFilesDirectory.Length; i++)
            {
                allFiles[i] = new MyFile(myFilesDirectory[i]);
            }

            int totalNumberOfFiles = myFilesDirectory.Length;
            int filesContainingTheWord = 0;

            for (int i = 0; i < allFiles.Length; i++)
            {
                if (allFiles[i].Contains(word))
                {
                    filesContainingTheWord++;
                }
            }

            return Math.Log10((double)totalNumberOfFiles / (double)filesContainingTheWord);
        }
        public static string[] GetAllWords(string directoryPath)
        {
            string[] result;

            string[] myFilesDirectory = Directory.GetFiles(directoryPath);

            string allWordsStr = "";

            for (int i = 0; i < myFilesDirectory.Length; i++)
            {
                allWordsStr += GetString(CreateFileArray(myFilesDirectory[i])) + " ";
            }

            result = Tools.GetNonRepeatedWords(Tools.GetCleanedWords(allWordsStr));

            return result;
        }

        public static int[] GetAllDocsWordsFrecuency(string directoryPath)
        {
            string[] cleanedWords = GetAllWordsRepeated(directoryPath);

            string[] allWords = GetAllWords(directoryPath);

            int[] frecuencyOfWords = new int[allWords.Length];

            int count;
            for (int i = 0; i < allWords.Length; i++)
            {
                count = 0;
                for (int j = 0; j < cleanedWords.Length; j++)
                {
                    if (allWords[i] == cleanedWords[j])
                    {
                        count++;
                    }
                }
                frecuencyOfWords[i] = count;
            }

            return frecuencyOfWords;
        }
        public static int[] GetFrecuencyOfWords(string path, string directoryPath)
        {
            string[] allLines = File.ReadAllLines(path);

            string allWordsStr = Tools.GetString(allLines);

            string[] cleanedWords = Tools.GetCleanedWords(allWordsStr);

            string[] allWords = Tools.GetAllWords(directoryPath);

            int[] frecuencyOfWords = new int[allWords.Length];

            int count;
            for (int i = 0; i < allWords.Length; i++)
            {
                count = 0;
                for (int j = 0; j < cleanedWords.Length; j++)
                {
                    if (allWords[i] == cleanedWords[j])
                    {
                        count++;
                    }
                }
                frecuencyOfWords[i] = count;
            }

            return frecuencyOfWords;
        }

        private static string[] GetAllWordsRepeated(string directoryPath)
        {
            string[] result;

            string[] myFilesDirectory = Directory.GetFiles(directoryPath);

            string allWordsStr = "";

            for (int i = 0; i < myFilesDirectory.Length; i++)
            {
                allWordsStr += GetString(CreateFileArray(myFilesDirectory[i])) + " ";
            }

            result = Tools.GetCleanedWords(allWordsStr);

            return result;
        }
        private static string[] CreateFileArray(string path)
        {
            string[] fileArray = File.ReadAllLines(path);

            return fileArray;
        }
        private static string GetString(string[] arr)
        {
            string result = " ";

            for (int i = 0; i < arr.Length; i++)
            {
                result += arr[i] + " ";
            }

            return result;
        }
        private static string[] GetCleanedWords(string words)
        {
            string[] wordsWithoutSpace;
            string[] cleanedWords;

            // For string[] parameter
            // string words = "";

            // for (int i = 0; i < lines.Length; i++)
            // {
            //     words += lines[i] + " ";
            // } 

            wordsWithoutSpace = words.Split(" ");

            cleanedWords = new string[wordsWithoutSpace.Length];

            for (int i = 0; i < cleanedWords.Length; i++)
            {
                cleanedWords[i] = wordsWithoutSpace[i].Split(".")[0].Split("!")[0].Split("?")[0].Split("¿")[0].Split("\"")[0].Split("\'")[0].Split(",")[0].Split(":")[0].Split("...")[0].Split(" ")[0].Split("  ")[0].ToLower();
            }

            return cleanedWords;
        }

        private static string[] GetNonRepeatedWords(string[] cleanedWords)
        {
            string[] result;
            string[] seudoResult = new string[cleanedWords.Length];

            for (int i = 0; i < seudoResult.Length; i++)
            {
                if (seudoResult.Contains(cleanedWords[i]))
                {
                    continue;
                }

                seudoResult[i] = cleanedWords[i];
            }

            int nullSpaces = 0;
            for (int i = 0; i < seudoResult.Length; i++)
            {
                if (seudoResult[i] == null)
                {
                    nullSpaces++;
                }
            }

            result = new string[(seudoResult.Length - 1) - nullSpaces];

            int myExternalIndex = 0;
            for (int i = 0; i < seudoResult.Length; i++)
            {
                if (seudoResult[i] == null || seudoResult[i].Length == 0)
                {
                    continue;
                }

                result[myExternalIndex] = seudoResult[i];
                myExternalIndex++;
            }

            return result;
        }
    }
}