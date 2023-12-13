namespace Spacewar.Tests;

public class ExceptionFeatures : Feature
{

    public IDictionary<object, object> ExceptionTree = new Dictionary<object, object>();

    [Fact]
    public void Test()
    {

        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Trees.Exceptions",
            (object[] args) => { return ExceptionTree; }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Trees.AddRecord",
            (object[] args) =>
            {
                return new DecisionTree_AddRecord(
                    (IDictionary<object, object>)args[0],
                    (object[])args[1],
                    args[2]);
            }
        ).Execute();

        IoC.Resolve<ICommand>(
            "Trees.AddRecord",
            IoC.Resolve<IDictionary<object, object>>("Trees.Exceptions"),
            new object[] { typeof(IMoveable), typeof(NotSupportedException) },
            typeof(NotImplementedException) // new ActionCommand(() => {}) // throw new NotImplementedException();
        ).Execute();

        var manual_dict = new Dictionary<object, object>() {
            {
                typeof(IMoveable),
                new Dictionary<object, object>() {
                    {typeof(NotSupportedException),
                    typeof(NotImplementedException) 
                    // new ActionCommand(() => {})
                    }
                }
            }
        };

        Assert.Equal(IoC.Resolve<Dictionary<object, object>>("Trees.Exceptions"), manual_dict);
    }
}
