using FluentValidation.Results;

namespace Core.Exceptions
{
    public class ValidationFailureException : Exception
    {
        public ValidationFailureException(IEnumerable<ValidationFailure> failures)
        : base("One or more validation failures have occurred.")
        {
            ErrorResponse = new ErrorResponse(failures);
        }

        public ValidationFailureException(ErrorResponse errorResponse)
        {
            ErrorResponse = errorResponse;
        }

        public ErrorResponse ErrorResponse { get; }
    }
}
