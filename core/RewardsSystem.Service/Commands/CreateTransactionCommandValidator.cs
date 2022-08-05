using FluentValidation;
using FluentValidation.Results;

namespace RewardsSystem.Service.Commands;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.Transaction.Price)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.OnlyPositiveNumbers);

        RuleFor(x => x.Transaction.Price)
            .ScalePrecision(2, 8);
    }

    protected override bool PreValidate(ValidationContext<CreateTransactionCommand> context, ValidationResult result)
    {
        if (context.InstanceToValidate.Transaction == null)
        {
            result.Errors.Add(new ValidationFailure("CreateTransactionCommandValidator", ValidationMessages.PreValidateErrorMessage));
            return false;
        }
        return true;
    }

    public static class ValidationMessages
    {
        public const string PreValidateErrorMessage = "Please ensure a model was supplied.";
        public const string OnlyPositiveNumbers = "The price must be only a positive number.";
    }
}
