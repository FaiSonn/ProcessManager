using System.Threading.Tasks;
using System.Windows;
using ProcessM.Core.Services;
using ProcessM.Core.ViewModels;

namespace ProcessManager
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();

            _vm = new MainViewModel(new ProcessService());

            DataContext = _vm;

            StartLoop();
        }

        private async void StartLoop()
        {
            while (true)
            {
                await _vm.RefreshAsync();

                await Task.Delay(2000);
            }
        }

        private void Kill_Click(object sender, RoutedEventArgs e)
        {
            _vm.KillSelected();
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            await _vm.RefreshAsync();
        }
    }
}