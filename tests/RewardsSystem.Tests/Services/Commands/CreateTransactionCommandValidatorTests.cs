using RewardsSystem.Service.Commands;
using RewardsSystem.Service.Models;

namespace RewardsSystem.Tests.Services.Commands;

public class CreateTransactionCommandValidatorTests
{
    [Fact]
    public void CreateTransactionCommandValidator_Validate_CheckWithNullObject()
    {
        var scope = new TestScope(null);

        var result = scope.Instance.Validate(scope.Request);

        result.IsValid.Should().BeFalse();
        Assert.Single(result.Errors);
        Assert.Equal(CreateTransactionCommandValidator.ValidationMessages.PreValidateErrorMessage, result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateTransactionCommandValidator_Validate_CheckWithGoodFilledFields()
    {
        var transaction = new CreateTransactionView()
        {
            Price = 100,
            CustomerId = 1
        };
        var scope = new TestScope(transaction);

        var result = scope.Instance.Validate(scope.Request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void CreateTransactionCommandValidator_Validate_CheckWithBadFilledFields()
    {
        var transaction = new CreateTransactionView()
        {
            Price = 0,
            CustomerId = 0
        };
        var scope = new TestScope(transaction);

        var result = scope.Instance.Validate(scope.Request);

        result.IsValid.Should().BeFalse();
        Assert.Equal(2, result.Errors.Count);
        Assert.Equal(CreateTransactionCommandValidator.ValidationMessages.OnlyPositiveNumbersForCustomerId, result.Errors[0].ErrorMessage);
        Assert.Equal(CreateTransactionCommandValidator.ValidationMessages.OnlyPositiveNumbersForPrice, result.Errors[1].ErrorMessage);
    }

    [Fact]
    public void CreateTransactionCommandValidator_Validate_CheckForMoreThanTwoDecimalPlaces()
    {
        var transaction = new CreateTransactionView()
        {
            Price = 10.101m,
            CustomerId = 1
        };
        var scope = new TestScope(transaction);

        var result = scope.Instance.Validate(scope.Request);

        result.IsValid.Should().BeFalse();
        Assert.Single(result.Errors);
        Assert.Equal("'Transaction Price' must not be more than 8 digits in total, with allowance for 2 decimals. 2 digits and 3 decimals were found.", 
            result.Errors[0].ErrorMessage);
    }

    public class TestScope : ValidatorTestScopeBase<CreateTransactionCommand, CreateTransactionCommandValidator>
    {
        public TestScope(CreateTransactionView transaction)
            : base(() => new CreateTransactionCommand(transaction),
                  new CreateTransactionCommandValidator())
        {
        }
    }
}
