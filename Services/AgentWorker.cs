using HttpListnerCSharp.Services.Interfaces;

namespace HttpListnerCSharp.Services {
    public class AgentWorker : IHostedService {
        #region [ Attributes ]

        private readonly ILogger<AgentWorker> _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IAgentListener _agentListener;

        #endregion

        #region [ Constructor ]

        public AgentWorker(ILogger<AgentWorker> logger, IHostApplicationLifetime appLifetime, IAgentListener agentListener) {
            _logger = logger;
            _appLifetime = appLifetime;
            _agentListener = agentListener;
        }

        #endregion

        #region [ Public Methods ]

        public Task StartAsync(CancellationToken cancellationToken) {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            return Task.CompletedTask;
        }

        #endregion

        #region [ Private Methods ]

        /// <summary>
        /// Perform post-startup activities here
        /// </summary>
        private void OnStarted() {
            _logger.LogInformation("AgentWorker foi iniciado");
            _agentListener.Start();
        }

        /// <summary>
        /// Perform on-stopping activities here
        /// </summary>
        private void OnStopping() {
            _logger.LogInformation("Tentando parar o AgentWorker");
        }

        /// <summary>
        /// Perform post-stopped activities here
        /// </summary>
        private void OnStopped() {
            _logger.LogInformation("AgentWorker foi parado");
        }
    }

    #endregion

}