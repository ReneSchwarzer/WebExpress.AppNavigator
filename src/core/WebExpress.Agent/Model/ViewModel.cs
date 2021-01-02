using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using WebExpress.Application;
using WebExpress.Internationalization;
using WebExpress.Plugin;

namespace WebExpress.Agent.Model
{
    public class ViewModel
    {
        /// <summary>
        /// Instanz des einzigen Modells
        /// </summary>
        private static ViewModel _this = null;

        /// <summary>
        /// Lifert die einzige Instanz der Modell-Klasse
        /// </summary>
        public static ViewModel Instance
        {
            get
            {
                if (_this == null)
                {
                    _this = new ViewModel();
                }

                return _this;
            }
        }

        /// <summary>
        /// Liefert die aktuelle Zeit
        /// </summary>
        public string Now => DateTime.Now.ToString("dd.MM.yyyy<br>HH:mm:ss");

        /// <summary>
        /// Liefert oder setzt den Verweis auf den Kontext des Plugins
        /// </summary>
        public IPluginContext Context { get; set; }

        /// <summary>
        /// Liefert die Programmversion
        /// </summary>
        [XmlIgnore]
        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// Liefert oder setzt die Anwendungen
        /// </summary>
        public ICollection<Application> Applications { get; } = new List<Application>();

        /// <summary>
        /// Konstruktor
        /// </summary>
        private ViewModel()
        {
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        public void Initialization()
        {
            foreach (var v in ApplicationManager.GetApplcations().Where(x => !x.ApplicationID.StartsWith("webexpress", System.StringComparison.OrdinalIgnoreCase)))
            {
                Applications.Add(new Application
                {
                    Name = InternationalizationManager.I18N(InternationalizationManager.DefaultCulture, v.PluginID, v.ApplicationName),
                    Icon = v.Icon.ToString(),
                    ContextPath = v.ContextPath?.ToString(),
                    AssetPath = v.AssetPath
                });
            }
        }

        /// <summary>
        /// Updatefunktion
        /// </summary>
        public virtual void Update()
        {
            
        }
    }
}