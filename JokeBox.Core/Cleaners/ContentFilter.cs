using System;
using JokeBox.Core.Patterns;

namespace Jokebox.Core.Helpers
{
    public class ContentFilter : LazyStatic<ContentFilter>
    {
        public ContentFilter(){}

        public bool IsGibberish(string s)
        {
            int count = 0;
            char lastChar = ' ';

            for (int i = 0; i < s.Length; i+=1 )
            {
                if (count > 2) return true;

                if (s[i] == lastChar && !lastChar.Equals("\n")) count += 1;
                else
                {
                    lastChar = s[i];
                    count = 0;
                }
            }
            return false;
        }

        public string CleanUp(string s)
        {
            string[] detect = { "(", ")", "[", "]", "<", ">", ";" };

            foreach (string d in detect) s = s.Replace(d, "");

            string[] olds = { "Ğ", "ğ", "Ş", "ş", "İ", "ı" };
            string[] news = { "G", "g", "S", "s", "I", "i" };

            for (int i = 0; i < olds.Length; i += 1) s = s.Replace(olds[i], news[i]);

            return s;
        }
    }
}
