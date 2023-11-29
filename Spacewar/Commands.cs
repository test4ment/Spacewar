namespace Spacewar;

public interface ICommand
{
    public void Execute();
}

public interface IMoveable
{
    public Vector position { get; set; }
    public Vector instant_velocity { get; }
}

public class MoveCommand : ICommand
{
    private readonly IMoveable moving_object;

    public MoveCommand(IMoveable moving_object)
    {
        this.moving_object = moving_object;
    }

    public void Execute()
    {
        moving_object.position += moving_object.instant_velocity;
    }
}

public class StartCommand : ICommand
{
    private readonly Order order;

    public void Execute()
    {
        var command = new MacroCommand(
            IoC.Resolve<ICommand>(
                order.cmd, 
                IoC.Resolve<UObject>(order.objectName)),
                );

        foreach(var i in (Dictionary<string, object>)order.args){
            IoC.Resolve<UObject>(order.objectName).properties.Set(i.Key, i.Value);
        }

        IoC.Resolve<UObject>(order.objectName).properties.Set(order.cmd, command);
        IoC.Resolve<UObject>(order.objectName).properties.Set("Commands", )

        IoC.Resolve<IQueue<ICommand>>("Game.Queue").Put(command);

        // Команда закладывается в объект, в очередь закладывается команда которая итерирует команды в объекте
    //     1. Resolve "Game.Object.SetValues"(initialValues)
    // 2. var operation = Resolve "Game.Operation.{order.name}"(order.target)
    // 3. target.SetProperty("order.name", operation)
    // 4. Resolve "Game.Queue.Add"(operation)
    }

    public StartCommand(Order order)
    {
        this.order = order;
    }
}

public class MacroCommand : ICommand{
    private readonly List<ICommand> commands = new List<ICommand>();

    public MacroCommand(params ICommand[] commands){
        this.commands = new List<ICommand>(commands);
    }

    public void Execute(){
        foreach(var i in commands){
            i.Execute();
        }
    }
}
