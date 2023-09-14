namespace Spacewar.Tests;

public class UnitTest1
{   
    // Всё работает
    [Fact]
    public void MovingSuccessfly(){
        Mock<IMoveable> moving_object = new Mock<IMoveable>();

        moving_object.Setup(obj => obj.position).Returns(new Double[] {12, 5});
        moving_object.Setup(obj => obj.instant_velocity).Returns(new Double[] {-7, 3});

        ICommand move = new MoveCommand(moving_object.Object);

        move.Execute();

        Assert.Equal(new Double[] {5, 8}, moving_object.Object.position);
    }

    [Fact]
    public void MovingCantGetPos(){
        Mock<IMoveable> moving_object = new Mock<IMoveable>();

        moving_object.Setup(obj => obj.position).Throws(new Exception());
        moving_object.Setup(obj => obj.instant_velocity).Returns(new Double[] {-7, 3});

        ICommand move = new MoveCommand(moving_object.Object);

        Assert.Throws<Exception>(() => move.Execute());
    }

    [Fact]
    public void MovingCantGetVel(){
        Mock<IMoveable> moving_object = new Mock<IMoveable>();

        moving_object.Setup(obj => obj.position).Returns(new Double[] {12, 5});
        moving_object.Setup(obj => obj.instant_velocity).Throws(new Exception());

        ICommand move = new MoveCommand(moving_object.Object);

        Assert.Throws<Exception>(() => move.Execute());
    }

    [Fact]
    public void MovingCantWritePos(){
        Mock<IMoveable> moving_object = new Mock<IMoveable>();

        moving_object.Setup(obj => obj.position).Returns(new Double[] {12, 5});
        moving_object.Setup(obj => obj.instant_velocity).Returns(new Double[] {-7, 3});

        moving_object.SetupSet(obj => obj.position = It.IsAny<Double[]>()).Throws(new Exception());
        ICommand move = new MoveCommand(moving_object.Object);

        Assert.Throws<Exception>(() => move.Execute());
    }
}