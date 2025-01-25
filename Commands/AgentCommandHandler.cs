using MediatR;
using System.Diagnostics;

namespace HttpListnerCSharp.Commands {
    public class AgentCommandHandler : IRequestHandler<AgentShellExecCommand, string> {

        async Task<string> IRequestHandler<AgentShellExecCommand, string>.Handle(AgentShellExecCommand command, CancellationToken cancellationToken) {
            command.Validate();
            command.Prepare();

            var process = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = "powershell.exe",
                    Arguments = $"-Command \"{command.Command}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = false
                }
            };

            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0) {
                string error = process.StandardError.ReadToEnd();
                return $"Erro na execução do comando <{command.Command}>: {error}";
            }

            return result;
        }

    }
}