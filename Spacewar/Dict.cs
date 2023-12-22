public interface IDict<T, U>
{
    public U Get(T key);
    public void Set(T key, U value);
    public IDictionary<T, U> dict { get; }
}
