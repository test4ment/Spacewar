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
