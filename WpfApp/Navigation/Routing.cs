namespace WpfApp.Navigation;

public static class Routing
{
    public static string GetRoute( FrameworkElement obj )
    {
        return (string)obj.GetValue( RouteProperty );
    }

    public static void SetRoute( FrameworkElement obj, string value )
    {
        obj.SetValue( RouteProperty, value );
    }

    // Using a DependencyProperty as the backing store for Route.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty RouteProperty =
        DependencyProperty.RegisterAttached( "Route", typeof( string ), typeof( Routing ), new PropertyMetadata( string.Empty ) );

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