using System;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json;
using WebExpress.AppNavigator.Model;
using WebExpress.WebAttribute;
using WebExpress.WebComponent;
using WebExpress.WebMessage;
using WebExpress.WebResource;

namespace WebExpress.AppNavigator.WebApi.V1
{
    [Segment("applications")]
    [ContextPath("1")]
    [Module<Module>]
    public sealed class ResourceApi : ResourceRest
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ResourceApi()
            : base()
        {
        }

        /// <summary>
        /// The initialization.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Initialization(IResourceContext context)
        {
            base.Initialization(context);
        }

        /// <summary>
        /// Processing of the get request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>An enumeration that can be serialized by JsonSerializer.</returns>
        public override object GetData(Request request)
        {
            var plugin = ComponentManager.PluginManager.GetPlugin(ResourceContext.PluginContext.PluginId);

            // request 
            if (request.Content != null)
            {
                var client = JsonSerializer.Deserialize(request.Content, typeof(API)) as API;

                foreach (var application in client.Applications)
                {
                    lock (ViewModel.ApplicationDictionary)
                    {
                        var key = application.ToString().ToLower();
                        if (!ViewModel.ApplicationDictionary.ContainsKey(key))
                        {
                            ViewModel.ApplicationDictionary.Add(key, new GlobalApplication()
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
                            ViewModel.ApplicationDictionary[key].Timestamp = DateTime.Now;
                        }
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
            var version = ComponentManager.PluginManager.HttpServerContext.Version;
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
