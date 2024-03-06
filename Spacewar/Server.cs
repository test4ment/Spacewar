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
            try
            {
                c.Execute();
            }
            catch (Exception e)
            {
                IoC.Resolve<ICommand>("Exception.Handler", e, c).Execute();
            }
        };

        t = new Thread(() =>
        {
            while (!stop)
            {
                behaviour();
            }
        });
    }

    internal void Stop()
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

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj.GetType() == typeof(ServerThread))
        {
            return ((ServerThread)obj).GetHashCode() == t.GetHashCode();
        }

        if (obj.GetType() == typeof(Thread))
        {
            return ((Thread)obj).GetHashCode() == t.GetHashCode();
        }

        return false;
    }

    public override int GetHashCode()
    {
        return t.GetHashCode();
    }
}
