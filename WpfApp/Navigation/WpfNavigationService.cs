using System.Reflection;
using System.Windows.Navigation;

namespace WpfApp.Navigation;

internal class WpfNavigationService : INavigationService
{
    private readonly App _app;
    private readonly IServiceProvider _services;
    private FrameworkElement? _view;
    private bool _clearNavigationStack;
    private bool _eventHandlerSet;

    public WpfNavigationService( App app, IServiceProvider services )
    {
        _app = app;
        _services = services;
    }

    private void NavigationService_Navigated( object sender, NavigationEventArgs e )
    {
        if ( ReferenceEquals( e.Content, _view ) && _clearNavigationStack )
        {
            var ns = ( (NavigationWindow)_app.MainWindow ).NavigationService;
            while ( ns.CanGoBack ) ns.RemoveBackEntry();
        }
    }

    public Task GotoAsync( string route )
        => OnGotoAsync( route );

    public Task GotoAsync( string route, IDictionary<string, object> parameters )
        => OnGotoAsync( route, parameters );

    private async Task OnGotoAsync( string route, IDictionary<string, object>? parameters = null )
    {
        await Task.Yield();

        if ( _app.MainWindow is null ) return;
        if ( _eventHandlerSet == false )
        {
            _eventHandlerSet = true;
            ( (NavigationWindow)_app.MainWindow ).NavigationService.Navigated += NavigationService_Navigated;
        }

        bool clearNavigationStack = false;
        if ( route.StartsWith( "//" ) )
        {
            clearNavigationStack = true;
            route = route.Substring( 2 );
        }

        if ( _app.MainWindow is NavigationWindow nav )
        {
            var view = Routing.GetOrCreateContent( route, _services );

            if ( view is null ) return;

            if ( parameters is not null )
            {
                ApplyQueryParameters( view, parameters );
                if ( view.DataContext is not null )
                    ApplyQueryParameters( view.DataContext, parameters );
            }

            nav.Navigate( view );

            _view = view;
            _clearNavigationStack = clearNavigationStack;
        }
    }

    private void ApplyQueryParameters( object instance, IDictionary<string, object> parameters )
    {
        var t = instance.GetType();

        foreach ( var attr in t.GetCustomAttributes<QueryPropertyAttribute>() )
        {
            if ( parameters.TryGetValue( attr.QueryId, out var value ) )
            {
                var prop = t.GetProperty( attr.Name );
                if ( prop is not null && prop.CanWrite && prop.PropertyType.IsAssignableFrom( value.GetType() ) )
                {
                    prop.SetValue( instance, value );
                }
            }
        }

        if ( instance is IQueryAttributable attributable )
            attributable.ApplyQueryAttributes( parameters );
    }
}

