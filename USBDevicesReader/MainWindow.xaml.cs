using System.Windows;

namespace USBDevicesReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Window window = new Window();
            this.Show();
            window.Owner = this;

            DataContext = new MainWindowViewModel(window);
        }
    }
}
