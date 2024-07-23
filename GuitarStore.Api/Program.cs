using GraphQL;

namespace GuitarStore.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateHostBuilder(args);
            builder.Build().Run();
        }
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
