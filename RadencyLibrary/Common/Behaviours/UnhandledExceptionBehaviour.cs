using MediatR;
using Microsoft.EntityFrameworkCore;

namespace RadencyLibrary.Common.Behaviours
{
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var requestName = typeof(TRequest).Name;

                _logger.LogError(ex, "Radency library request: Update database Exeption for Request {Name} {@Request}", requestName, request);

                throw;
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;

                _logger.LogError(ex, "Radency library request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

                throw;
            }
        }
    }

}
