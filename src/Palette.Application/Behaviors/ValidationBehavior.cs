

using FluentValidation;
using MediatR;

namespace Palette.Application.Behaviors;


// pipeline behavior that runs BEFORE every MediatR handler
// intercepts all comamnds/queries and validates them automatically
public class ValidationBehaviors<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    // inject all validators for this request type
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    public ValidationBehaviors(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    // runs before the actual handler
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // if no validators exist for this reqm skip validation
        if (!_validators.Any()) return await next();
        // create validation context with the request
        var context = new ValidationContext<TRequest>(request);

        // run all vlaidators and collect failures
        var failures = _validators
            .Select(v => v.Validate(context))       // validate request
            .SelectMany(result => result.Errors)    // get all errors
            .Where(f => f != null)                  // filter nulls
            .ToList();

        // if validation failed, throw exception(caught by controller)
        if (failures.Count != 0) throw new ValidationException(failures);

        // validation passed, continue to handler
        return await next();
    }
}