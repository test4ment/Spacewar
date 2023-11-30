public interface IDict<T, U>
{
    public U Get(T key);
    public void Set(T key, U value);
    public Dictionary<string, object> dict { get; }
}
