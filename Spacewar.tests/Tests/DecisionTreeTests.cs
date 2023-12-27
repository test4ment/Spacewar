namespace Spacewar.Tests;
using D = System.Collections.Generic.Dictionary<int, object>;

public class BuildTreeTests
{
    public BuildTreeTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var tree = new D();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Collisions.Tree", (object[] args) =>
        {
            return tree;
        }).Execute();
    }

    [Fact]
    public void BuildCollisionTreeCommand_Positive()
    {
        var reader = new Mock<IArraysFromFileReader>();
        var path = "../../../CollisionVectors.txt";
        var arrays = File.ReadAllLines(path).Select(
            line => line.Split(" ").Select(num => int.Parse(num)).ToArray()
        ).ToList();

        var expected_layer_1 = new HashSet<int>() { 1, 6 };
        var expected_layer_2 = new HashSet<int>() { 2, 7 };
        var expected_layer_3 = new HashSet<int>() { 3, 8 };
        var expected_layer_4 = new HashSet<int>() { 4, 5, 9 };

        reader.Setup(r => r.ReadArrays()).Returns(arrays);

        var cmd = new DecisionTree_Read(reader.Object);
        cmd.Execute();

        var tree = IoC.Resolve<D>("Game.Collisions.Tree");

        var real_layer_1 = tree.Keys;
        var real_layer_2 = ((D)tree[1]).Keys.Union(((D)tree[6]).Keys);
        var real_layer_3 = ((D)((D)tree[1])[2]).Keys.Union(((D)((D)tree[6])[7]).Keys);
        var real_layer_4 = ((D)((D)((D)tree[1])[2])[3]).Keys.Union(((D)((D)((D)tree[6])[7])[8]).Keys);

        Assert.True(expected_layer_1.SequenceEqual(real_layer_1));
        Assert.True(expected_layer_2.SequenceEqual(real_layer_2));
        Assert.True(expected_layer_3.SequenceEqual(real_layer_3));
        Assert.True(expected_layer_4.SequenceEqual(real_layer_4));
    }

    [Fact]
    public void BuildCollisionTreeCommand_CantReadCollisionFile()
    {
        var reader = new Mock<IArraysFromFileReader>();
        reader.Setup(r => r.ReadArrays()).Throws(new NotImplementedException());

        var cmd = new DecisionTree_Read(reader.Object);

        Assert.Throws<NotImplementedException>(cmd.Execute);
    }
}
