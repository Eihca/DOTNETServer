using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DOTNETServer
{
    class NewsParserABSCBN : NewsParser
    {

        List<News> NewsParser.extractNews(string content) {

            List<News> list = new List<News>();
         
            int start = content.IndexOf("content tabs-inner current");
            int end = content.IndexOf("</ul>", start);
            content = content.Substring(start, end-start+1);

            //string pattern = @"<a href= ""/(.+?)/(\d+/\d+/\d+)/(.+?)"">(.+?)</a>";

            string pattern = @"<a href=""/(.+?)/(\d+/\d+/\d+)/(.+?)"">(.+?)</a>";
            Regex rg = new Regex(pattern);

            Console.WriteLine(content);
            MatchCollection matches = rg.Matches(content);
            foreach(Match match in matches){
                News news = new News();
                news.title = match.Groups[4].Value;
                
                news.company = "ABS-CBN";
                news.link = "https://news.abs-cbn.com/" + match.Groups[1].Value + "/" + match.Groups[2].Value + "/" + match.Groups[3].Value;
                try {
                    news.date = Convert.ToDateTime(String.Format("{0:d/M/yyyy HH:mm:ss}", match.Groups[2].Value));
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
                list.Add(news);


                Console.WriteLine("abs " + news.ToString());
            }
            return list;
        }
    }
}
