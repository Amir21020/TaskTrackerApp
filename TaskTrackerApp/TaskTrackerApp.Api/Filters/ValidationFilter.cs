using FluentValidation;


namespace TaskTrackerApp.Api.Filters;

public sealed class ValidationFilter<TValue> : IEndpointFilter where TValue : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var argument = context.Arguments.FirstOrDefault(x => x is TValue) as TValue;

        if (argument is not null)
        {
            var validator = context.HttpContext.RequestServices.GetService<IValidator<TValue>>();

            if (validator is not null)
            {
                var validationResult = await validator.ValidateAsync(argument, context.HttpContext.RequestAborted);

                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
            }
        }

        return await next(context);
    }
}