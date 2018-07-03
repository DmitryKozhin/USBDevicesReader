using System.Windows;
using USBDevicesReader.Tools;

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
            Logger.Source.InitLogger();
            Logger.Source.Log.Info("Application started");

            Window window = new Window();
            this.Show();
            window.Owner = this;

            DataContext = new MainWindowViewModel(window);
        }
    }
}
