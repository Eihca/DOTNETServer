/*using F23.StringSimilarity;
using java.sql;*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
/*using static com.sun.tools.javac.code.Attribute;
using MySql.Data.MySqlClient;
using java.text;
*/
namespace DOTNETServer
{
    class NewsAggregator
    {
        
        int poolSize;
        List<string> urls;
        private static List<News> newsList = new List<News>();

        public List<News> getNews()
        {
            return newsList;
        }

        public NewsAggregator(int poolSize, List<string> urls)
        {
            this.poolSize = poolSize;
            this.urls = urls;
            ThreadPool.SetMaxThreads(poolSize, poolSize);
        }

        static int finishCount;

        public void Run()
        {


            finishCount = 0;
            int i = 0;
            foreach (string url in urls)
            {
                
                ThreadPool.QueueUserWorkItem(new WaitCallback(Process),
                    new ProcParam("" + i++, url));
            }
            // wait until all threads are finished
            while (finishCount < urls.Count)
            {
                Thread.Sleep(10);
            };
        }

        class ProcParam
        {
            public ProcParam(string name, string url)
            {
                this.name = name;
                this.url = url;
            }
            public string name;
            public string url;
        }


        static async void Process(object procParam)
        {
            ProcParam param = (ProcParam)procParam;
            NewsAgent agent = new NewsAgent(param.name, param.url);

            Console.WriteLine("Process " + param.url);

            newsList.AddRange(await agent.DownloadAsync());
            //newsList.ForEach(Console.WriteLine);

            finishCount++;
        }
    }
}
