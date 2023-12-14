namespace WpfApp.ViewModels
{
    public class QueryPageViewModel : ViewModelBase, IQueryAttributable
    {
        private readonly INavigationService _navigationService;
        private string? _queryData;

        [Reactive] public string Query { get; set; } = string.Empty;
        public ReactiveCommand<Unit, Unit> QueryCommand { get; }
        [Reactive] public ICollection<string>? QueryResult { get; private set; }

        public QueryPageViewModel( INavigationService navigationService )
        {
            var canQuery = this.WhenAnyValue( e => e.Query, ( query ) => !string.IsNullOrEmpty( query ) );
            QueryCommand = ReactiveCommand.CreateFromTask( OnQueryAsync, canQuery );
            _navigationService = navigationService;
        }

        private async Task OnQueryAsync( CancellationToken cancellationToken )
        {
            await _navigationService.GotoAsync( "queryPage", new Dictionary<string, object> { { "queryParameter", Query } } );
        }

        private async Task PerformQueryAsync( CancellationToken cancellationToken )
        {
            QueryResult = null;
            await Task.Delay( 250 );
            if ( _queryData is not null )
                QueryResult = Enumerable.Repeat( _queryData, 20 ).ToImmutableList();
        }

        protected override async Task OnInitializeAsync( CancellationToken cancellationToken )
        {
            await base.OnInitializeAsync( cancellationToken );
            if ( !IsInitialized )
                await PerformQueryAsync( cancellationToken );
        }

        public void ApplyQueryAttributes( IDictionary<string, object> query )
        {
            if ( query.TryGetValue( "queryParameter", out var data ) && data is string stringdata ) _queryData = stringdata;
        }
    }
}