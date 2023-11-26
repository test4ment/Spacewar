namespace Spacewar.Tests;

[FeatureFile(@"../../../Features/continiousoperations.feature")]
public class ContiniousOpsTests : Feature
{
    private readonly Mock<Order> newOrder = new Mock<Order>();

    [Given("Инициализирован IoC")]
    public static void IoCInit()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
    }
    [And("Объект и приказ для него")]
    public void StartCommandTest()
    {
        // Делаем объект, который двигается почти как настоящий
        var moving_object = new Mock<IMoveable>();
        moving_object.Setup(obj => obj.position).Returns(new Vector(5, -6));
        moving_object.Setup(obj => obj.instant_velocity).Returns(new Vector(1, 1));

        // Делаем очередь, которая ведет себя почти как настоящая
        var queue = new Queue<ICommand>();
        var queueMock = new Mock<IQueue<ICommand>>();

        queueMock.Setup(q => q.Take()).Returns(() => queue.Dequeue());
        queueMock.Setup(q => q.Put(It.IsAny<ICommand>())).Callback((ICommand obj) => queue.Enqueue(obj));

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
            "Game.Queue",
            (object[] args) =>
            {
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

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
            "Game.Operation.Repeat",
            (object[] args) =>
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

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
            "Game.StartCommand",
            (object[] args) => new StartCommand((Order)args[0])
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
            newOrder.Object.orderName,
            (object[] args) => newOrder.Object
        ).Execute();
    }

    [When("Executed")]
    public void Execution()
    {
        IoC.Resolve<ICommand>("Game.StartCommand", newOrder.Object).Execute();
    }

    [Then("В очередь приходят ICommand")]
    public static void QueueIsFilling()
    {
        var a = 0;
        while (a != 10)
        {
            IoC.Resolve<IQueue<ICommand>>("Game.Queue").Take().Execute();
            a++;
        }

        Assert.True(true); // Код дойдет сюда только если операция умеет сама себя повторять
        // Спросить как надо сделать
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
