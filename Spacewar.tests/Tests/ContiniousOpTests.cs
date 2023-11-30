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
    [And(@"Объект в точке \(-?(\d+), (-?\d+)\) и приказ для начала движения со скоростью \(-?(\d+), (-?\d+)\)")]
    public void StartCommandTest(int posx, int posy, int velx, int vely)
    {
        var spaceship = new Mock<UObject>(new ObjDictionary());

        spaceship.Object.properties.Set("Position", new Vector(posx, posy));
        spaceship.Object.properties.Set("Velocity", new Vector(0, 0));

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
                (object[] args) => spaceship.Object
            ).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
            "Commands.Move",
                (object[] args) => new MoveCommand(
                    new MoveableAdapter(
                        (UObject)args[0]
                    )
                )
            ).Execute();

        newOrder.Setup(o => o.target).Returns(IoC.Resolve<UObject>("Game.Objects.Object1"));
        newOrder.Setup(o => o.cmd).Returns("Commands.Move");
        IDict<string, object> newOrderArgs = new ObjDictionary();
        newOrderArgs.Set("Velocity", new Vector(velx, vely));
        newOrder.Setup(o => o.args).Returns(newOrderArgs);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
            "Game.StartCommand",
            (object[] args) => new StartCommand((Order)args[0])
        ).Execute();

        IoC.Resolve<IQueue<ICommand>>("Game.Queue").Put(
            new StartCommand(newOrder.Object)
        );
    }

    [When(@"Проходит (\d+) итераций")]
    public static void NGameTicks(int n)
    {
        for (var i = 0; i <= n; i++)
        {
            IoC.Resolve<IQueue<ICommand>>("Game.Queue").Take().Execute();
        }
    }

    [Then(@"Объект окажется в точке \(-?(\d+), (-?\d+)\)")]
    public static void QueueIsFilling(int x, int y)
    {
        Assert.Equal(
            IoC.Resolve<UObject>("Game.Objects.Object1").properties.Get("Position"),
            new Vector(x, y)
        );
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

internal class ObjDictionary : IDict<string, object>
{
    public IDictionary<string, object> dict { get; } = new Dictionary<string, object>();

    public object Get(string key)
    {
        return dict[key];
    }

    public void Set(string key, object value)
    {
        dict[key] = value;
    }
}
