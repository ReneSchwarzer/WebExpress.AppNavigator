using System.Collections.Generic;

namespace WebExpress.AppNavigator.Model
{
    public class API
    {
        /// <summary>
        /// Returns or sets the host name.
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Returns or sets the ip.
        /// </summary>
        public IEnumerable<string> HostAdresses { get; set; }

        /// <summary>
        /// Returns or sets the uri of the web server.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Returns or sets the computer name.
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// Returns or sets the number of processors.
        /// </summary>
        public int ProcessorCount { get; set; }

        /// <summary>
        /// Returns or sets the os version.
        /// </summary>
        public string OSVersion { get; set; }

        /// <summary>
        /// Returns or sets whether the operating system is 64-bit.
        /// </summary>
        public bool OS64Bit { get; set; }

        /// <summary>
        /// Returns or sets the operating system.
        /// </summary>
        public string OS { get; set; }

        /// <summary>
        /// Returns or sets the framework.
        /// </summary>
        public string Framework { get; set; }

        /// <summary>
        /// Returns or sets the current time.
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Returns or sets the version of the HttpServer.
        /// </summary>
        public string HttpServerVersion { get; set; }

        /// <summary>
        /// Returns or sets the enumeration of applications.
        /// </summary>
        public IEnumerable<GlobalApplication> Applications { get; set; } = new List<GlobalApplication>();
    }
}
