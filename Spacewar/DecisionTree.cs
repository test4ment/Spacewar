namespace Spacewar;

public class DecisionTree_AddRecord : ICommand
{
    private readonly IDictionary<object, object> tree;
    private readonly object[] record;
    private readonly object value;

    public DecisionTree_AddRecord(IDictionary<object, object> tree, object[] record, object value)
    {
        this.tree = tree;
        this.record = record;
        this.value = value;
    }

    public void AddRecord(IDictionary<object, object> tree, object[] record, object value)
    {
        try
        {
            tree.TryAdd(record[0], new Dictionary<object, object>());
            AddRecord((IDictionary<object, object>)tree[record[0]], record[1..], value);
        }
        catch (IndexOutOfRangeException)
        {
            tree[record[0]] = value;
        }
    }

    public void Execute()
    {
        AddRecord(tree, record, value);
    }
}
