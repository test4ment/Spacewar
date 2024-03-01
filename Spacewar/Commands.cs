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
        var command = new ContiniousObjectCommand(order.target, order.cmd);

        order.args.dict.ToList().ForEach(pair =>
            order.target.properties.Set(pair.Key, pair.Value)
        );

        order.target.properties.Set(
            order.cmd,
            IoC.Resolve<ICommand>(
                order.cmd,
                order.target
            )
        );

        IoC.Resolve<IQueue<ICommand>>("Game.Queue").Put(command);
    }

    public StartCommand(Order order)
    {
        this.order = order;
    }
}

public class ContiniousObjectCommand : ICommand
{
    private readonly UObject obj;
    private readonly string cmd;

    public void Execute()
    {
        ((ICommand)obj.properties.Get(cmd)).Execute();
        IoC.Resolve<IQueue<ICommand>>("Game.Queue").Put(this);
    }

    public ContiniousObjectCommand(UObject obj, string cmd)
    {
        this.obj = obj;
        this.cmd = cmd;
    }
}

public class HardStopServer : ICommand
{
    private readonly ServerThread server;

    public HardStopServer(ServerThread server)
    {
        this.server = server;
    }

    public void Execute()
    {
        server.Stop();
    }
}

public class SoftStopServer : ICommand
{
    private readonly ServerThread server;
    private readonly Action action;

    public SoftStopServer(ServerThread server, Action action)
    {
        this.server = server;
        this.action = action;
    }

    public void Execute()
    {
        server.SetBehaviour(() =>
        {
            if (server.q.Count == 0)
            {
                server.Stop();
                action();
            }
            else
            {
                var c = server.q.Take();
                try{
                    c.Execute();
                }
                catch(Exception e){
                    IoC.Resolve<ICommand>("Exception.Handler", e, c).Execute();
                }
            }
        });
    }
}
