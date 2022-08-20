using System.Text.RegularExpressions;

namespace MoogleEngine
{
    static class Tools
    {
        public static string[] Tokenize(string text)
        {
            return RemoveAccent(text).Split(" @$/#.-:&+=[]*^~!?(){},''\">_<;%\\".ToCharArray()).Select(word => Regex.Replace(word, "[^a-zA-Z]", "").ToLower()).Where(word => word != "").ToArray();
        }

        public static string TokenizeWord(string text)
        {
            var str = RemoveAccent(text).ToLower();
            return Regex.Replace(str, "[^a-zA-Z]", "");
        }

        public static string[] TokenizeQuery(string text)
        {
            return RemoveAccent(text).Split(" @$/#.-:&+=[]?(){},''\">_<;%\\".ToCharArray()).Select(word => Regex.Replace(word, "[^a-zA-Z]", "").ToLower()).Where(word => word != "").ToArray();
        }
        private static string RemoveAccent(string text)
        {
            string result = text;
            result = result.Replace('á', 'a');
            result = result.Replace('é', 'e');
            result = result.Replace('í', 'i');
            result = result.Replace('ó', 'o');
            result = result.Replace('ú', 'u');

            return result;
        }
    }
}