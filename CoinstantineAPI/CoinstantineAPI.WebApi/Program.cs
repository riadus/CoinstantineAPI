using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace CoinstantineAPI.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
               //.UseUrls("http://*:5100;http://localhost:5000")
                .CaptureStartupErrors(true)
            .UseSetting("detailedErrors", "true")
                .UseStartup<Startup>()
                .Build();
    }
}
