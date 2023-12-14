
namespace WpfApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public MainWindowViewModel( INavigationService navigationService )
        {
            _navigationService = navigationService;
        }

        protected override async Task OnInitializeAsync( CancellationToken cancellationToken )
        {
            await base.OnInitializeAsync( cancellationToken );
            await _navigationService.GotoAsync( "queryPage" );
        }
    }
}