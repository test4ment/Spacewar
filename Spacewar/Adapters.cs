namespace Spacewar;

public class RotateableAdapter : IRotateable
{
    private readonly UObject _obj;
    public RotateableAdapter(UObject obj)
    {
        _obj = obj;
    }
    public Angle angle
    {
        get => (Angle)_obj.properties.Get("Angle");
        set => _obj.properties.Set("Angle", value);
    }
    public Angle angle_velocity => (Angle)_obj.properties.Get("Velocity");
}
