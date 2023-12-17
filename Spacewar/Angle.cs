namespace Spacewar;

public class Angle
{
    public int _angle;

    public Angle(int angle)
    {
        _angle = angle;
    }

    public override bool Equals(object? obj)
    {
        return GetHashCode() == ((Angle)obj).GetHashCode();
    }

    public static Angle operator +(Angle a, Angle b)
    {
        return new Angle((a._angle + b._angle) % 360);
    }

    public override int GetHashCode()
    {
        return _angle.GetHashCode();
    }
}
