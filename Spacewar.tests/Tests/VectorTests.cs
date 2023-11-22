namespace Spacewar.Tests;

[FeatureFile(@"..\..\..\Features\vector.feature")]
public class VectorFeatures: Feature{
    public Vector? vec1;
    public Vector? vec2;
    public Vector? sum;
    public Exception? e;
    public bool equality;

    [Given(@"Вектор \(-?(\d+), -?(\d+)\) и вектор \(-?(\d+), -?(\d+)\)")]
    public void VectorMaker(int int1, int int2, int int3, int int4){
        vec1 = new Vector(int1, int2);
        vec2 = new Vector(int3, int4);
    }

    [Given(@"Вектор \(-?(\d+)\) и вектор \(-?(\d+), -?(\d+)\)")]
    public void VectorMaker(int int1, int int2, int int3){
        vec1 = new Vector(int1);
        vec2 = new Vector(int2, int3);
    }

    [When("складывать")]
    public void Sum(){
        try{
            sum = vec1 + vec2;
        }
        catch (Exception e){
            this.e = e;
        }
    }

    [Then(@"получится вектор \(-?(\d+), -?(\d+)\)")]
    public void VectorEquals(int int1, int int2){   
        Assert.Equal(sum, new Vector(int1, int2));
    }

    [When("сравнивать")]
    public void VectorCompare(){
        equality = vec1.Equals(vec2);
    }

    [Then("результат ложь")]
    public void AssertFalse(){
        Assert.False(equality);
    }

    [Then("появляется ошибка")]
    public void AssertThrows(){
        Assert.NotNull(e);
    }
}