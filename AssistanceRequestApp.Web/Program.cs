namespace AssistanceRequestApp.Web
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Defines the <see cref="Program" />.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The Main.
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/>.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// The CreateHostBuilder.
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/>.</param>
        /// <returns>The <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logBuilder =>
            {
                logBuilder.ClearProviders(); // removes all providers from LoggerFactory
                logBuilder.AddConsole();
                logBuilder.AddTraceSource("Information, ActivityTracing"); // Add Trace listener provider
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
