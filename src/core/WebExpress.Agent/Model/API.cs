using System.Collections.Generic;

namespace WebExpress.Agent.Model
{
    public class API
    {
        /// <summary>
        /// Liefert oder setzt den Hostnamen
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Liefert oder setzt die IP
        /// </summary>
        public IEnumerable<string> HostAdresses { get; set; }

        /// <summary>
        /// Liefert oder setzt die Uri des Webservers
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Liefert oder setzt den Computernamen
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// Liefert oder setzt die Anzahl der Prozessoren
        /// </summary>
        public int ProcessorCount { get; set; }

        /// <summary>
        /// Liefert oder setzt die OSVersion
        /// </summary>
        public string OSVersion { get; set; }

        /// <summary>
        /// Liefert oder setzt ob das Betriebssystem 64-Bit ist
        /// </summary>
        public bool OS64Bit { get; set; }

        /// <summary>
        /// Liefert oder setzt das Betriebssystem
        /// </summary>
        public string OS { get; set; }

        /// <summary>
        /// Liefert oder setzt das Framework
        /// </summary>
        public string Framework { get; set; }

        /// <summary>
        /// Liefert oder setzt die aktuelle Zeit
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Liefert oder setzt die Version des HttpServers
        /// </summary>
        public string HttpServerVersion { get; set; }

        /// <summary>
        /// Liefert oder setzt die Anwendungen
        /// </summary>
        public IEnumerable<GlobalApplication> Applications { get; set; } = new List<GlobalApplication>();
    }
}
