using System.Collections.Concurrent;
using System.Threading;

public class ServerThread{
    private Thread t;
    public BlockingCollection<ICommand> q;
    private bool stop = false;
    private Action behaviour;

    public ServerThread(BlockingCollection<ICommand> queue){
        q = queue;
        behaviour = () => {
            var c = q.Take();
            try{
                c.Execute();
            }
            catch(Exception e){
                IoC.Resolve<ICommand>("Trees.Exceptions.Handler", c, e);
            }
        };

        t = new Thread(() => {
            while(!stop){
                behaviour();
            }
        });
    }

    public void Stop(){
        stop = true;
    }

    public void SetBehaviour(Action newBeh){
        behaviour = newBeh;
    }

}