using FluentValidation.Results;

namespace Core.Exceptions
{
    public record ErrorResponse
    {
        public ErrorResponse(IEnumerable<ValidationFailure> failures)
        {
            Errors = failures.Select(s => new ValidationError() { ErrorMessage = s.ErrorMessage, PropertyName = s.PropertyName }).ToList();
        }

        public ErrorResponse(IEnumerable<(string error, string code)> failures)
        {
            Errors = failures.Select(s => new ValidationError() { ErrorMessage = s.error, PropertyName = s.code }).ToList();
        }

        public List<ValidationError> Errors { get; }
    }
}
