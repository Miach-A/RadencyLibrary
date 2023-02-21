using FluentValidation;
using FluentValidation.Results;
using MediatR;
using RadencyLibrary.CQRS.Base;

namespace RadencyLibrary.Common.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
        where TResponse : IValidatedResponse<ValidationFailure>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    _validators.Select(v =>
                        v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                    .Where(r => r.Errors.Any())
                    .SelectMany(r => r.Errors)
                    .ToList();

                if (failures.Any())
                {
                    //throw new ValidationException(failures);
                    var responce = (TResponse)Activator.CreateInstance(typeof(TResponse))!;
                    responce.Validated = false;
                    responce.Errors = failures;
                    return responce;
                }


            }
            return await next();
        }
    }

}
