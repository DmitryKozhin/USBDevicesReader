using System;
using System.Collections.ObjectModel;

namespace USBDevicesReader.Tools
{
    /// <summary>
    /// Class for search device
    /// </summary>
    public class Searcher
    {
        /// <summary>
        /// Initialize lazy field
        /// </summary>
        private static readonly Lazy<Searcher> Lazy =
            new Lazy<Searcher>(() => new Searcher());

        /// <summary>
        /// Gets the lazily initialized value of the current System.Lazy`1 instance
        /// </summary>
        public static Searcher Source => Lazy.Value;

        /// <summary>
        /// Search in input collection
        /// </summary>
        /// <param name="value">Searching element</param>
        /// <param name="collection">Input collection</param>
        /// <returns>Items found or input collection</returns>
        public ObservableCollection<DeviceViewModel> SearchBy(string value, ObservableCollection<DeviceViewModel> collection)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                return collection;

            var searchResult = new ObservableCollection<DeviceViewModel>();

            foreach (var deviceViewModel in collection)
            {
                if (deviceViewModel.Vendor.Contains(value)
                    || deviceViewModel.FriendlyName.Contains(value)
                    || deviceViewModel.Product.Contains(value)
                    || deviceViewModel.ProductId.ToString().Contains(value)
                    || deviceViewModel.VendorId.ToString().Contains(value)
                    || deviceViewModel.Version.ToString().Contains(value))
                {
                    searchResult.Add(deviceViewModel);
                }
            }

            return searchResult;
        }
    }
}