using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace RewardsSystem.Tests.API;

public abstract class ControllerTests<TController, TScope>
    where TScope : TestScopeBase<TController>
{
    public static CancellationToken AnyCancellationToken => It.IsAny<CancellationToken>();

    protected abstract TScope CreateTestScope();
        
    public async Task VerifyMediatr<TQuery, TResponse>(
        Func<TResponse> valueFunction, 
        Func<TController, TScope, Task<ActionResult<TResponse>>> action)
        where TQuery : IRequest<TResponse>
    {
        var scope = CreateTestScope();
        scope.Mediator
            .Setup(x => x.Send(It.IsAny<TQuery>(), AnyCancellationToken))
            .ReturnsAsync(valueFunction());
        var result = await action(scope.Instance, scope);
            
        scope.Mediator.Verify(x => x.Send(It.IsAny<TQuery>(), AnyCancellationToken), Times.Once);
    }

    public async Task CheckForCorrespondenceOfTheResponse<TQuery, TResponse>(
        Func<TResponse> valueFunction, 
        Func<TController, TScope, Task<ActionResult<TResponse>>> action)
        where TQuery : IRequest<TResponse>
    {
        var scope = CreateTestScope();
        scope.Mediator
            .Setup(x => x.Send(It.IsAny<TQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(valueFunction());

        var actionResult = await action(scope.Instance, scope);
        var objectResult = actionResult.Result as ObjectResult;

        Assert.NotNull(objectResult);
        Assert.Equal(200, objectResult.StatusCode);

        var okObjectResult = actionResult.Result as OkObjectResult;

        Assert.NotNull(okObjectResult);

        var response = (TResponse)okObjectResult.Value;

        Assert.NotNull(response);
        response.Should().BeEquivalentTo(valueFunction());
    }
}