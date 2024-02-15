using FluentValidation;

namespace Application.DotNetCodeAnalyzer.Commands
{
    public class DotNetAnalyzeCodeCommandValidator : AbstractValidator<DotNetAnalyzeCodeCommand>
    {
        public DotNetAnalyzeCodeCommandValidator()
        {
            RuleFor(command => command.DotNetVersionInfo).NotNull();
            RuleFor(command => command.Codes).NotNull().NotEmpty();
            RuleForEach(codes => codes.Codes)
                .Must(code =>
                {
                    return code.Trim().Length > 0;
                })
                .WithMessage("Empty string in code!");
        }
    }
}
