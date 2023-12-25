using Hwdtech;

public class DecisionTree_Read : ICommand
{
    private readonly IArraysFromFileReader _reader;

    public DecisionTree_Read(IArraysFromFileReader reader)
    {
        _reader = reader;
    }

    public void Execute()
    {
        var arrays = _reader.ReadArrays();

        arrays.ForEach(array =>
        {
            var node = IoC.Resolve<Dictionary<int, object>>("Game.Collisions.Tree");
            array.ToList().ForEach(num =>
            {
                node.TryAdd(num, new Dictionary<int, object>());
                node = (Dictionary<int, object>)node[num];
            });
        });
    }
}