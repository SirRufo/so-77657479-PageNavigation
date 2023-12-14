namespace WpfApp.Navigation;

public interface INavigationService
{
    Task GotoAsync( string route );
    Task GotoAsync( string route, IDictionary<string, object> parameters );
}

