using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace aspnetapp {
    public class Program {
        public static void Main (string[] args) {
            string port = "5000";
            if (Environment.GetEnvironmentVariable ("PORT") != "") {
                port = Environment.GetEnvironmentVariable ("PORT");
            }
            CreateWebHostBuilder (port, args).Build ().Run ();
        }
        public static IWebHostBuilder CreateWebHostBuilder (string port, string[] args) =>
            WebHost.CreateDefaultBuilder (args)
            .UseUrls ($"http://0.0.0.0:{port}")
            .UseStartup<Startup> ();
    }
}