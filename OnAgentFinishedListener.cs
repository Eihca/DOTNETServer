using System.Collections.Generic;

namespace DOTNETServer
{
    interface OnAgentFinishedListener
    {
        void onFinished(List<News> newsList);
    }
}