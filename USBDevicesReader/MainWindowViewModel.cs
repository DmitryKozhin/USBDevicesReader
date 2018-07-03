using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Management;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Expression.Interactivity.Core;
using RawHidReading;
using USBDevicesReader.Tools;

namespace USBDevicesReader
{
    /// <summary>
    /// Main ViewModel class
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Fields

        private ObservableCollection<DeviceViewModel> _hidDeviceCollection;
        private DeviceViewModel _selectedHidDevice;
        private string _searchString;

        private readonly ManagementEventWatcher _updateEventWatcher;
        private readonly HidDevice[] _devices;
        private readonly HID _hid;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a property value changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="window">Application window</param>
        public MainWindowViewModel(Window window)
        {        
            KeyUpCommand = new ActionCommand(KeyUpExecute);

            try
            {
                _hid = new HID(HID.GetWindowHandle(window.Owner));
                _devices = _hid.ListDevices();

                HidDeviceCollection = new ObservableCollection<DeviceViewModel>
                    (GetDeviceViewModels(_devices));

                //turn on USB device event
                WqlEventQuery qCreation = new WqlEventQuery
                {
                    EventClassName = "__InstanceOperationEvent",
                    WithinInterval = new TimeSpan(0, 0, 2),
                    Condition = @"TargetInstance ISA 'Win32_USBControllerdevice'"
                };
                _updateEventWatcher = new ManagementEventWatcher(qCreation);
                _updateEventWatcher.EventArrived += USBEventArrived_Update;
                _updateEventWatcher.Start();
            }
            catch (Exception e)
            {              
                Logger.Source.Log.Fatal("HID device update error", e);
                MessageBox.Show(e.Message, "Fatal error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }        
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~MainWindowViewModel()
        {
            _updateEventWatcher?.Stop();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Pressing to button
        /// </summary>
        public ICommand KeyUpCommand { get; }

        /// <summary>
        /// Displayed сollection of <see cref="DeviceViewModel"/>/>
        /// </summary>
        public ObservableCollection<DeviceViewModel> HidDeviceCollection
        {
            get => _hidDeviceCollection;

            set
            {
                _hidDeviceCollection = value;

                OnPropertyChanged(nameof(HidDeviceCollection));
            }
        }

        /// <summary>
        /// Selected device in <see cref="HidDeviceCollection"/>
        /// </summary>
        public DeviceViewModel SelectedHidDevice
        {
            get => _selectedHidDevice;

            set
            {
                _selectedHidDevice = value;

                OnPropertyChanged(nameof(SelectedHidDevice));
            }
        }

        /// <summary>
        /// Searching element
        /// </summary>
        public string SearchString
        {
            get => _searchString;

            set
            {
                _searchString = value;

                OnPropertyChanged(nameof(SearchString));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Key press execute
        /// </summary>
        /// <param name="obj">Entered text</param>
        private void KeyUpExecute(object obj)
        {
            HidDeviceCollection = Searcher.Source.SearchBy
                ((obj as TextBox)?.Text, GetDeviceViewModels(_devices));
        }

        /// <summary>
        /// Get <see cref="DeviceViewModel"/> collection
        /// </summary>
        /// <param name="inputDevices">Connected devices</param>
        /// <returns></returns>
        private ObservableCollection<DeviceViewModel> GetDeviceViewModels(IEnumerable<HidDevice> inputDevices)
        {
            var deviceCollection = new ObservableCollection<DeviceViewModel>();
            foreach (var device in inputDevices)
            {
                deviceCollection.Add(new DeviceViewModel(device));
            }

            return deviceCollection;
        }

        /// <summary>
        /// Subscription method for <see cref="PropertyChanged"/>
        /// </summary>
        /// <param name="propertyName">Name of the caller property</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Handling an event adding or removing a USB device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void USBEventArrived_Update(object sender, EventArrivedEventArgs e)
        {
            HidDeviceCollection = new ObservableCollection<DeviceViewModel>
                (GetDeviceViewModels(_hid.ListDevices()));

            SearchString = string.Empty;
        }

        #endregion
    }
}