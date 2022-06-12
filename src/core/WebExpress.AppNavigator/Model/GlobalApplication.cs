using System;

namespace WebExpress.AppNavigator.Model
{
    public class GlobalApplication
    {
        /// <summary>
        /// Liefert oder setzt den Namen
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Liefert oder setzt die Host-URL
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Liefert oder setzt die BasisUrl
        /// </summary>
        public string ContextPath { get; set; }

        /// <summary>
        /// Liefert oder setzt die Icon
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Liefert oder setzt die Version des Plugins
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Liefert oder setzt den Asset-Pfad
        /// </summary>
        public string AssetPath { get; set; }

        /// <summary>
        /// Liefert oder setzt den Zeitstempel des letzten Zugriffs
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Umwandlung des Objektes in seine Stringform
        /// </summary>
        /// <returns>Das Objekt in Stringform</returns>
        public override string ToString()
        {
            return $"{Host}{ContextPath}";
        }
    }
}
