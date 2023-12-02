namespace Spacewar;

public class MoveableAdapter : IMoveable
{
    private readonly UObject _obj;
    public MoveableAdapter(UObject obj)
    {
        _obj = obj;
    }
    public Vector position
    {
        get => (Vector)_obj.properties.Get("Position");
        set => _obj.properties.Set("Position", value);
    }
    public Vector instant_velocity => (Vector)_obj.properties.Get("Velocity");
}
