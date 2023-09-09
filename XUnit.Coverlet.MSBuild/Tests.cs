namespace XUnit.Coverlet.Collector;

public class UnitTest1
{
    [Theory]
    [InlineData(12.0, 5.0, -7.0, 3.0)]
    public void CanMove(Double posx, Double posy, Double velx, Double vely){
        Spaceship ship = new Spaceship();
        ship.SetPos(posx, posy);
        ship.SetVel(velx, vely);
        ship.Move();
        Assert.Equal(ship.GetPos(), new Double[]{posx + velx, posy + vely});
    }

    [Fact]
    public void NoPosCanMove(){
        Spaceship ship = new Spaceship();
        Assert.ThrowsAny<Exception>(() => ship.Move());
    }

    [Fact]
    public void NoVelCanMove(){
        Spectator spec = new Spectator();
        Assert.ThrowsAny<Exception>(() => spec.Move());
    }

    [Fact]
    public void CannotMove(){
        Turret turret = new Turret();
        Assert.ThrowsAny<Exception>(() => turret.Move());
    }
}