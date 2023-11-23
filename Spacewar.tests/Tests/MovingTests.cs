namespace Spacewar.Tests;

[FeatureFile(@"../../../Features/movecommand.feature")]
public class MovingFeatures : Feature
{
    Mock<IMoveable> moving_object = new Mock<IMoveable>();
    Exception? except;

    [Given(@"Для объекта, находящегося в точке \(-?(\d+), (-?\d+)\) и движущегося со скоростью \((-?\d+), (-?\d+)\)")]
    public void ObjectPositionVel(int pos1, int pos2, int vel1, int vel2)
    {
        moving_object.Setup(obj => obj.position).Returns(new Vector(pos1, pos2)).Verifiable();
        moving_object.Setup(obj => obj.instant_velocity).Returns(new Vector(vel1, vel2)).Verifiable();
    }

    [When(@"Execute")]
    public void ExecuteMove(){
        ICommand move = new MoveCommand(moving_object.Object);
        try{
            move.Execute();
        }
        catch (Exception e){
            except = e;
        }
    }

    [Then(@"движение меняет положение объекта на \(-?(\d+), -?(\d+)\)")]
    public void CheckPos(int pos1, int pos2){
        moving_object.VerifySet(m => m.position = new Vector(pos1, pos2), Times.Once);
        moving_object.VerifyAll();
    }

    [Then("Ошибка")]
    public void AssertExcept(){
        Assert.NotNull(except);
    }

    [Given("Объект, у которого невозможно прочитать положение объекта в пространстве")]
    public void MovingCantGetPos(){
        moving_object.Setup(obj => obj.position).Throws(new Exception());
        moving_object.Setup(obj => obj.instant_velocity).Returns(new Vector(-7, 3));
    }

    [Given("Объект, у которого невозможно прочитать значение мгновенной скорости")]
    public void MovingCantGetVel(){
        moving_object.Setup(obj => obj.position).Returns(new Vector(12, 5));
        moving_object.Setup(obj => obj.instant_velocity).Throws(new Exception());
    }

    [Given("Объект, у которого невозможно изменить положение в пространстве")]
    public void MovingCantWritePos(){
        moving_object.Setup(obj => obj.position).Returns(new Vector(12, 5));
        moving_object.Setup(obj => obj.instant_velocity).Returns(new Vector(-7, 3));
        moving_object.SetupSet(obj => obj.position = It.IsAny<Vector>()).Throws(new Exception());
    }
}
