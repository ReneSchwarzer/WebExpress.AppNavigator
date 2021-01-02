namespace WebExpress.Agent.Model
{
    public class Application
    {
        /// <summary>
        /// Liefert oder setzt den Namen
        /// </summary>
        public string Name { get; set; }

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
    }
}
