using System.Buffers;
using Cysharp.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace GameApi
{
    public class Program
    {
        private static readonly Utf8PreparedFormat<string, LogLevel, string> LogPrefix
            = ZString.PrepareUtf8<string, LogLevel, string>("{0} {1} --- {2}    : ");

        private const int LogRollSizeKb = 1024 * 1024 * 10;

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Debug);

                    logging.AddZLoggerFile("logs/app.log", options => { options.PrefixFormatter = LogFormatter; });

                    logging.AddZLoggerRollingFile((dt, x) => $"logs/app.{dt.ToLocalTime():yyyyMMdd}.{x:000}.log",
                        x => x.ToLocalTime().Date, LogRollSizeKb,
                        options => { options.PrefixFormatter = LogFormatter; });

                    logging.AddZLoggerConsole(options => { options.PrefixFormatter = LogFormatter; });
                });

        private static void LogFormatter(IBufferWriter<byte> writer, LogInfo info) =>
            LogPrefix.FormatTo(ref writer,
                info.Timestamp.DateTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.fff"), info.LogLevel,
                info.CategoryName);
    }
}