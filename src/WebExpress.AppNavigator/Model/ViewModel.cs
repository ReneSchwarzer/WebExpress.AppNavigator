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
using WebExpress.WebComponent;
using WebExpress.WebModule;

namespace WebExpress.AppNavigator.Model
{
    public class ViewModel
    {
        /// <summary>
        /// Returns the current time.
        /// </summary>
        public static string Now => DateTime.Now.ToString("dd.MM.yyyy<br>HH:mm:ss");

        /// <summary>
        /// Returns or sets the reference to the context of the module.
        /// </summary>
        public static IModuleContext MuduleContext { get; set; }

        /// <summary>
        /// Returns the program version.
        /// </summary>
        [XmlIgnore]
        public static string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// Returns the globally available applications.
        /// Key=, Value=
        /// </summary>
        public static IDictionary<string, GlobalApplication> ApplicationDictionary { get; } = new Dictionary<string, GlobalApplication>();

        /// <summary>
        /// Returns the settings.
        /// </summary>
        public static Settings Settings { get; private set; } = new Settings();

        /// <summary>
        /// Returns the HttpClient for rest api queries.
        /// </summary>
        private static HttpClient Client { get; } = new HttpClient();

        /// <summary>
        /// Constructor
        /// </summary>
        private ViewModel()
        {
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public static void Initialization()
        {
            LoadSettings();
        }

        /// <summary>
        /// Update function
        /// </summary>
        public static void Update()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, Settings.Master);
            var options = new JsonSerializerOptions { WriteIndented = true };

            var hostName = Dns.GetHostName();
            var hostAdresses = Dns.GetHostAddresses(hostName).Select(x => x.ToString()).ToList();
            var hostUri = MuduleContext.PluginContext.Host.Uri ?? MuduleContext.PluginContext.Host.Endpoints.FirstOrDefault()?.Uri;
            var osVersion = Environment.OSVersion.ToString();
            var machineName = Environment.MachineName;
            var processorCount = Environment.ProcessorCount;
            var os64Bit = Environment.Is64BitOperatingSystem;
            var os = RuntimeInformation.OSDescription;
            var framework = RuntimeInformation.FrameworkDescription;
            var time = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss:ms");
            var version = ComponentManager.PluginManager.HttpServerContext.Version;
            var applications = ComponentManager.ApplicationManager.Applications
                    .Where(x => !x.ApplicationId.StartsWith("webexpress", StringComparison.OrdinalIgnoreCase))
                    .Select(x => new LocalApplication
                    {
                        Name = InternationalizationManager.I18N(InternationalizationManager.DefaultCulture, x.PluginContext.PluginId, x.ApplicationName),
                        Icon = MuduleContext.PluginContext.Host.ContextPath.Append(x.Icon.ToString()),
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
                        lock (ApplicationDictionary)
                        {
                            if (!ApplicationDictionary.ContainsKey(application.ToString().ToLower()))
                            {
                                ApplicationDictionary.Add(application.ToString().ToLower(), application);
                            }
                            else
                            {
                                ApplicationDictionary[application.ToString().ToLower()].Timestamp = DateTime.Now;
                            }
                        }
                    }
                }
                else
                {
                    MuduleContext.PluginContext.Host.Log.Error($"Master: {Settings.Master} get {response.StatusCode}");
                }

            }
            catch (Exception ex)
            {
                MuduleContext.PluginContext.Host.Log.Exception(ex);
                MuduleContext.PluginContext.Host.Log.Error($"Master: {Settings.Master}");
            }

            // clean up old applications
            lock (ApplicationDictionary)
            {
                var toRemove = ApplicationDictionary.Values.Where(x => (DateTime.Now - x.Timestamp).TotalMinutes > 10).ToList();
                foreach (var v in toRemove)
                {
                    ApplicationDictionary.Remove(v.ToString().ToLower());
                }
            }
        }

        /// <summary>
        /// Invoked when the settings are to be loaded.
        /// </summary>
        public static void LoadSettings()
        {
            // Konfiguration laden
            var serializer = new XmlSerializer(typeof(Settings));

            try
            {
                using var reader = File.OpenText(Path.Combine(MuduleContext.PluginContext.Host.ConfigPath, "appnavigator.settings.xml"));
                Settings = serializer.Deserialize(reader) as Settings;
            }
            catch
            {
                MuduleContext.PluginContext.Host.Log.Error("File with the settings was not found!");
            }
        }
    }
}