using Jokebox.Core.Resources;
using JokeBox.Core.Patterns;

namespace JokeBox.Core.Localization
{
    /// <summary>
    /// This class offers access to resource files for localization purposes.
    /// </summary>
    public class Localization : LazyStatic<Localization>
    {
        public Localization(){}

        /// <summary>
        /// Will return the resource value as it is stored in the resource file.
        /// </summary>
        public string Raw(string key)
        {
            return AppResources.ResourceManager.GetString(key);
        }

        /// <summary>
        /// Will return the resource value in lower case letters.
        /// </summary>
        public string Lower(string key)
        {
            return Raw(key).ToLower();   
        }

        /// <summary>
        /// Will return the resource value in capitalized form.
        /// </summary>
        public string Capitalized(string key)
        {
            string t = Lower(key);
            return t[0].ToString().ToUpper() + t.Substring(1).ToLower();
        }

        /// <summary>
        /// Will return a string with a period at the end of it.
        /// </summary>
        public string WithPeriod(string s)
        {
            if (s.EndsWith(".")) return s;
            return s + ".";
        }

        /// <summary>
        /// Will return the passed string without a period at the end of it.
        /// </summary>
        public string WithoutPerioud(string s)
        {
            if (s.EndsWith(".")) return s.Substring(0, s.Length - 1);
            return s;
        }
    }
}

