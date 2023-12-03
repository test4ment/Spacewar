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

public class CheckCollision : ICommand{
    public List<int> features = new List<int>();
    public UObject object1;
    public UObject object2;
    
    public CheckCollision(UObject object1, UObject object2){
        this.object1 = object1;
        this.object2 = object2;
    }

    public void Execute(){
        var obj1_pos = ((Vector)object1.properties.Get("Position"))._values;
        var obj1_vel = ((Vector)object1.properties.Get("Velocity"))._values;
        
        var obj2_pos = ((Vector)object2.properties.Get("Position"))._values;
        var obj2_vel = ((Vector)object2.properties.Get("Velocity"))._values;
        
        features = new List<int>();
        
        ((List<(int, int)>)obj1_pos.Zip(obj2_pos)).ForEach(
            Items => {
                features.Add(Items.Item2 - Items.Item1);
            }
        );
        ((List<(int, int)>)obj1_vel.Zip(obj2_vel)).ForEach(
            Items => {
                features.Add(Items.Item2 - Items.Item1);
            }
        );

        IoC.Resolve<ICommand>("Tree.Collision", features).Execute();
    }
}

public class ActionCommand : ICommand
{
    private readonly Action _action;
    public ActionCommand(Action action) => _action = action;
    public void Execute()
    {
        _action();
    }
}