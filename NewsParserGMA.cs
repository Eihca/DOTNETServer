
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DOTNETServer
{
    class NewsParserGMA : NewsParser
    {
        List<News> NewsParser.extractNews(String content)
        {
            List<News> list = new List<News>();
          
            int start = content.IndexOf("<item>");
            int end = content.IndexOf("</channel>", start);
            content = content.Substring(start, end-start+1);

            //Console.WriteLine(content);
            string pattern = @"<item> <title><!\[CDATA\[(.+?)\]\]></title>(.+?)<link>(.+?)</link> <description><!\[CDATA\[\]\]></description> <pubDate>(.+?)</pubDate>";
            Regex rg = new Regex(pattern);
            
            MatchCollection matches = rg.Matches(content);
            foreach(Match match in matches)
            {
                News news = new News();
                news.title = match.Groups[1].Value;
                //0 whole match
                news.company = "GMA";
                news.link = match.Groups[3].Value;
                try
                {
                    news.date = Convert.ToDateTime(String.Format("{0:d/M/yyyy HH:mm:ss}", match.Groups[4].Value));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                list.Add(news);


                Console.WriteLine("gma " + news.ToString());
            }
            return list;
        }

    }
}
