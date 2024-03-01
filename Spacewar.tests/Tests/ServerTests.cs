namespace Spacewar.Tests;
using System.Collections.Concurrent;

[FeatureFile(@"../../../Features/server.feature")]
public class ServerFeatures : Feature
{
    [Given("Инициализирован IoC")]
    public static void IoCInit()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>(
                "Scopes.New",
                IoC.Resolve<object>("Scopes.Root")
            )
        ).Execute();
    }

    [And("Создана очередь")]
    public static void QueueInit()
    {
        var q = new BlockingCollection<ICommand>();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Queue",
            (object[] args) => { return q; }
        ).Execute();
    }

    [And("Создан сервер")]
    public static void ServerInit()
    {
        var server = new ServerThread(
            IoC.Resolve<BlockingCollection<ICommand>>("Queue")
        );

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server",
            (object[] args) => { return server; }
        ).Execute();
    }

    [And("Добавлена пустая команда")]
    public static void NopCmdAdd()
    {
        var nop = new Mock<ICommand>();
        nop.Setup(m => m.Execute()).Verifiable();

        IoC.Resolve<BlockingCollection<ICommand>>("Queue").Add(nop.Object);
    }

    [And("Добавлена команда hard-остановки")]
    public static void HardStopCmdAdd()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Commands.HardStop", (object[] args) => {
            new ActionCommand(() => {
                new HardStopServer((ServerThread)args[0])();
                try{
                    new ActionCommand((Action)args[1]);
                }
                catch{}
            });
        }).Execute();
        
        var stopcmd = IoC.Resolve<ICommand>("Commands.HardStop", IoC.Resolve<ServerThread>("Server")); //

        IoC.Resolve<BlockingCollection<ICommand>>("Queue").Add(stopcmd);
    }

    [And("Добавлена команда soft-остановки")]
    public static void SoftStopCmdAdd()
    {
        IoC.Resolve<ICommand>("IoC.Register", "Commands.SoftStop", (object[] args) =>
        {
            try{
                return new SoftStopServer((ServerThread)args[0], (Action)args[1]);
            }
            catch{
                return new SoftStopServer((ServerThread)args[0], () => {});
            }
        }).Execute();

        var stopcmd = IoC.Resolve<ICommand>("Commands.SoftStop", IoC.Resolve<ServerThread>("Server"), () => {});
        IoC.Resolve<BlockingCollection<ICommand>>("Queue").Add(stopcmd);
    }

    [And("Добавлена долгая операция")]
    public static void AddThinking()
    {
        var longcmd = new LongComputingCommand();
        IoC.Resolve<BlockingCollection<ICommand>>("Queue").Add(longcmd);
    }

    [And(@"Ждать (\d+) секунд")]
    public static void WaitFor(int seconds)
    {
        Thread.Sleep(seconds * 1000);
    }

    [When("Запущен сервер")]
    public static void StartServer()
    {
        IoC.Resolve<ServerThread>("Server").Start();
    }

    [Then(@"В очереди остается (\d+) команд")]
    public static void Remains(int cmds)
    {
        Thread.Sleep(150);
        Assert.Equal(cmds, IoC.Resolve<BlockingCollection<ICommand>>("Queue").Count);
    }

    internal class LongComputingCommand : ICommand
    {
        public LongComputingCommand() { }
        public void Execute()
        {
            Thread.Sleep(100);
        }
    }
}
