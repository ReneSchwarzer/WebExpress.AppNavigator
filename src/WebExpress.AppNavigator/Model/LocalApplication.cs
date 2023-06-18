using WebExpress.WebUri;

namespace WebExpress.AppNavigator.Model
{
    public class LocalApplication
    {
        /// <summary>
        /// Returns or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Returns or sets the base uri.
        /// </summary>
        public string ContextPath { get; set; }

        /// <summary>
        /// Returns or sets the icon.
        /// </summary>
        public UriResource Icon { get; set; }

        /// <summary>
        /// Returns or sets the version of the plugin.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Returns or sets the asset path.
        /// </summary>
        public string AssetPath { get; set; }
    }
}
