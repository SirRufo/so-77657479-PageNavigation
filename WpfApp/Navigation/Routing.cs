namespace WpfApp.Navigation;

public static class Routing
{
    private static readonly Dictionary<string, Type> _registeredRoutes = new Dictionary<string, Type>();

    public static FrameworkElement GetOrCreateContent( string route, IServiceProvider serviceProvider )
    {
        if ( _registeredRoutes.TryGetValue( route, out var type ) )
        {
            var element = serviceProvider.GetService( type ) as FrameworkElement;
            if ( element is not null ) return element;
        }
        throw new InvalidOperationException( "" );
    }

    public static void RegisterRoute( string route, Type type )
    {
        if ( string.IsNullOrEmpty( route ) ) throw new ArgumentException( nameof( route ) );
        if ( !type.IsAssignableTo( typeof( FrameworkElement ) ) ) throw new ArgumentException( nameof( type ) );

        _registeredRoutes.Add( route, type );
    }
}