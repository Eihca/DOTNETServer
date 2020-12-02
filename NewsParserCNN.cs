using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DOTNETServer
{
    class NewsParserCNN : NewsParser
    {

        List<News> NewsParser.extractNews(string content)
        {
            List<News> list = new List<News>();
            //DateTime format = new DateTime("yyyy/MM/d", Locale.US);
            int start = content.IndexOf("Latest Articles");
            int end = content.IndexOf("</section>", start);
            //https://stackoverflow.com/questions/10614870/index-and-length-must-refer-to-a-location-within-the-string-parameter-name-len/10615025#:~:text=It%20means%20that%20your%20ddlweek,Substring(index%2C%20length)%20.&text=It%20just%20means%20you're,than%2024%20characters%20in%20length).
            content = content.Substring(start, end-start+1);
            //Console.WriteLine(content);
            /*https://www.c-sharpcorner.com/article/c-sharp-regex-examples/
             https://regexone.com/references/csharp
            https://social.msdn.microsoft.com/Forums/en-US/0ef910bd-2431-4229-95dc-a5c6fe7a6af6/including-quotes-in-a-c-regex-pattern?forum=csharplanguage*/
            string pattern = @"<a href=""/news/(\d+/\d+/\d+)/(.+?)"">(.+?)</a>";
            Regex rg = new Regex(pattern, RegexOptions.ECMAScript );

            //skip non news item, the first
            MatchCollection matches = rg.Matches(content);

            foreach(Match match in matches)
            {
                //https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.match.groups?view=netcore-3.1
                News news = new News();
                news.title = match.Groups[3].Value;
                //0 whole match
                news.company = "CNNph";
                news.link = "https://cnnphilippines.com/news/" + match.Groups[1].Value + "/" + match.Groups[2].Value;
                try
                {
                    news.date = Convert.ToDateTime(String.Format("{0:d/M/yyyy HH:mm:ss}", match.Groups[1].Value));
                }
                catch (Exception e)
                {
                    Console.WriteLine("NewsParserCNN " + e.Message);
                }
                list.Add(news);
                //Console.WriteLine("NewsParserCNN "  + news.toString());
            }

            return list;
        }
    }
}
