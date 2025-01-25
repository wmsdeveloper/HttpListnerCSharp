using HttpListnerCSharp.Config;
using HttpListnerCSharp.Configuration;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddApiConfiguration(builder.Environment);
builder.Services.AddServiceConfiguration();
builder.Services.Configure<AgentSettings>(options => builder.Configuration.GetSection($"{nameof(AgentSettings)}").Bind(options));

var host = builder.Build();
host.Run();