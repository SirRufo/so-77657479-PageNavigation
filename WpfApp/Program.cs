// Create a builder by specifying the application and main window.
using WpfApp;

var builder = WpfApplication<App, MainWindow>.CreateBuilder( args );

builder.Services
    .AddSingleton<INavigationService, WpfNavigationService>()

    .AddTransient<MainWindowViewModel>()
    .AddTransient<QueryPage>().AddTransient<QueryPageViewModel>();

Routing.RegisterRoute( "queryPage", typeof( QueryPage ) );

var app = builder.Build();

await app.RunAsync();