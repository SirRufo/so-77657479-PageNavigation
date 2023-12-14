namespace WpfApp.Views
{
    /// <summary>
    /// Interaktionslogik für QueryPage.xaml
    /// </summary>
    public partial class QueryPage : Page
    {
        public QueryPage( QueryPageViewModel viewModel )
        {
            InitializeComponent();
            DataContext = viewModel;
            Loaded += async ( s, e ) =>
            {
                Query_TextBox.Focus();
                await viewModel.InitializeAsync();
            };
        }
    }
}
