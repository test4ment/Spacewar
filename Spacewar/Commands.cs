namespace Spacewar;

public interface ICommand{
    public void Execute();
}

public interface IMoveable{
    public Double[] position {get; set;} // probably we should make Vector type
    public Double[] instant_velocity {get;}
}

public class MoveCommand: ICommand{
    IMoveable moving_object;

    public MoveCommand(IMoveable moving_object){
        this.moving_object = moving_object;
    }

    public void Execute(){
        moving_object.position = new Double[] {moving_object.position[0] += moving_object.instant_velocity[0], moving_object.position[1] += moving_object.instant_velocity[1]};
    }
}