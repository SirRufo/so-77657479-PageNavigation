namespace WpfApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow( MainWindowViewModel viewModel )
        {
            InitializeComponent();
            DataContext = viewModel;
            Loaded += async ( s, e ) => await viewModel.InitializeAsync();
        }
    }
}