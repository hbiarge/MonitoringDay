using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ProductImagesApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                // Health check middleware: listen at /health endpoint
                .UseBeatPulse(options =>
                {
                    options.SetAlternatePath("health");
                })
                .Build();
    }
}
