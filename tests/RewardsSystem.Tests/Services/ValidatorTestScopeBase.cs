using FluentValidation;
using FluentValidation.Results;

namespace RewardsSystem.Tests.Services;

public class ValidatorTestScopeBase<TRequest, TValidator>
    where TValidator : AbstractValidator<TRequest>
{
    public TRequest Request { get; set; }
    public TValidator Instance { get; }

    public ValidatorTestScopeBase(Func<TRequest> requestFactory, TValidator instance)
    {
        Request = requestFactory();
        Instance = instance;
    }

    public ValidationResult Validate()
    {
        return Instance.Validate(Request);
    }
}
