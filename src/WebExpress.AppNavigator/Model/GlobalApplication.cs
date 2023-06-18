using System;

namespace WebExpress.AppNavigator.Model
{
    public class GlobalApplication
    {
        /// <summary>
        /// Returns or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Returns or sets the host uri.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Returns or sets the base uri.
        /// </summary>
        public string ContextPath { get; set; }

        /// <summary>
        /// Returns or sets the icon.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Returns or sets the version of the plugin.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Returns or sets the asset path.
        /// </summary>
        public string AssetPath { get; set; }

        /// <summary>
        /// Returns or sets the timestamp of the last access.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Conversion of the object into its string form.
        /// </summary>
        /// <returns>The object as a string.</returns>
        public override string ToString()
        {
            return $"{Host}{ContextPath}";
        }
    }
}
