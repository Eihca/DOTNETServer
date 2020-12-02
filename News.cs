using System;
using System.Collections.Generic;
using System.Text;

namespace DOTNETServer
{
    public class News
    {
        public String company;
        public DateTime date;
        public String title;
        public String summary;
        public String link;
        public String content;

        public override string ToString()
        {
            return $"Title: {title} \n" +
                $"Company: {company} \n" +
                $"Date: {date} \n" +
                $"Link: {link} \n" +
                $"Content: {content}\n_______________________________________";
        }
    }
}
