using System;

namespace JokeBox.API.Models
{
    public class Joke
    {
        public int id { get; set; }
        public string composer { get; set; }
        public string content { get; set; }
        public int upvotes { get; set; }
        public int downvotes { get; set; }

        public override string ToString()
        {
            return string.Format("#{0} {1}", id, content.Substring(0, 10));
        }
    }
}
