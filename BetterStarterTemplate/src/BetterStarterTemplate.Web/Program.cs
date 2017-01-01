using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;

namespace BetterStarterTemplate.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Builder.Startup>()
                //Added to support moving StartUp.cs to a class library (https://github.com/aspnet/Hosting/issues/903)
                // Ignore the startup class assembly as the "entry point" and instead point it to this app
                .UseSetting(WebHostDefaults.ApplicationKey, typeof(Program).GetTypeInfo().Assembly.FullName) 
                .Build();

            host.Run();
        }
    }
}
