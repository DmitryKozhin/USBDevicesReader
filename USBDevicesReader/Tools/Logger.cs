using System;
using log4net;
using log4net.Config;

namespace USBDevicesReader.Tools
{
    public class Logger
    {
        /// <summary>
        /// Initialize lazy field
        /// </summary>
        private static readonly Lazy<Logger> Lazy =
            new Lazy<Logger>(() => new Logger());

        /// <summary>
        /// Gets the lazily initialized value of the current System.Lazy`1 instance
        /// </summary>
        public static Logger Source => Lazy.Value;

        public ILog Log { get; } = LogManager.GetLogger("LOGGER");

        public void InitLogger()
        {
            XmlConfigurator.Configure();
        }
    }
}