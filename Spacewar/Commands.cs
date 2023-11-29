using System.Runtime.InteropServices;

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
        // var command = new MacroCommand(
        //     IoC.Resolve<ICommand>(
        //         order.cmd, 
        //         IoC.Resolve<UObject>(order.objectName)),
        //         );

        // var command = IoC.Resolve<ICommand>(
        //     order.cmd, 
        //     IoC.Resolve<UObject>(order.objectName)
        // );

        var command = new ContiniousObjectCommand(order.objectName, order.cmd);

        foreach(var i in (Dictionary<string, object>)order.args.dict){
            IoC.Resolve<UObject>(order.objectName).properties.Set(i.Key, i.Value);
        }

        IoC.Resolve<UObject>(order.objectName).properties.Set(
            order.cmd,
            IoC.Resolve<ICommand>(
                order.cmd,
                IoC.Resolve<UObject>(order.objectName)
            )
        );
        // IoC.Resolve<UObject>(order.objectName).properties.Set("Commands", )

        IoC.Resolve<IQueue<ICommand>>("Game.Queue").Put(command);

        // ((ICommand)(IoC.Resolve<UObject>(
        //     order.objectName
        // ).properties.Get(order.cmd))).Execute();
    }

    public StartCommand(Order order)
    {
        this.order = order;
    }
}

// public class MacroCommand : ICommand{
//     private readonly List<ICommand> commands = new List<ICommand>();

//     public MacroCommand(params ICommand[] commands){
//         this.commands = new List<ICommand>(commands);
//     }

//     public void Execute()
//     {
//         foreach (var i in commands)
//         {
//             i.Execute();
//         }
//     }
// }

public class ContiniousObjectCommand : ICommand{
    private readonly string obj;
    private readonly string cmd;

    public void Execute(){
        ((ICommand)(IoC.Resolve<UObject>(obj).properties.Get(cmd))).Execute();
        IoC.Resolve<IQueue<ICommand>>("Game.Queue").Put(new ContiniousObjectCommand(obj, cmd));
    }

    public ContiniousObjectCommand(string obj, string cmd){
        this.obj = obj;
        this.cmd = cmd;
    }
}