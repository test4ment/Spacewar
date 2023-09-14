namespace Spacewar;



public class Spaceship: IMoveable{
    public Double[] position {get; set;}
    public Double[] instant_velocity {get; set;}

    public Spaceship(Double posx, Double posy, Double velx, Double vely){
        this.position = new Double[] {posx, posy};
        this.instant_velocity = new Double[] {velx, vely};
    }
}

public class PhotonTorpedo: IMoveable{
    public Double[] position {get; set;}
    public Double[] instant_velocity {get; set;}

    public PhotonTorpedo(Double posx, Double posy, Double velx, Double vely){
        this.position = new Double[] {posx, posy};
        this.instant_velocity = new Double[] {velx, vely};
    }
}

// public class Turret: Object{
//     Double[]? position;
//     override public void Move(){
//         throw new Exception();
//     }
// }

// public class Spectator: Object{
//     override public void Move(){
//         throw new NotImplementedException();
//     }
// }
