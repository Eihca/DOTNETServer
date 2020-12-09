using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DOTNETServer
{
    class DotNETCoreServer

    {
        const int PORT = 8088;
        string serverHost;
        string gson;
        private static string server;
        TcpClient socketForServer = null;
        private static NetworkStream networkStream;
        private static Socket socketForClient;

        public static IWebHost CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Warning))
                .UseUrls("http://0.0.0.0:8088/")
                .Build();

        static void Main(string[] args)
        {

            Console.WriteLine("Enter host server (blank if this is the server):");
            server = Console.ReadLine();
            Console.WriteLine();
            TcpListener tcpListener = null;
            socketForClient = null;
            TcpClient socketForServer = null;
            networkStream = null;
            IPAddress ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
            Console.WriteLine(Dns.GetHostName());
            try
            {
                if (server.Equals(""))
                {	                // this is the server
/*                    Console.WriteLine("Server Listening for connections ...");
                    tcpListener = new TcpListener(ipAddress, PORT);
                    // listen for client connection (this auto blocks)
                    tcpListener.Start();
                    socketForClient = tcpListener.AcceptSocket();
                    if (socketForClient.Connected)
                    {
                        Console.WriteLine("Server accepted connection from " + socketForClient.AddressFamily.ToString());
                        networkStream = new NetworkStream(socketForClient);
                    }*/

                    Console.WriteLine("Starting server...");
                    var builder = CreateWebHostBuilder(args);
                    /*            var task = Task.Run(() => {*/
                    builder.Run();
                    Console.WriteLine("Server started");
                    /*});*/
                }
                else
                {                   // this is a client
                    Console.WriteLine("Connecting to server ...");
                    socketForServer = new TcpClient(server, PORT);
                    Console.WriteLine("Connected to server " + socketForServer.Client.AddressFamily.ToString());
                    var httpclient = new HttpAPIDistAppClient();
                    httpclient.read();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }


        }
    }
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddRouting();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                /* var rb = new RoutingBuilder(app);
                rb.Routes.Add(new NewsRouter(), "news", app.ApplicationServices.GetService<IInlineConstraintResolver>());*/

                //extract params
                Console.WriteLine(context.Request.Path);
                var path = context.Request.Path;

                if(path == "/news")
                {

                    var reader = new StreamReader(context.Request.Body);
                    var param = await reader.ReadToEndAsync();
                    Console.WriteLine(param);
                    List<string> urls = JsonConvert.DeserializeObject<List<string>>(param);
                    Console.WriteLine(urls);

                    NewsAggregator aggr = new NewsAggregator(6, urls);

                    aggr.Run();
                    List<News> newslist = new List<News>();
                    newslist = aggr.getNews();
                    newslist.ForEach(Console.WriteLine);
                    /* News news = new News();
                     news.company = "GMA";
                     news.date = DateTime.Now.ToString();
                     news.title = "Sample News" +param;
                     news.link = "http://tryingthis.com";
                     newslist.Add(news);*/
                    var json = JsonConvert.SerializeObject(newslist);
                    var data = Encoding.UTF8.GetBytes(json);

                    //var data = System.Text.Encoding.UTF8.GetBytes("Hello World from the ASP.Net CORE!");
                    context.Response.Body.Write(data, 0, data.Length);
                }
                else if(path == "/capitalize")
                {

                    var reader = new StreamReader(context.Request.Body);
                    var param = await reader.ReadToEndAsync();
                    Console.WriteLine(param);
                    List<string> cap = JsonConvert.DeserializeObject<List<string>>(param);
                    
                    for(int c=0;c<cap.Count; c++)
                    {
                        cap[c] = cap[c].ToUpper();
                    }

                    var json = JsonConvert.SerializeObject(cap);
                    var data = Encoding.UTF8.GetBytes(json);

                    context.Response.Body.Write(data, 0, data.Length);
                }
                else if (path == "/home" || path == "/")
                {
                    var wel = "Welcome to News Parser";

                    var data = Encoding.UTF8.GetBytes(wel);

                    context.Response.Body.Write(data, 0, data.Length);
                }
                else
                {
                    var inv = "Invalid URL";
                
                    var data = Encoding.UTF8.GetBytes(inv);

                    context.Response.Body.Write(data, 0, data.Length);
                }


                
            });
            //return Task.CompletedTask;
        }

/*        public class NewsRouter: IRouter
        {
            public VirtualPathData GetVirtualPath(VirtualPathContext context)
            {
                return null;
            }

            public Task RouteAsync(RouteContext context)
            {
                List<News> newslist = new List<News>();
                News news = new News();
                news.company = "GMA";
                news.date = DateTime.Now.ToString();
                news.title = "Sample News";
                news.link = "http://tryingthis.com";
                newslist.Add(news);
                var json = JsonConvert.SerializeObject(newslist);
                var data = Encoding.UTF8.GetBytes(json);

                //var data = System.Text.Encoding.UTF8.GetBytes("Hello World from the ASP.Net CORE!");
                context.HttpContext.Response.Body.WriteAsync(data, 0, data.Length);

                return Task.FromResult(0);
            }
        }*/
    }

}
