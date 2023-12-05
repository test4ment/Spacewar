namespace Spacewar.Tests;

// [FeatureFile(@"../../../Features/decisiontree_collision.feature")]
public class DecisionTreeFeatures : Feature{
    
    [Fact]
    public void Test(){
        var tree = new UniversalTree();
        object exceptionAction = () => {throw new Exception("Collision!");};
        
        UniversalTree.AddRecord(new object[]{1, 1, -1, -1}, exceptionAction, tree.tree);


        Assert.Equal(((Dictionary<object, object>)((Dictionary<object, object>)((Dictionary<object, object>)tree.tree[1])[1])[-1])[-1], exceptionAction);
    }
}

// dx dy drx dry act
//  1  1  -1  -1  throw