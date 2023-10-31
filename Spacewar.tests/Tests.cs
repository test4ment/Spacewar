namespace Spacewar.Tests;

public class UnitTest1
{
    // Всё работает
    [Fact]
    public void MovingSuccessfly()
    {
        var moving_object = new Mock<IMoveable>();

        moving_object.Setup(obj => obj.position).Returns(new double[] { 12, 5 }).Verifiable();
        moving_object.Setup(obj => obj.instant_velocity).Returns(new double[] { -7, 3 }).Verifiable();

        ICommand move = new MoveCommand(moving_object.Object);

        move.Execute();

        // Assert.Equal(new double[] { 5, 8 }, moving_object.Object.position);
        moving_object.VerifySet(m => m.position = new double[] { 5, 8 }, Times.Once);
        moving_object.VerifyAll();
    }

    [Fact]
    public void MovingCantGetPos()
    {
        var moving_object = new Mock<IMoveable>();

        moving_object.Setup(obj => obj.position).Throws(new Exception());
        moving_object.Setup(obj => obj.instant_velocity).Returns(new double[] { -7, 3 });

        ICommand move = new MoveCommand(moving_object.Object);

        Assert.Throws<Exception>(() => move.Execute());
    }

    [Fact]
    public void MovingCantGetVel()
    {
        var moving_object = new Mock<IMoveable>();

        moving_object.Setup(obj => obj.position).Returns(new double[] { 12, 5 });
        moving_object.Setup(obj => obj.instant_velocity).Throws(new Exception());

        ICommand move = new MoveCommand(moving_object.Object);

        Assert.Throws<Exception>(() => move.Execute());
    }

    [Fact]
    public void MovingCantWritePos()
    {
        var moving_object = new Mock<IMoveable>();

        moving_object.Setup(obj => obj.position).Returns(new double[] { 12, 5 });
        moving_object.Setup(obj => obj.instant_velocity).Returns(new double[] { -7, 3 });

        moving_object.SetupSet(obj => obj.position = It.IsAny<double[]>()).Throws(new Exception());
        ICommand move = new MoveCommand(moving_object.Object);

        Assert.Throws<Exception>(() => move.Execute());
    }
}
