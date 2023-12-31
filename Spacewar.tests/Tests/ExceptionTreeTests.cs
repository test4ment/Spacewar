namespace Spacewar.Tests;

[FeatureFile(@"../../../Features/exceptiondecide.feature")]
public class ExceptionFeatures : Feature
{

    public IDictionary<object, object> ExceptionTree = new Dictionary<object, object>();
    public IDictionary<object, object> manual_dict = new Dictionary<object, object>();

    [Given("Инициализирован IoC")]
    public static void IoCInit()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>(
                "Scopes.New",
                IoC.Resolve<object>("Scopes.Root")
            )
        ).Execute();
    }

    [And("Готовое дерево с ошибкой")]
    public void GotTree()
    {
        manual_dict = new Dictionary<object, object>() {
            {
                typeof(IMoveable),
                new Dictionary<object, object>() {
                    {typeof(NotSupportedException),
                    typeof(NotImplementedException)
                    }
                }
            }
        };
    }

    [When("Генерируется дерево обработчика")]
    public void GenTree()
    {
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
            typeof(NotImplementedException)
        ).Execute();
    }

    [Then("Сгенерированное дерево совпадает с ожидаемым")]
    public void CompareTree()
    {
        Assert.Equal(IoC.Resolve<Dictionary<object, object>>("Trees.Exceptions"), manual_dict);
    }
}
