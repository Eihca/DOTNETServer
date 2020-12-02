using System;
using System.Collections.Generic;
using System.Text;

namespace DOTNETServer
{
    interface NewsParser
    {
        List<News> extractNews(String content);
    }
}
