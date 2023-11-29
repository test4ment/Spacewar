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
        var spaceship = new Mock<UObject>(new ObjDictionary());
        // moving_object.Setup(obj => obj.position).Returns(new Vector(0, 0));
        // moving_object.Setup(obj => obj.instant_velocity).Returns(new Vector(0, 0));

        spaceship.Object.properties.Set("Position", new Vector(0, 0));
        spaceship.Object.properties.Set("Velocity", new Vector(0, 0));
        
        IMoveable moving_object = new MoveableAdapter(spaceship.Object);

        // IMoveable moving_object = new MoveableAdapter(spaceship.Object);

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

        // IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
        //     "Game.Operation.Repeat",
        //     (object[] args) =>
                
        //     )
        // ).Execute();

        newOrder.Setup(o => o.objectName).Returns("Game.Objects.Object1");
        newOrder.Setup(o => o.cmd).Returns("Commands.Move");
        IDict<string, object> newOrderArgs = new ObjDictionary();
        newOrderArgs.Set("Velocity", new Vector(1, 1));
        newOrder.Setup(o => o.args).Returns(newOrderArgs);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
            "Game.StartCommand",
            (object[] args) => new StartCommand((Order)args[0])
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

internal class ObjDictionary : IDict<string, object>{
    private readonly Dictionary<string, object> dict = new Dictionary<string, object>();

    public object Get(string key){
        return dict[key];
    }

    public void Set(string key, object value){
        dict[key] = value;
    }
}