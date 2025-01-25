using FluentValidation;

namespace HttpListnerCSharp.Commands.Validation {
    public class AgentShellExecCommandValidation : AbstractValidator<AgentShellExecCommand> {
        public AgentShellExecCommandValidation() {
            RuleFor(c => c.Command).NotEmpty().WithMessage("O comando não foi informado");
        }
    }
}