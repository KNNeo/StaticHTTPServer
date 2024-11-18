using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

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
                    .Build();

            if (runAsService)
            {
                host.Run();
            }
            else
            {
                host.RunAsService();
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