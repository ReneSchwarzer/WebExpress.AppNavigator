using System;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json;
using WebExpress.Agent.Model;
using WebExpress.Attribute;
using WebExpress.Plugin;

namespace WebExpress.Agent.WebResource
{
    [ID("API")]
    [Segment("")]
    [Path("")]
    [Module("WebExpress.Agent")]
    public sealed class ResourceApi : WebExpress.WebResource.ResourceApi
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public ResourceApi()
            : base()
        {
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        public override void Initialization()
        {
            base.Initialization();
        }

        /// <summary>
        /// Verarbeitung
        /// </summary>
        public override void Process()
        {
            base.Process();

            var plugin = PluginManager.GetPlugin(Context.PluginID);

            // Anfrage 
            if (Request.Content != null)
            {
                var client = JsonSerializer.Deserialize(Request.Content, typeof(API)) as API;

                foreach (var application in client.Applications)
                {
                    if (!ViewModel.Instance.ApplicationDictionary.ContainsKey(application.ToString()))
                    {
                        ViewModel.Instance.ApplicationDictionary.Add(application.ToString(), new GlobalApplication()
                        {
                            Host = application.Host,
                            Name = application.Name,
                            Icon = application.Icon,
                            ContextPath = application.ContextPath,
                            AssetPath = application.AssetPath,
                            Version = application.Version,
                            Timestamp = DateTime.Now
                        });
                    }
                    else
                    {
                        ViewModel.Instance.ApplicationDictionary[application.ToString()].Timestamp = DateTime.Now;
                    }
                }
            }

            var hostName = Dns.GetHostName();
            var hostAdresses = Dns.GetHostAddresses(hostName).Select(x => x.ToString()).ToList();
            var hostPort = plugin.Host.Port;
            var osVersion = Environment.OSVersion.ToString();
            var machineName = Environment.MachineName;
            var processorCount = Environment.ProcessorCount;
            var os64Bit = Environment.Is64BitOperatingSystem;
            var os = RuntimeInformation.OSDescription;
            var framework = RuntimeInformation.FrameworkDescription;
            var time = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss:ms");
            var version = PluginManager.Context.Version;
            var applications = ViewModel.Instance.ApplicationDictionary.Values;

            var api = new API()
            {
                HostName = hostName,
                HostAdresses = hostAdresses,
                OSVersion = osVersion,
                MachineName = machineName,
                ProcessorCount = processorCount,
                OS64Bit = os64Bit,
                OS = os,
                Framework = framework,
                Time = time,
                Applications = applications
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            Content = JsonSerializer.Serialize(api, options);
        }
    }
}
