namespace Spacewar.Tests;

[FeatureFile(@"../../../Features/decisiontree_collision.feature")]
public class DecisionTreeFeatures : Feature
{
    public ITree<object, object> tree = new UniversalTree();
    public readonly object exceptionAction = () => { throw new Exception("Collision!"); };
    public Dictionary<object, object> manual_dict = new Dictionary<object, object>();
    public Action func = () => { };

    [Given("Заполненное дерево решений")]
    public void MakeTree()
    {
        UniversalTree.AddRecord(new object[] { 1, 1, -1, -1 }, exceptionAction, tree.tree);
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

        func = () => { Assert.Equal(manual_dict, tree.tree); };
    }

    [Then("Дерево состоит из словаря словарей")]
    public void AssertDictOfDicts()
    {
        func();
    }
}
