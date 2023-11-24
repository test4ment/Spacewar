public class Vector
{
    public int[] _values { get; }

    public Vector(params int[] values)
    {
        _values = values;
    }

    public static Vector operator +(Vector left, Vector right)
    {
        if (left._values.Length != right._values.Length)
        {
            throw new ArgumentException();
        }

        var temp = new int[left._values.Length];

        for (var i = 0; i < left._values.Length; i++)
        {
            temp[i] = left._values[i] + right._values[i];
        }

        return new Vector(temp);
    }

    public override bool Equals(object obj)
    {
        return GetHashCode() == ((Vector)obj).GetHashCode();
    }

    public override int GetHashCode()
    {
        var hash = 0;
        for (var i = 0; i < _values.Length; i++)
        {
            hash += _values[i].GetHashCode() << (2 + i);
        }

        return hash;
    }
}
