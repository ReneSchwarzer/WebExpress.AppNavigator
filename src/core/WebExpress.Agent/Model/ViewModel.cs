using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
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
        /// Liefert oder setzt die lokal verfügbaren Anwendungen
        /// </summary>
        public List<LocalApplication> LocalApplications { get; } = new List<LocalApplication>();

        /// <summary>
        /// Liefert oder setzt die global verfügbaren Anwendungen
        /// </summary>
        public List<GlobalApplication> GlobalApplications { get; } = new List<GlobalApplication>();

        /// <summary>
        /// Liefert oder setzt die global verfügbaren Anwendungen
        /// </summary>
        public IDictionary<string, GlobalApplication> ApplicationDictionary { get; } = new Dictionary<string, GlobalApplication>();

        /// <summary>
        /// Liefert oder setzt die Settings
        /// </summary>
        public Settings Settings { get; private set; } = new Settings();

        /// <summary>
        /// Liefert den HttpClient für Rest-API-Abfragen
        /// </summary>
        private HttpClient Client { get; } = new HttpClient();

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
            LoadSettings();

            foreach (var v in ApplicationManager.GetApplcations().Where(x => !x.ApplicationID.StartsWith("webexpress", System.StringComparison.OrdinalIgnoreCase)))
            {
                LocalApplications.Add(new LocalApplication
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
            var request = new HttpRequestMessage(HttpMethod.Post, Settings.Agent);
            var options = new JsonSerializerOptions { WriteIndented = true };

            var hostName = Dns.GetHostName();
            var hostAdresses = Dns.GetHostAddresses(hostName).Select(x => x.ToString()).ToList();
            var hostPort = Context.Host.Port;
            var osVersion = Environment.OSVersion.ToString();
            var machineName = Environment.MachineName;
            var processorCount = Environment.ProcessorCount;
            var os64Bit = Environment.Is64BitOperatingSystem;
            var os = RuntimeInformation.OSDescription;
            var framework = RuntimeInformation.FrameworkDescription;
            var time = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss:ms");
            var version = PluginManager.Context.Version;
            var applications = ViewModel.Instance.LocalApplications;

            var api = new API()
            {
                HostName = hostName,
                HostAdresses = hostAdresses,
                HostPort = hostPort,
                OSVersion = osVersion,
                MachineName = machineName,
                ProcessorCount = processorCount,
                OS64Bit = os64Bit,
                OS = os,
                Framework = framework,
                Time = time,
                Applications = applications.Select(x => new GlobalApplication()
                {
                    Host = "",
                    Name = x.Name,
                    Icon = x.Icon,
                    AssetPath = x.AssetPath,
                    ContextPath = x.ContextPath,
                    Version = x.Version
                })
            };

            var json = JsonSerializer.Serialize(api, options);

            request.Content = new StringContent(json);
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json; charset=utf-8");

            var response = Client.Send(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var global = response.Content.ReadFromJsonAsync(typeof(API)).Result as API;

                GlobalApplications.Clear();
                GlobalApplications.AddRange(global.Applications);
            }

            // Bereinige alte Anwendungen
            var toRemove = ApplicationDictionary.Values.Where(x => (DateTime.Now - x.Timestamp).TotalMinutes > 10).ToList();
            foreach (var v in toRemove)
            {
                ApplicationDictionary.Remove(v.ToString());
            }
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Einstellungen geladen werden sollen
        /// </summary>
        public void LoadSettings()
        {
            // Konfiguration laden
            var serializer = new XmlSerializer(typeof(Settings));

            try
            {
                using var reader = File.OpenText(Path.Combine(Context.Host.ConfigPath, "agent.settings.xml"));
                Settings = serializer.Deserialize(reader) as Settings;
            }
            catch
            {
                Context.Log.Error("Datei mit den Einstellungen wurde nicht gefunden!");
            }
        }
    }
}