using Kemar.UrgeTruck.Api.Core.Extension;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Kemar.UrgeTruck.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
               //.ConfigureServices((hostContext, services) =>
               // {
               //     UTRegisterQuartzExtension.RegisterUTQuartzServices(hostContext, services);
               // });
    }
}
