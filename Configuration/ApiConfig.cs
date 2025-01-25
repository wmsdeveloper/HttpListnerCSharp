using Serilog;

namespace HttpListnerCSharp.Configuration {
    public static class ApiConfig {
        public static void AddApiConfiguration(this ConfigurationManager configuration, IHostEnvironment env) {
            // Configure settings
            configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            // Configure Serilog to write logs to a text file
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File($"logs/OSWebServerAgentApp.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}