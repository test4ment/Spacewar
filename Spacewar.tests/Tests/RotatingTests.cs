namespace Spacewar.Tests;

[FeatureFile(@"../../../Features/rotatecommand.feature")]
public class RotatingFeatures : Feature
{
    private readonly Mock<IRotateable> rotating_object = new Mock<IRotateable>();
    private Action _act = () => { };

    [Given(@"Для объекта, находящегося под углом к горизонту в \(-?(\d+)\) градусов и поворачивающегося со скоростью \(-?(\d+)\) градусов")]
    public void ObjectAngleVel(int ang1, int vel1)
    {
        rotating_object.Setup(obj => obj.angle).Returns(new Angle(ang1)).Verifiable();
        rotating_object.Setup(obj => obj.angle_velocity).Returns(new Angle(vel1)).Verifiable();
    }

    [When(@"Execute")]
    public void ExecuteRotate()
    {
        ICommand rotate = new RotateCommand(rotating_object.Object);
        _act = () => { rotate.Execute(); };
    }

    [Then(@"меняет поворот объекта на \(-?(\d+)\) градусов")]
    public void CheckAng(int ang1)
    {
        _act();
        rotating_object.VerifySet(m => m.angle = new Angle(ang1), Times.Once);
        rotating_object.VerifyAll();
    }

    [Then("Ошибка")]
    public void AssertExcept()
    {
        Assert.Throws<Exception>(() => { _act(); });
    }

    [Given("Объект, у которого невозможно прочитать значение угла наклона к горизонту")]
    public void RotatingCantGetAng()
    {
        rotating_object.Setup(obj => obj.angle).Throws(new Exception());
        rotating_object.Setup(obj => obj.angle_velocity).Returns(new Angle(90));
    }

    [Given("Объект, у которого невозможно прочитать значение угловой скорости")]
    public void RotatingCantGetVel()
    {
        rotating_object.Setup(obj => obj.angle).Returns(new Angle(45));
        rotating_object.Setup(obj => obj.angle_velocity).Throws(new Exception());
    }

    [Given("Объект, у которого невозможно изменить угол наклона к горизонту")]
    public void RotatingCantWriteAng()
    {
        rotating_object.Setup(obj => obj.angle).Returns(new Angle(45));
        rotating_object.Setup(obj => obj.angle_velocity).Returns(new Angle(90));
        rotating_object.SetupSet(obj => obj.angle = It.IsAny<Angle>()).Throws(new Exception());
    }
}
