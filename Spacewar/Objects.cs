namespace Spacewar;

public interface Order
{
    public UObject target { get; }
    public string cmd { get; }
    public IDict<string, object> args { get; }
}

public class UObject
{
    public IDict<string, object> properties;

    public UObject(IDict<string, object> properties)
    {
        this.properties = properties;
    }
}
