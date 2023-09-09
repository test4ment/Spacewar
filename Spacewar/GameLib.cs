namespace Spacewar;

public interface IMoveable
{
    public void Move();
}

abstract public class Object: IMoveable{
    Double[]? position;
    Double[]? instant_velocity;
    public abstract void Move();
}

public class Spaceship: Object{
    Double[]? position;
    Double[]? instant_velocity;

    public Spaceship(Double posx, Double posy){
        this.position = new Double[] {posx, posy};
    }

    public Spaceship(){}

    public void SetPos(Double posx, Double posy){
        this.position = new Double[] {posx, posy};
    }

    public void SetVel(Double posx, Double posy){
        this.instant_velocity = new Double[] {posx, posy};
    }

    public Double[]? GetPos(){
        return this.position;
    }

    public Double[]? GetVel(){
        return this.instant_velocity;
    }

    override public void Move(){
        this.position[0] += this.instant_velocity[0];
        this.position[1] += this.instant_velocity[1];
    }
}

public class Turret: Object{
    Double[]? position;
    override public void Move(){
        throw new Exception();
    }
}

public class Spectator: Object{
    override public void Move(){
        throw new NotImplementedException();
    }
}
