
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DOTNETServer
{
    class NewsParserPNA: NewsParser
    {
        List<News> NewsParser.extractNews(String content)
        {
            List<News> list = new List<News>();
            
            int start = content.IndexOf("Latest News");
            int end = content.IndexOf("</ul>", start);
            content = content.Substring(start, end-start+1);

            string pattern = @"<a class=""title"" href=""(.+?)"">(.+?)</a><br>\s+<span class=""date"">Date Posted: (.+?)</span>";
            //Console.WriteLine(content);
            Regex rg = new Regex(pattern, RegexOptions.Singleline);
            MatchCollection matches = rg.Matches(content);
            foreach (Match match in matches)
            {
                News news = new News();
                news.title = match.Groups[2].Value;
                //0 whole match
                news.company = "PNA";
                news.link = match.Groups[1].Value;
                try
                {
                    news.date = Convert.ToDateTime(String.Format("{0:d/M/yyyy HH:mm:ss}", match.Groups[3].Value));
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e.Message);
                }
                list.Add(news);
                //Console.WriteLine(news.toString());
            }
            return list;
        }
    }
}
