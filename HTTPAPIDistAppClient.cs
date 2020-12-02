using System;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

//Combined from SimpleChat and this link that showed how to http post json  in c# http://zetcode.com/csharp/httpclient/
namespace DOTNETServer
{
    class HttpAPIDistAppClient
    {
        const int PORT = 8088;

        public HttpAPIDistAppClient()
        {
            

        }


        public async void read()
        {
            Console.WriteLine("reacehd");

            List<string> urls = new List<string>();
            using (StreamReader reader = new StreamReader("urls.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    urls.Add(line);
                }
            }

            urls.ForEach(Console.WriteLine);
            
            var json = JsonConvert.SerializeObject(urls);
            var data = new StringContent(json, Encoding.UTF8, "application/json");


            var url = "http://localhost" + ":" + PORT;
            using var client = new HttpClient();
            Console.WriteLine(url);

            var response = client.PostAsync(url, data).Result;
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                Console.WriteLine(response);
            }

            response.EnsureSuccessStatusCode();
            var resp = await response.Content.ReadAsStringAsync();

            List<News> news = JsonConvert.DeserializeObject<List<News>>(resp);
            news.ForEach(Console.WriteLine);
            

        }



    }
}