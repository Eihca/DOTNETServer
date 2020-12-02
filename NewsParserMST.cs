
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DOTNETServer
{
    class NewsParserMST : NewsParser
    {
         List<News> NewsParser.extractNews(String content)
        {
            List<News> list = new List<News>();
          
            int start = content.IndexOf("<item>");
            int end = content.IndexOf("</channel>", start);
            content = content.Substring(start, end-start+1);

            //Console.WriteLine(content);
            string pattern = @"<item><title><!\[CDATA\[(.+?)\]\]></title><link><!\[CDATA\[(.+?)\]></link>(.+?)<pubDate>(.+?)</pubDate>";
            Regex rg = new Regex(pattern);
            MatchCollection matches = rg.Matches(content);
            foreach(Match match in matches)
            {
                News news = new News();
                news.title = match.Groups[1].Value;
                //0 whole match
                news.company = "Manila Standard Times";
                news.link = match.Groups[2].Value;
                try
                {
                    news.date = Convert.ToDateTime(String.Format("{0:d/M/yyyy HH:mm:ss}", match.Groups[4].Value));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                list.Add(news);


                //Console.WriteLine(news.toString());
            }
            return list;
        }
    }
}
