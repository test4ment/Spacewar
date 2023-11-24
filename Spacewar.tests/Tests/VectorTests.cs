namespace Spacewar.Tests;

[FeatureFile(@"../../../Features/vector.feature")]
public class VectorFeatures : Feature
{
    public Vector? vec1;
    public Vector? vec2;
    public Func<Vector>? _act;
    public Func<bool>? _boolact;

    [Given(@"Вектор \(-?(\d+), -?(\d+)\) и вектор \(-?(\d+), -?(\d+)\)")]
    public void VectorMaker(int int1, int int2, int int3, int int4)
    {
        vec1 = new Vector(int1, int2);
        vec2 = new Vector(int3, int4);
    }

    [Given(@"Вектор \(-?(\d+)\) и вектор \(-?(\d+), -?(\d+)\)")]
    public void VectorMaker(int int1, int int2, int int3)
    {
        vec1 = new Vector(int1);
        vec2 = new Vector(int2, int3);
    }

    [When("складывать")]
    public void Sum()
    {
        _act = () => {return vec1 + vec2;};
    }

    [Then(@"получится вектор \(-?(\d+), -?(\d+)\)")]
    public void VectorEquals(int int1, int int2)
    {
        Assert.Equal(_act(), new Vector(int1, int2));
    }

    [When("сравнивать")]
    public void VectorCompare()
    {
        _boolact = () => {return vec1.Equals(vec2);};
    }

    [Then("результат ложь")]
    public void AssertFalse()
    {
        Assert.False(_boolact());
    }

    [Then("появляется ошибка")]
    public void AssertThrows()
    {
        Assert.Throws<ArgumentException>(() => {_act();});
    }
}
