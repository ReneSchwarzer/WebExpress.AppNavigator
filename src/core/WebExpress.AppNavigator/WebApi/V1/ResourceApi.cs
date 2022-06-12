using System;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json;
using WebExpress.AppNavigator.Model;
using WebExpress.Message;
using WebExpress.WebAttribute;
using WebExpress.WebPlugin;
using WebExpress.WebResource;

namespace WebExpress.AppNavigator.WebApi.V1
{
    [Id("RestAPI1")]
    [Segment("applications")]
    [Path("1")]
    [Module("WebExpress.AppNavigator")]
    public sealed class ResourceApi : ResourceRest
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
        /// <param name="context">Der Kontext</param>
        public override void Initialization(IResourceContext context)
        {
            base.Initialization(context);
        }

        /// <summary>
        /// Verarbeitung des GET-Request
        /// </summary>
        /// <param name="request">Die Anfrage</param>
        /// <returns>Eine Aufzählung, welche JsonSerializer serialisiert werden kann.</returns>
        public override object GetData(Request request)
        {
            var plugin = PluginManager.GetPlugin(Context.Plugin.PluginId);

            // Anfrage 
            if (request.Content != null)
            {
                var client = JsonSerializer.Deserialize(request.Content, typeof(API)) as API;

                foreach (var application in client.Applications)
                {
                    if (!ViewModel.ApplicationDictionary.ContainsKey(application.ToString().ToLower()))
                    {
                        ViewModel.ApplicationDictionary.Add(application.ToString().ToLower(), new GlobalApplication()
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
                        ViewModel.ApplicationDictionary[application.ToString().ToLower()].Timestamp = DateTime.Now;
                    }
                }
            }

            var hostName = Dns.GetHostName();
            var hostAdresses = Dns.GetHostAddresses(hostName).Select(x => x.ToString()).ToList();
            //var hostPort = plugin.Host.Port;
            var osVersion = Environment.OSVersion.ToString();
            var machineName = Environment.MachineName;
            var processorCount = Environment.ProcessorCount;
            var os64Bit = Environment.Is64BitOperatingSystem;
            var os = RuntimeInformation.OSDescription;
            var framework = RuntimeInformation.FrameworkDescription;
            var time = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss:ms");
            var version = PluginManager.Context.Version;
            var applications = ViewModel.ApplicationDictionary.Values;

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

            return api;
        }
    }
}
