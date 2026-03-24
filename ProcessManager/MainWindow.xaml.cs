using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
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
            _vm = new MainViewModel(new ProcessService()); // ProcessService, не IProcessService
            DataContext = _vm;
            CreateCpuCoresUI();
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

        // 
        private async void Kill_Click(object sender, RoutedEventArgs e)
        {
            _vm.KillSelected();
            await _vm.RefreshAsync(); // важно
        }

        // 
        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            await _vm.RefreshAsync();
        }

        // 
        private void SetPriority_Click(object sender, RoutedEventArgs e)
        {
            if (PriorityBox.SelectedItem == null)
                return;

            var value = ((ComboBoxItem)PriorityBox.SelectedItem)
                        .Content.ToString();

            var priority = (ProcessPriorityClass)
                Enum.Parse(typeof(ProcessPriorityClass), value);

            _vm.ChangePriority(priority);
        }

        // 
        private void CreateCpuCoresUI()
        {
            int coreCount = Environment.ProcessorCount;

            for (int i = 0; i < coreCount; i++)
            {
                var cb = new CheckBox
                {
                    Content = $"Core {i}",
                    Margin = new Thickness(5),
                   
                };

                CoresPanel.Children.Add(cb);
            }
        }

        // 
        private void SetAffinity_Click(object sender, RoutedEventArgs e)
        {
            int count = CoresPanel.Children.Count;
            bool[] cores = new bool[count];

            for (int i = 0; i < count; i++)
            {
                var cb = (CheckBox)CoresPanel.Children[i];
                cores[i] = cb.IsChecked == true;
            }

            _vm.SetAffinity(cores);
        }
    }
}