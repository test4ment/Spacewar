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
    
    [And("Запущен сервер")]
    public void ServerInit(){
        ServerThread server = new ServerThread(q);

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Queue",
            (object[] args) => {return q;}
        ).Execute();
    }

    [And("Добавлена пустая команда")]
    public void NopCmdAdd(){
        SomeCommand nop = new SomeCommand();
        q.Add(nop);
    }

    [And("Добавлена команда hard-остановки")]
    public void NopCmdAdd(){
        HardStopServer stopcmd = new HardStopServer(server);
        q.Add(stopcmd);
    }

    [Then(@"В очереди остается (\d+) команд")]
    public void Remains(int cmds){
        Assert.Equal(q.Count, 1);
    }
}