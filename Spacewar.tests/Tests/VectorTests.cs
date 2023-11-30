namespace Spacewar.Tests;

[FeatureFile(@"../../../Features/vector.feature")]
public class VectorFeatures : Feature
{
    public Vector vec1 = new Vector();
    public Vector? vec2 = new Vector();
    public Func<Vector> _act = () => {return new Vector();};
    public Func<bool> _boolact = () => {return true;};

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

    [Given(@"Вектор \(-?(\d+)\) и null")]
    public void VectorAndNull(int int1){
        vec1 = new Vector(int1);
        vec2 = null;
    }

    [When("складывать")]
    public void Sum()
    {
        _act = () => { return vec1 + vec2; };
    }

    [When("складывать в другом порядке")]
    public void SumRight()
    {
        _act = () => { return vec2 + vec1; };
    }

    [When("сравнивать")]
    public void Compare(){
        _boolact = () => {return vec1.Equals(vec2);};
    }

    [Then(@"получится вектор \(-?(\d+), -?(\d+)\)")]
    public void VectorEquals(int int1, int int2)
    {
        Assert.Equal(_act(), new Vector(int1, int2));
    }

    [Then("появляется ошибка")]
    public void AssertThrows()
    {
        Assert.Throws<ArgumentException>(() => { _act(); });
    }

    [Then("результат ложь")]
    public void AssertFalse()
    {
        Assert.False(_boolact());
    }
}
