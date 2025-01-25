using HttpListnerCSharp.Services;
using HttpListnerCSharp.Services.Interfaces;
using Serilog;

namespace HttpListnerCSharp.Config {
    public static class ServicesConfig {

        public static void AddServiceConfiguration(this IServiceCollection services) {
            services.AddSerilog();
            services.AddHostedService<AgentWorker>();
            services.AddSingleton<IAgentListener, AgentListner>();
            services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(Program).Assembly));
        }
    }
}