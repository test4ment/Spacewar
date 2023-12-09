namespace Spacewar.Tests;

[FeatureFile(@"../../../Features/macrocommand.feature")]
public class MacroCommandsFeature : Feature
{
    public Mock<UObject> test_obj = new Mock<UObject>(new ObjDictionary());

    [Given("Инициализирован IoC")]
    public static void IoCInit()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
    }

    [And("Сгенерированы разные команды")]
    public static void MakeCommands()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
            "Game.StartCommand",
            (object[] args) => new StartCommand((Order)args[0])
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
            "Commands.Move",
                (object[] args) => new MoveCommand(
                    new MoveableAdapter(
                        (UObject)args[0]
                    )
                )
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
            "Commands.SomeCommand",
                (object[] args) => new SomeCommand()
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
            "Commands.AnotherCommand",
                (object[] args) => new AnotherCommand()
        ).Execute();

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
    }
    [When("Команды кладутся и исполняются в очереди")]
    public void Test()
    {
        var order1 = new Mock<Order>();
        order1.Setup(o => o.target).Returns(test_obj.Object);
        order1.Setup(o => o.cmd).Returns("Commands.Move");
        order1.Setup(o => o.args).Returns(new ObjDictionary());

        var order2 = new Mock<Order>();
        order2.Setup(o => o.target).Returns(test_obj.Object);
        order2.Setup(o => o.cmd).Returns("Commands.SomeCommand");
        order2.Setup(o => o.args).Returns(new ObjDictionary());

        var order3 = new Mock<Order>();
        order3.Setup(o => o.target).Returns(test_obj.Object);
        order3.Setup(o => o.cmd).Returns("Commands.AnotherCommand");
        order3.Setup(o => o.args).Returns(new ObjDictionary());

        IoC.Resolve<IQueue<ICommand>>(
            "Game.Queue"
        ).Put(
            IoC.Resolve<ICommand>("Game.StartCommand", order1.Object)
        );

        IoC.Resolve<IQueue<ICommand>>(
            "Game.Queue"
        ).Put(
            IoC.Resolve<ICommand>("Game.StartCommand", order2.Object)
        );

        IoC.Resolve<IQueue<ICommand>>(
            "Game.Queue"
        ).Put(
            IoC.Resolve<ICommand>("Game.StartCommand", order3.Object)
        );

        IoC.Resolve<IQueue<ICommand>>(
            "Game.Queue"
        ).Take().Execute(); // makes move

        IoC.Resolve<IQueue<ICommand>>(
            "Game.Queue"
        ).Take().Execute(); // makes some

        IoC.Resolve<IQueue<ICommand>>(
            "Game.Queue"
        ).Take().Execute(); // makes another
    }

    [Then("В объекте оказываются все сгенерированные команды")]
    public void qq()
    {
        Assert.IsType<MoveCommand>(test_obj.Object.properties.Get("Commands.Move"));
        Assert.IsType<SomeCommand>(test_obj.Object.properties.Get("Commands.SomeCommand"));
        Assert.IsType<AnotherCommand>(test_obj.Object.properties.Get("Commands.AnotherCommand"));
    }
}

internal class SomeCommand : ICommand
{
    public void Execute()
    {

    }
}

internal class AnotherCommand : ICommand
{
    public void Execute()
    {

    }
}
