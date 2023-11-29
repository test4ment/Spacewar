namespace Spacewar;

public interface Order
{
    public string objectName { get; }
    public string cmd { get; }
    public IDict<string, object> args { get; }
}

public abstract class UObject{
    public IDict<string, object> properties;

    private UObject(IDict<string, object> properties){
        this.properties = properties;
    }
}
