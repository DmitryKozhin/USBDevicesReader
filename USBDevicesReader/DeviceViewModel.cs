using RawHidReading;

namespace USBDevicesReader
{
    public class DeviceViewModel
    {
        private readonly HidDevice _hidDevice;

        public DeviceViewModel(HidDevice hidDevice)
        {
            if (hidDevice != null)
                _hidDevice = hidDevice;
        }

        public string Vendor => _hidDevice.Vendor;
        public string Product => _hidDevice.Product;
        public ushort VendorId => _hidDevice.VendorId;
        public ushort ProductId => _hidDevice.VendorId;
        public ushort Version => _hidDevice.Version;
        public string FriendlyName => _hidDevice.FriendlyName;
    }
}