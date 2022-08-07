using RewardsSystem.Service.Commands;
using RewardsSystem.Service.Models;

namespace RewardsSystem.Tests.Services.Commands;

public class CreateCustomerCommandValidatorTests
{
    [Fact]
    public void CreateCustomerCommandValidator_Validate_CheckWithNullObject()
    {
        var scope = new TestScope(null);

        var result = scope.Instance.Validate(scope.Request);

        result.IsValid.Should().BeFalse();
        Assert.Single(result.Errors);
        Assert.Equal(CreateCustomerCommandValidator.ValidationMessages.PreValidateErrorMessage, result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void CreateCustomerCommandValidator_Validate_CheckWithGoodFilledFields()
    {
        var customer = new CreateCustomerView()
        {
            Name = "Denis"
        };
        var scope = new TestScope(customer);

        var result = scope.Instance.Validate(scope.Request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void CreateCustomerCommandValidator_Validate_CheckWithBadFilledFields()
    {
        var customer = new CreateCustomerView()
        {
            Name = ""
        };
        var scope = new TestScope(customer);

        var result = scope.Instance.Validate(scope.Request);

        result.IsValid.Should().BeFalse();
        Assert.Equal(2, result.Errors.Count);
        Assert.Equal(CreateCustomerCommandValidator.ValidationMessages.EmptyName, result.Errors[0].ErrorMessage);
        Assert.Equal(CreateCustomerCommandValidator.ValidationMessages.InvalidNameLength, result.Errors[1].ErrorMessage);
    }

    public class TestScope : ValidatorTestScopeBase<CreateCustomerCommand, CreateCustomerCommandValidator>
    {
        public TestScope(CreateCustomerView customer)
            : base(() => new CreateCustomerCommand(customer),
                  new CreateCustomerCommandValidator())
        {
        }
    }
}
