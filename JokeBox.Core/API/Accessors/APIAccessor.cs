using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JokeBox.Core.Patterns;

namespace API.Accessors
{
    public class APIAccessor : LazyStatic<APIAccessor>
    {
        private string _baseURL;
        private HttpClient _client;

        public APIAccessor()
        {
            _baseURL = "http://areondev.de/nexus/nexus.php";
            _client = new HttpClient();
        }

        /// <summary>
        /// Will return a list of Jokes from the API.
        /// </summary>
        /// <param name="username">The username of this user. Needed in order to filter out own jokes.</param>
        /// <param name="countryCode">The localization code for the jokes.</param>
        public async Task<string> Get(string username, string countryCode)
        {
            string url = _baseURL + string.Format("?jokebox_get&user={0}&ccode={1}", username, countryCode);
            try
            {
                var response = await _client.GetByteArrayAsync(url);
                return Encoding.GetEncoding("ISO-8859-1").GetString(response, 0, response.Length);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Will upvote or downvote a joke.
        /// </summary>
        /// <param name="up">Will upvote joke if true, downvote if false.</param>
        /// <param name="id">ID of the Joke.</param>
        /// <param name="countryCode">The localization code for the joke.</param>
        public string Vote(bool up, int id, string countryCode)
        {
            string method = "upvote";
            if (!up) method = "downvote";

            string url = _baseURL + string.Format("?jokebox_{0}&ccode={1}&id={2}", method, countryCode, id);
            try
            {
                HttpResponseMessage res = _client.GetAsync(url).Result;
                return res.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null; 
        }
    
        /// <summary>
        /// Will delete a joke from the database.
        /// </summary>
        public string Delete(string id, string countryCode)
        {
            string url = _baseURL + string.Format("?jokebox_delete&ccode={0}&id={1}", countryCode, id);
            try
            {
                HttpResponseMessage res = _client.GetAsync(url).Result;
                return res.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Will report a joke and mark it as such (incrementing).
        /// </summary>
        public string Report(string id, string countryCode)
        {
            string url = _baseURL + string.Format("?jokebox_report&ccode={0}&id={1}", countryCode, id);
            try
            {
                HttpResponseMessage res = _client.GetAsync(url).Result;
                return res.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Will get the total points of the user.
        /// </summary>
        public string GetPoints(string username, string countryCode)
        {
            string url = _baseURL + string.Format("?jokebox_getpoints&ccode={0}&composer={1}", countryCode, username);
            try
            {
                HttpResponseMessage res = _client.GetAsync(url).Result;
                return res.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }
    
        /// <summary>
        /// Will submit a joke to the database.
        /// </summary>
        public string Submit(string username, string content, string countryCode)
        {
            string url = _baseURL + string.Format("?jokebox_submit&ccode={0}&composer={1}&content={2}", countryCode, username, content);
            try
            {
                HttpResponseMessage res = _client.GetAsync(url).Result;
                return res.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
