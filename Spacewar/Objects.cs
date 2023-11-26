namespace Spacewar;

public interface Order{
    public string orderName {get;}
    public ICommand cmd {get;}
    public object[] args {get;}

    // public Order(string IoC_obj, ICommand cmd, params object[] args){
    //     this.IoC_obj = IoC_obj;
    //     this.cmd = cmd;
    //     this.args = args;
    // }
}