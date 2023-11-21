namespace Spacewar;

public interface ICommand
{
    public void Execute();
}

public interface IMoveable
{
    public int[] position { get; set; } // probably we should make Vector type
    public int[] instant_velocity { get; }
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
        var curr_pos = moving_object.position;
        var vel = moving_object.instant_velocity;
        moving_object.position = new int[] { curr_pos[0] += vel[0], curr_pos[1] += vel[1] };
    }
}
