namespace Spacewar;

public interface ICommand
{
    public void Execute();
}

public interface IRotateable
{
    public Angle angle { get; set; }
    public Angle angle_velocity { get; }
}

public interface IArraysFromFileReader
{
    public List<int[]> ReadArrays();
}

public class RotateCommand : ICommand
{
    private readonly IRotateable rotating_object;

    public RotateCommand(IRotateable rotating_object)
    {
        this.rotating_object = rotating_object;
    }

    public void Execute()
    {
        rotating_object.angle += rotating_object.angle_velocity;
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
