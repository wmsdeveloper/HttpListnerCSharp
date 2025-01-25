using MediatR;
using Microsoft.Extensions.Options;
using HttpListnerCSharp.Commands;
using HttpListnerCSharp.Configuration;
using HttpListnerCSharp.Services.Interfaces;
using System.Net;
using System.Text;

namespace HttpListnerCSharp.Services {
    public class AgentListner : IAgentListener {
        #region [ Attributes ]

        private readonly ILogger<AgentListner> _logger;
        private readonly AgentSettings _settings;
        private readonly IMediator _mediator;
        private HttpListener _listener = new();

        #endregion

        #region [ Constructor ]

        public AgentListner(ILogger<AgentListner> logger, IOptions<AgentSettings> settings, IMediator mediator) {
            _logger = logger;
            _settings = settings.Value;
            _mediator = mediator;
        }

        #endregion

        #region [ Public Methods ]

        public void Start() {
            _listener.Prefixes.Add($"http://+:{_settings.Port}/");
            _listener.Start();
            _logger.LogInformation($"Escutando na porta {_settings.Port}...");

            Receive();
        }

        public void Stop() {
            _listener.Stop();
        }

        #endregion

        #region [ Private Methods ]

        private void Receive() {
            _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
        }

        private void ListenerCallback(IAsyncResult result) {
            if (_listener.IsListening) {
                var context = _listener.EndGetContext(result);
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                if (request.HttpMethod == "OPTIONS") {
                    response.AddHeader("Access-Control-Allow-Headers", "*");
                }
                response.AppendHeader("Access-Control-Allow-Origin", "*");
                _logger.LogInformation($"{request.Url}");

                ProcessRequest(context);
                Receive();
            }
        }

        public void ProcessRequest(HttpListenerContext ctx) {
            string commandResult = string.Empty;
            try {
                HttpListenerRequest request = ctx.Request;
                string? cmd = request.Headers.Get("x-cmd") ?? string.Empty;
                _logger.LogInformation($"Received command: {cmd}");
                commandResult = _mediator.Send(new AgentShellExecCommand() { Command = cmd }).Result;
            }
            catch (Exception ex) {
                commandResult = ex.Message;
            }

            ProcessResponse(ctx, commandResult);
        }

        private void ProcessResponse(HttpListenerContext ctx, string responseMessage) {
            using HttpListenerResponse response = ctx.Response;
            response.Headers.Set("Content-Type", "text/plain");

            string data = responseMessage ?? "unknown";
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            response.ContentLength64 = buffer.Length;

            using Stream ros = response.OutputStream;
            ros.Write(buffer, 0, buffer.Length);
        }

        #endregion

    }
}