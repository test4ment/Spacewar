namespace Spacewar.Tests;

[FeatureFile(@"../../../Features/decisiontree_collision.feature")]
public class DecisionTreeFeatures : Feature
{
    public readonly object exceptionAction = () => { throw new Exception("Collision!"); };
    public Dictionary<object, object> manual_dict = new Dictionary<object, object>();
    public Action func = () => { };

    [Given("Заполненное дерево решений")]
    public void MakeTree()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>(
                "Scopes.New",
                IoC.Resolve<object>("Scopes.Root")
            )
        ).Execute();

        IDictionary<object, object> tree = new Dictionary<object, object>();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Trees.Collision",
            (object[] args) => { return tree; }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Trees.AddRecord",
            (object[] args) =>
            {
                return new DecisionTree_AddRecord(
                    (IDictionary<object, object>)args[0],
                    (object[])args[1],
                    args[2]);
            }
        ).Execute();

        IoC.Resolve<ICommand>(
            "Trees.AddRecord",
            IoC.Resolve<Dictionary<object, object>>("Trees.Collision"),
            new object[] { 1, 1, -1, -1 },
            exceptionAction
        ).Execute();
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

    [Then("Сгенерированное дерево совпадает с ожидаемым")]
    public void AssertDictOfDicts()
    {
        Assert.Equal(manual_dict, IoC.Resolve<Dictionary<object, object>>("Trees.Collision"));
    }
}
