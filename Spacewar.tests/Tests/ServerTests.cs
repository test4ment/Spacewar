namespace Spacewar.Tests;
using System.Collections.Concurrent;

[FeatureFile(@"../../../Features/server.feature")]
public class ServerFeatures : Feature{
    [Given("Инициализирован IoC")]
    public void IoCInit(){
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
    public void QueueInit(){
        BlockingCollection<ICommand> q = new BlockingCollection<ICommand>();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Queue",
            (object[] args) => {return q;}
        ).Execute();
    }
    
    [And("Создан сервер")]
    public void ServerInit(){
        ServerThread server = new ServerThread(
            IoC.Resolve<BlockingCollection<ICommand>>("Queue")
        );

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Server",
            (object[] args) => {return server;}
        ).Execute();
    }

    [And("Добавлена пустая команда")]
    public void NopCmdAdd(){
        SomeCommand nop = new SomeCommand();
        IoC.Resolve<BlockingCollection<ICommand>>("Queue").Add(nop);
    }

    [And("Добавлена команда hard-остановки")]
    public void HardStopCmdAdd(){
        HardStopServer stopcmd = new HardStopServer(IoC.Resolve<ServerThread>("Server"));
        IoC.Resolve<BlockingCollection<ICommand>>("Queue").Add(stopcmd);
    }

    [And("Добавлена команда soft-остановки")]
    public void SoftStopCmdAdd(){
        SoftStopServer stopcmd = new SoftStopServer(IoC.Resolve<ServerThread>("Server"));
        IoC.Resolve<BlockingCollection<ICommand>>("Queue").Add(stopcmd);
    }

    [And("Добавлена долгая операция")]
    public void AddThinking(){
        LongComputingCommand longcmd = new LongComputingCommand();
        IoC.Resolve<BlockingCollection<ICommand>>("Queue").Add(longcmd);
    }

    [And(@"Ждать (\d+) секунд")]
    public void WaitFor(int seconds){
        Thread.Sleep(seconds * 1000);
    }

    [When("Запущен сервер")]
    public void StartServer(){
        IoC.Resolve<ServerThread>("Server").Start();
    }

    [Then(@"В очереди остается (\d+) команд")]
    public void Remains(int cmds){
        for(var i = 0; i < 10; i++){}
        Assert.Equal(cmds, IoC.Resolve<BlockingCollection<ICommand>>("Queue").Count);
    }

    internal class LongComputingCommand : ICommand{
        public LongComputingCommand(){}
        public void Execute(){
            Thread.Sleep(3000);
        }
    }
}