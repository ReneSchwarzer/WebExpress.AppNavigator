using System.Xml.Serialization;

namespace WebExpress.AppNavigator.Model
{
    [XmlRoot(ElementName = "settings", IsNullable = false)]
    public class Settings
    {
        /// <summary>
        /// Liefert oder setzt die Url des Master
        /// </summary>
        [XmlElement("master")]
        public string Master { get; set; }

    }
}
