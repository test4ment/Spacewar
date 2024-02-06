namespace Spacewar;
using System.Collections.Concurrent;

public class ServerThread
{
    private readonly Thread t;
    public BlockingCollection<ICommand> q;
    private bool stop = false;
    private Action behaviour;

    public ServerThread(BlockingCollection<ICommand> queue)
    {
        q = queue;
        behaviour = () =>
        {
            var c = q.Take();
            c.Execute();
        };

        t = new Thread(() =>
        {
            while (!stop)
            {
                behaviour();
            }
        });
    }

    public void Stop()
    {
        stop = true;
    }

    public void SetBehaviour(Action newBeh)
    {
        behaviour = newBeh;
    }

    public void Start()
    {
        t.Start();
    }
}
