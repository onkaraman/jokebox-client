using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using JokeBox.Core.Patterns;
using JokeBox.Models;

namespace JokeBox.Parsers
{
    public class APIParser : LazyStatic<APIParser>
    {
        public APIParser() {}

        /// <summary>
        /// Will take the result of a get request of jokes and
        /// parse the XML to a list of Joke objects.
        /// </summary>
        public List<Joke> ParseGet(string result)
        {
            List<Joke> jokes = new List<Joke>();

            try
            {
                XElement erg = XElement.Parse(result);
                IEnumerable<XElement> r = erg.DescendantsAndSelf("value");

                foreach (XElement sitem in r)
                {
                    Joke j = new Joke();
                    j.id = Convert.ToInt32(sitem.Element("id").Value);
                    j.upvotes = Convert.ToInt32(sitem.Element("upvotes").Value);
                    j.downvotes = Convert.ToInt32(sitem.Element("downvotes").Value);
                    j.composer = sitem.Element("composer").Value;
                    j.content = sitem.Element("content").Value;

                    jokes.Add(j);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debugger.Break();
            }

            return jokes;
        }
    
        /// <summary>
        /// Will take the result of the GetPoints-Request and return a parsed
        /// result.
        /// </summary>
        public int ParsePoints(string result)
        {
            int points = 0;
            try
            {
                XElement erg = XElement.Parse(result);
                IEnumerable<XElement> r = erg.DescendantsAndSelf("value");

                foreach (XElement sitem in r)
                {
                    points = Convert.ToInt32(r.ElementAt(0).Element("upvotes").Value);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return points;
        }
    }
}
