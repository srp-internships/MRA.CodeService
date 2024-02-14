using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Core.Filters.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponce> : IPipelineBehavior<TRequest, TResponce>
        where TRequest : IRequest<TResponce>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;
        private readonly IConfiguration _configuration;

        public PerformanceBehaviour(ILogger<TRequest> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _timer = new Stopwatch();
        }

        public async Task<TResponce> Handle(TRequest request, RequestHandlerDelegate<TResponce> next,
            CancellationToken cancellationToken)
        {
            _timer.Start();

            try
            {
                var response = await next();
                return response;
            }
            finally
            {
                _timer.Stop();

                var elapsedMilliseconds = _timer.ElapsedMilliseconds;
                int threshold = int.Parse(_configuration["LongRunningRequestThreshold"]);

                if (elapsedMilliseconds > threshold)
                {
                    var requestName = typeof(TRequest).Name;
                    _logger.LogError($"Long Running Request: {requestName}, millisecond: {elapsedMilliseconds}");
                }
            }
        }
    }
}