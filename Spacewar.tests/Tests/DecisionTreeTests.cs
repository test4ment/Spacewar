namespace Spacewar.Tests;

// [FeatureFile(@"../../../Features/decisiontree_collision.feature")]
public class DecisionTreeFeatures : Feature{
    public Mock<IDecisionTree<int, Action>> decider = new Mock<IDecisionTree<int, Action>>();

    [Fact]
    public static void IoCInit(){
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Extract.CollisionFeatures", (params object[]) => {
            
        }).Execute();
    }

    [Fact]
    public void Test(){
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Tree.Collision").Execute();

        var a4 = new Dictionary<object, object>();
        a4.Add(-1, () => {throw new Exception();});
        var a3 = new Dictionary<object, object>();
        a3.Add(-1, a4);
        var a2 = new Dictionary<object, object>();
        a2.Add(1, a3);

        BinaryDecider.tree.Add(1, a2);
        
        decider.Setup(d => d.AddRecord(It.IsAny<int[]>())).Throws(new NotImplementedException());
        decider.Setup(d => d.GetDecision(It.IsAny<int[]>())).Callback(
            (int[] a) => 
                BinaryDecider.Decide(a)
        );

        var b = new int[]{1, 1, -1, -1};

        Assert.ThrowsAny<Exception>(decider.Object.GetDecision(b));
    }
}

// dx dy drx dry act
//  1  1  -1  -1  throw
internal static class BinaryDecider{
    public static Dictionary<object, object> tree {get;} = new Dictionary<object, object>();

    public static Action Decide(int[] features)
    {
        if (tree.ContainsKey(features[0]))
        {
            return Decide(features[1..]);
        }
        else
        {
            return () => {};
        }
        // return tree.GetValueOrDefault(features[0], () => {});
    }
}