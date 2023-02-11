using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System;

namespace StaticHTTPServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var runAsService = Environment.UserInteractive;

            var path = ConfigurationManager.AppSettings["FileDirectory"];
            if (args.Length > 0)
            {
                path = args[0];
            }

            var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseContentRoot(path)
                    .UseWebRoot(path)
                    .UseUrls(ConfigurationManager.AppSettings["ServerAddress"])
                    .UseStartup<Startup>()
                    .ConfigureLogging((hostingContext, logging) =>
                    {
                        //logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        logging.AddConsole();
                        //logging.AddDebug();
                    })
                    .Build();

            if (runAsService)
            {
                host.RunAsService();
            }
            else
            {
                host.Run();
            }
        }
    }
    internal class Startup
    {
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var fileServerOptions = new FileServerOptions();
            fileServerOptions.EnableDefaultFiles = true;
            fileServerOptions.EnableDirectoryBrowsing = true;
            fileServerOptions.FileProvider = env.WebRootFileProvider;
            fileServerOptions.StaticFileOptions.ServeUnknownFileTypes = true;
            app.UseFileServer(fileServerOptions);
        }
    }
}