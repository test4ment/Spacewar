namespace Spacewar.Tests;
using System.Collections.Concurrent;

[FeatureFile(@"../../../Features/server.feature")]
public class ServerFeatures : Feature
{
    private static ManualResetEvent mre = new ManualResetEvent(false);
    private static ManualResetEvent mreTests = new ManualResetEvent(false);
    private Action act = () => {}; 
    private Exception? e;

    [Given("Инициализирован IoC и обработчик исключений")]
    public static void IoCInit()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.Root")
        ).Execute();

        try{
            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Exception.Handler",
                (object[] args) => {return new Mock<ICommand>().Object;}
            ).Execute();
        } catch {}

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

    [And("Добавлена команда ожидания MRE")]
    public static void MreAwaitCmd(){
        var wait = new Mock<ICommand>();
        wait.Setup(m => m.Execute()).Callback(() => mre.WaitOne());

        IoC.Resolve<BlockingCollection<ICommand>>("Queue").Add(wait.Object);
    }

    [And("Разблокирован MRE")]
    public static void MreSet()
    {
        mre.Set();
    }

    [And("Добавлена команда выбрасывающая ошибку")]
    public static void ExceptCmdAdd()
    {
        var except = new Mock<ICommand>();
        except.Setup(m => m.Execute()).Throws(new Exception()).Verifiable();

        IoC.Resolve<BlockingCollection<ICommand>>("Queue").Add(except.Object);
    }

    [And("Добавлена команда hard-остановки")]
    public static void HardStopCmdAdd()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.HardStop", (object[] args) => {
            return new ActionCommand(() => {
                new HardStopServer((ServerThread)args[0]).Execute();
                try{
                    new ActionCommand((Action)args[1]).Execute();
                }
                catch{}
            });
        }).Execute();
        
        var stopcmd = IoC.Resolve<ICommand>("Commands.HardStop", IoC.Resolve<ServerThread>("Server"), () => {mreTests.Set();});

        IoC.Resolve<BlockingCollection<ICommand>>("Queue").Add(stopcmd);
    }

    [And("Добавлена команда soft-остановки")]
    public static void SoftStopCmdAdd()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.SoftStop", (object[] args) =>
        {
            try{
                return new SoftStopServer((ServerThread)args[0], (Action)args[1]);
            }
            catch{
                return new SoftStopServer((ServerThread)args[0], () => {});
            }
        }).Execute();

        var stopcmd = IoC.Resolve<ICommand>("Commands.SoftStop", IoC.Resolve<ServerThread>("Server"), () => {mreTests.Set();});
        IoC.Resolve<BlockingCollection<ICommand>>("Queue").Add(stopcmd);
    }

    [When("Запущен сервер")]
    public static void StartServer()
    {
        IoC.Resolve<ServerThread>("Server").Start();
    }

    [When("Попытка выполнить команду остановки из другого потока")]
    public void TryStopServerOutside(){
        var server = IoC.Resolve<ServerThread>("Server");
        var anotherThread = new Thread(() => {
            try{
                new HardStopServer(server).Execute();
            }
            catch(Exception e){
                this.e = e;
            }
            mreTests.Set();
        });

        anotherThread.Start();
    }
    
    [Then("Появляется ошибка")]
    public void AssertException(){
        mreTests.WaitOne();
        Assert.IsType<Exception>(e);
        mreTests.Reset();
    }

    [Then(@"В очереди остается (\d+) команд")]
    public static void Remains(int cmds)
    {
        mreTests.WaitOne();
        Assert.Equal(cmds, IoC.Resolve<BlockingCollection<ICommand>>("Queue").Count);
        mreTests.Reset();
    }
}