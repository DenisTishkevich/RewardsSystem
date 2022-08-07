using MediatR;
using Moq;

namespace RewardsSystem.Tests.API;

public class TestScopeBase<TController>
{
    public TController Instance { get; set; }
    public Mock<IMediator> Mediator { get; set; }

    public TestScopeBase(Func<IMediator, TController> factory)
    {
        Mediator = new Mock<IMediator>();
        Instance = factory(Mediator.Object);
    }
}