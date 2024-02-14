using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Filters.Behaviours
{
    public class UnhandledExceptionBehaviour<TRequest, TResponce> : IPipelineBehavior<TRequest, TResponce> where TRequest : IRequest<TResponce>
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponce> Handle(TRequest request, RequestHandlerDelegate<TResponce> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogError(ex, "Unhandled Exception for Request {Name} {@Request}", requestName, request);
                throw;
            }
        }
    }
}
