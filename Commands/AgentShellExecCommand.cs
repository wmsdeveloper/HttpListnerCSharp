using FluentValidation;
using MediatR;
using HttpListnerCSharp.Commands.Validation;

namespace HttpListnerCSharp.Commands {
    public class AgentShellExecCommand : IRequest<string> {
        #region [ Private Consts ]

        // ASCII
        private const string AscIISpace = "%20";
        private const string AscIIQuotationMark = "%22";
        private const string AscIIApostrophe = "%27";

        // Default
        private const string Space = " ";
        private const string QuotationMark = "\"";
        private const string Apostrophe = "'";

        #endregion

        public string Command { get; set; } = string.Empty;

        public void Prepare() {
            string _command = Command;
            _command = _command.Contains(AscIISpace) ? _command.Replace(AscIISpace, Space) : _command;
            _command = _command.Contains(AscIIQuotationMark) ? _command.Replace(AscIIQuotationMark, QuotationMark) : _command;
            _command = _command.Contains(AscIIApostrophe) ? _command.Replace(AscIIApostrophe, Apostrophe) : _command;
            Command = _command;
        }

        public void Validate() {
            var ValidationResult = new AgentShellExecCommandValidation().Validate(this);
            if (!ValidationResult.IsValid)
                throw new ValidationException(ValidationResult.Errors);
        }
    }
}