using System.Xml.Serialization;

namespace WebExpress.AppNavigator.Model
{
    [XmlRoot(ElementName = "settings", IsNullable = false)]
    public class Settings
    {
        /// <summary>
        /// Returns or sets the uri of the master.
        /// </summary>
        [XmlElement("master")]
        public string Master { get; set; }

    }
}
