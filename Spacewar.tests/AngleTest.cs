namespace Spacewar.Tests;

[FeatureFile(@"../../../Features/Angle.feature")]
public class AngleFeatures : Feature
{
    public Angle ang1 = new Angle(0);
    public Angle? ang2 = null;
    public Angle ang3 = new Angle(0);
    public Func<Angle> _act = () => { return new Angle(0); };
    public Func<bool> _boolact = () => { return true; };

    [Given(@"Угол \(-?(\d+)\) и угол \(-?(\d+)\)")]
    public void VectorMaker(int int1, int int2)
    {
        ang1 = new Angle(int1);
        ang3 = new Angle(int2);
    }

    [Given(@"Угол \(-?(\d+)\) и null")]
    public void VectorAndNull(int int1)
    {
        ang1 = new Angle(int1);
        ang2 = null;
    }

    [When("сравнивать")]
    public void Compare()
    {
        _boolact = () => { return ang1.Equals(ang2); };
    }

    [When("складывать")]
    public void Sum()
    {
        _act = () => { return ang1 + ang3; };
    }

    [When("складывать с null")]
    public void Sumn()
    {
        _act = () =>
        {
            if (ang1 != null && ang2 != null)
            {
                return ang1 + ang2;
            }
            else
            {
                throw new NullReferenceException();
            }
        };
    }

    [Then(@"получится угол \(-?(\d+)\)")]
    public void VectorEquals(int int1)
    {
        Assert.Equal(_act(), new Angle(int1));
    }

    [Then("появляется ошибка")]
    public void AssertThrows()
    {
        Assert.Throws<NullReferenceException>(() => { _act(); });
    }

    [Then("результат ложь")]
    public void AssertFalse()
    {
        Assert.False(_boolact());
    }
}
