namespace Spacewar.Tests;

[FeatureFile(@"../../../Features/decisiontree_collision.feature")]
public class DecisionTreeFeatures : Feature
{
    // public ITree<object, object> tree = new UniversalTree();
    public IDictionary<object, object> tree = new Dictionary<object, object>();
    public readonly object exceptionAction = () => { throw new Exception("Collision!"); };
    public Dictionary<object, object> manual_dict = new Dictionary<object, object>();
    public Action func = () => { };

    [Given("Заполненное дерево решений")]
    public void MakeTree()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        // UniversalTree.AddRecord(new object[] { 1, 1, -1, -1 }, exceptionAction, tree.tree);

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Trees.Collision",
            (object[] args) => { return tree; }
        ).Execute();

            // (IDictionary<object, object> tree, object[] record, object value) =>
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Trees.AddRecord",
            (object[] args) =>
            {
                var tree = (IDictionary<object, object>)args[0];
                var record = (object[])args[1];
                var value = args[2];
                try
                {
                    tree.TryAdd(record[0], new Dictionary<object, object>());
                    IoC.Resolve<bool>("Trees.AddRecord", (IDictionary<object, object>)tree[record[0]], record[1..], value);
                }
                catch (IndexOutOfRangeException)
                {
                    tree[record[0]] = value;
                    return (object)true;
                }
                return (object)false;
            }
        ).Execute();

        IoC.Resolve<bool>(
            "Trees.AddRecord",
            IoC.Resolve<Dictionary<object, object>>("Trees.Collision"),
            new object[] { 1, 1, -1, -1 }, 
            exceptionAction
        );
        //     public static void AddRecord(object[] record, object value, IDictionary<object, object> tree)
        // {
        // try
        // {
        //     _ = tree.TryAdd(record[0], new Dictionary<object, object>());
        //     AddRecord(record[1..], value, (IDictionary<object, object>)tree[record[0]]);
        // }
        // catch (IndexOutOfRangeException)
        // {
        //     tree[record[0]] = value;
        // }
        // }
    }

    [When("Сравнение сгенерированного и сделанного вручную дерева")]
    public void CompareTrees()
    {
        var branch4 = new Dictionary<object, object>
        {
            { -1, exceptionAction }
        };
        var branch3 = new Dictionary<object, object>
        {
            { -1, branch4 }
        };
        var branch2 = new Dictionary<object, object>
        {
            { 1, branch3 }
        };
        manual_dict = new Dictionary<object, object>
        {
            { 1, branch2 }
        };
    }

    [Then("Дерево состоит из словаря словарей")]
    public void AssertDictOfDicts()
    {
        Assert.Equal(manual_dict, IoC.Resolve<Dictionary<object, object>>("Trees.Collision"));
    }

    internal class ActionCommand : ICommand
    {
        private readonly Action<object[], object, IDictionary<object, object>> action;
        private readonly object[] arg1;
        private readonly object arg2;
        private readonly IDictionary<object, object> arg3;
        public ActionCommand(Action<object[], object, IDictionary<object, object>> action, object[] arg1, object arg2, IDictionary<object, object> arg3)
        {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
        }
        public void Execute()
        {
            action(arg1, arg2, arg3);
        }
    }
}
