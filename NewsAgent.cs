
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Console = System.Console;

namespace DOTNETServer{
    class NewsAgent {
        private string name;
        private string url;

        private Url httpUrl;

        private string urlstr;
        private NewsParser newsParser;
        private List<News> newsList = new List<News>();
      

        public NewsAgent(string name, string url) {
            this.name = name;
            this.url = url;
            urlstr = url;
            
            if (urlstr.StartsWith("https://data.gmanetwork.com/"))
            {
                newsParser = new NewsParserGMA();
            }
            else if (urlstr.StartsWith("https://news.abs-cbn.com/"))
            {
                newsParser = new NewsParserABSCBN();
            }
            else if (urlstr.StartsWith("https://cnnphilippines.com/"))
            {
                newsParser = new NewsParserCNN();
            }
            else if (urlstr.StartsWith("https://www.pna.gov.ph/"))
            {
                newsParser = new NewsParserPNA();
            }
            else if (urlstr.StartsWith("https://manilastandard.net/"))
            {
                newsParser = new NewsParserMST();
            }
        }

        internal async Task<List<News>> DownloadAsync() {
            using var client = new HttpClient();

            /*http://zetcode.com/csharp/readwebpage/
            client.DefaultRequestHeaders.Add("User-Agent", "C# console program");
            var content = await client.GetStringAsync(url);*/

            //https://stackoverflow.com/questions/49828449/the-character-set-provided-in-contenttype-is-invalid
            client.DefaultRequestHeaders.Clear();
            string s = null;

            var result = await client.GetAsync(url);
            using (var sr = new StreamReader(await result.Content.ReadAsStreamAsync(), Encoding.GetEncoding("iso-8859-1")))
            {
                s = sr.ReadToEnd();
            }

            try
            {
                newsList = new List<News>();
            
                if (newsParser != null)
                {
                    Console.WriteLine("Reaches here");
                    newsList = newsParser.extractNews(s.ToString());
                }
                else
                {
                    newsList = new List<News>();
                    //Console.WriteLine("NewsAgent:  NO PARSER" + url  );
                }
               

            }
            catch (Exception e)
            {
                Console.WriteLine("NewsAgent: " + e.Message);
            }
            return newsList;

        }


    }
}
