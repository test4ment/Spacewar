namespace Spacewar.Tests;

public class ContiniousOpsTests : Feature{
    private readonly Mock<Order> newOrder = new Mock<Order>();

    [Fact]
    public static void IoCInit(){
        new InitScopeBasedIoCImplementationCommand().Execute();
    }
    [Fact]
    public void StartCommandTest()
    {
        var moving_object = new Mock<IMoveable>();

        var queue = new Queue<ICommand>();
        var queueMock = new Mock<IQueue<ICommand>>();

        queueMock.Setup(q => q.Take()).Returns(() => queue.Dequeue());
        queueMock.Setup(q => q.Put(It.IsAny<ICommand>())).Callback((ICommand obj) => queue.Enqueue(obj));
        
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
            "Game.Queue", 
            (object[] args)=> {
                return queueMock.Object;
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
            "Game.Objects.Object1",
            (object[] args) => moving_object.Object
            ).Execute();
        
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
            "Commands.Move",
            (object[] args) => new MoveCommand((IMoveable)args[0])
            ).Execute();
        
        // IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
        //     "Game.Move.Object1",
        //     (object[] args) => IoC.Resolve<ICommand>("Commands.Move", "Game.Objects.Object1")
        //     ).Execute();
        
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
            "Game.Operation.Repeat",
            (object[] args) =>  // orderName
                new ActionCommand(() =>
                    IoC.Resolve<IQueue<ICommand>>("Game.Queue").Put(
                        IoC.Resolve<ICommand>("Game.StartCommand",
                            IoC.Resolve<Order>((string)args[0])
                    )
                )
            )
        ).Execute();

        newOrder.Setup(o => o.orderName).Returns("Game.StartMove.Object1");
        newOrder.Setup(o => o.cmd).Returns(
            IoC.Resolve<ICommand>("Commands.Move",
                IoC.Resolve<IMoveable>("Game.Objects.Object1")
            )
        );

        newOrder.Setup(o => o.args).Returns(new object[1]);

        // IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
        //     "Game.Orders.Order1",
        //     (object[] args) => newOrder.Object
        // ).Execute();

        // var startContinousCommand = new Mock<StartCommand>(newOrder.Object);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
            "Game.StartCommand",
            (object[] args) => new StartCommand((Order) args[0])
        ).Execute();

        IoC.Resolve<ICommand>("Game.StartCommand", newOrder.Object).Execute();

        var a = 0;
        foreach(var i in queue)
        {
            a++;
        }

        Assert.Equal(2, a);
    }
}

internal class ActionCommand : ICommand
{
    private readonly Action _action;
    public ActionCommand(Action action) => _action = action;
    public void Execute()
    {
        _action();
    }
}