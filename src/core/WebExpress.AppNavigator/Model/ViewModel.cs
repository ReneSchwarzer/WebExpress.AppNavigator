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
using WebExpress.Internationalization;
using WebExpress.WebApplication;
using WebExpress.WebModule;
using WebExpress.WebPlugin;

namespace WebExpress.AppNavigator.Model
{
    public class ViewModel
    {
        /// <summary>
        /// Liefert die aktuelle Zeit
        /// </summary>
        public static string Now => DateTime.Now.ToString("dd.MM.yyyy<br>HH:mm:ss");

        /// <summary>
        /// Liefert oder setzt den Verweis auf den Kontext des Moduls
        /// </summary>
        public static IModuleContext Context { get; set; }

        /// <summary>
        /// Liefert die Programmversion
        /// </summary>
        [XmlIgnore]
        public static string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// Liefert oder setzt die global verfügbaren Anwendungen
        /// Key=, Value=
        /// </summary>
        public static IDictionary<string, GlobalApplication> ApplicationDictionary { get; } = new Dictionary<string, GlobalApplication>();

        /// <summary>
        /// Liefert oder setzt die Settings
        /// </summary>
        public static Settings Settings { get; private set; } = new Settings();

        /// <summary>
        /// Liefert den HttpClient für Rest-API-Abfragen
        /// </summary>
        private static HttpClient Client { get; } = new HttpClient();

        /// <summary>
        /// Konstruktor
        /// </summary>
        private ViewModel()
        {
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        public static void Initialization()
        {
            LoadSettings();
        }

        /// <summary>
        /// Updatefunktion
        /// </summary>
        public static void Update()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, Settings.Master);
            var options = new JsonSerializerOptions { WriteIndented = true };

            var hostName = Dns.GetHostName();
            var hostAdresses = Dns.GetHostAddresses(hostName).Select(x => x.ToString()).ToList();
            var hostUri = Context.Plugin.Host.Uri ?? Context.Plugin.Host.Endpoints.FirstOrDefault()?.Uri;
            var osVersion = Environment.OSVersion.ToString();
            var machineName = Environment.MachineName;
            var processorCount = Environment.ProcessorCount;
            var os64Bit = Environment.Is64BitOperatingSystem;
            var os = RuntimeInformation.OSDescription;
            var framework = RuntimeInformation.FrameworkDescription;
            var time = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss:ms");
            var version = PluginManager.Context.Version;
            var applications = ApplicationManager.GetApplcations()
                    .Where(x => !x.ApplicationID.StartsWith("webexpress", StringComparison.OrdinalIgnoreCase))
                    .Select(x => new LocalApplication
                    {
                        Name = InternationalizationManager.I18N(InternationalizationManager.DefaultCulture, x.Plugin.PluginId, x.ApplicationName),
                        Icon = Context.Plugin.Host.ContextPath.Append(x.Icon.ToString()),
                        ContextPath = x.ContextPath?.ToString(),
                        AssetPath = x.AssetPath
                    });

            var api = new API()
            {
                HostName = hostName,
                HostAdresses = hostAdresses,
                Uri = hostUri,
                OSVersion = osVersion,
                MachineName = machineName,
                ProcessorCount = processorCount,
                OS64Bit = os64Bit,
                OS = os,
                Framework = framework,
                Time = time,
                Applications = applications.Select(x => new GlobalApplication()
                {
                    Host = hostUri,
                    Name = x.Name,
                    Icon = x.Icon.ToString(),
                    AssetPath = x.AssetPath,
                    ContextPath = x.ContextPath,
                    Version = x.Version
                })
            };

            try
            {
                var json = JsonSerializer.Serialize(api, options);

                request.Content = new StringContent(json);
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json; charset=utf-8");

                var response = Client.Send(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var global = response.Content.ReadFromJsonAsync(typeof(API)).Result as API;

                    foreach (var application in global.Applications)
                    {
                        if (!ApplicationDictionary.ContainsKey(application.ToString().ToLower()))
                        {
                            ApplicationDictionary.Add(application.ToString().ToLower(), application);
                        }
                        else
                        {
                            //Applications.AddRange(global.Applications);
                            ApplicationDictionary[application.ToString().ToLower()].Timestamp = DateTime.Now;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Context.Log.Exception(ex);
                Context.Log.Error($"Master: {Settings.Master}");
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
        public static void LoadSettings()
        {
            // Konfiguration laden
            var serializer = new XmlSerializer(typeof(Settings));

            try
            {
                using var reader = File.OpenText(Path.Combine(Context.Plugin.Host.ConfigPath, "appnavigator.settings.xml"));
                Settings = serializer.Deserialize(reader) as Settings;
            }
            catch
            {
                Context.Log.Error("Datei mit den Einstellungen wurde nicht gefunden!");
            }
        }
    }
}