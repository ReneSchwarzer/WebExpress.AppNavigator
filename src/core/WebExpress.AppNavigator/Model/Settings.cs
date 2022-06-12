using System.Xml.Serialization;

namespace WebExpress.Agent.Model
{
    [XmlRoot(ElementName = "settings", IsNullable = false)]
    public class Settings
    {
        /// <summary>
        /// Liefert oder setzt die Url des Agenten
        /// </summary>
        [XmlElement("agent")]
        public string Agent { get; set; }

    }
}
