using FluentValidation;
using FluentValidation.Results;

namespace RewardsSystem.Service.Commands;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Customer.Name)
            .NotEmpty()
            .WithMessage(ValidationMessages.EmptyName);
        RuleFor(x => x.Customer.Name)
            .Length(1, 80)
            .WithMessage(ValidationMessages.InvalidNameLength);
    }

    protected override bool PreValidate(ValidationContext<CreateCustomerCommand> context, ValidationResult result)
    {
        if (context.InstanceToValidate.Customer == null)
        {
            result.Errors.Add(new ValidationFailure("CreateCustomerCommandValidator", ValidationMessages.PreValidateErrorMessage));
            return false;
        }
        return true;
    }

    public static class ValidationMessages
    {
        public const string PreValidateErrorMessage = "Please ensure a model was supplied.";
        public const string EmptyName = "Name must not be empty.";
        public const string InvalidNameLength = "The length of Name must be 80 characters or fewer.";
    }
}
