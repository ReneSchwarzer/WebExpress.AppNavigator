using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using WebExpress.Agent.Model;
using WebExpress.Application;
using WebExpress.Plugin;
using WebExpress.WebResource;

namespace WebExpress.Agent.WebResource
{
    public sealed class PageApiBase : ResourceApi
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageApiBase()
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

            //var converter = new TimeSpanConverter();

            var hostName = Dns.GetHostName();
            var hostAdresses = Dns.GetHostAddresses(hostName).Select(x => x.ToString()).ToList();
            var osVersion = Environment.OSVersion.ToString();
            var machineName = Environment.MachineName;
            var processorCount = Environment.ProcessorCount;
            var os64Bit = Environment.Is64BitOperatingSystem;
            var os = RuntimeInformation.OSDescription;
            var framework = RuntimeInformation.FrameworkDescription;
            var time = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss:ms");
            var version = PluginManager.Context.Version;
            var applications = ApplicationManager.GetApplcations().Select
            (
                x => new Model.Application()
                {
                    Name = x.ApplicationName,
                    ContextPath = x.ContextPath?.ToString(),
                    Icon = x.Icon?.ToString(),
                    Version = x.Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion,
                    AssetPath = x.AssetPath
                }
            );

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
                Applications = applications,
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            Content = JsonSerializer.Serialize(api, options);
        }
    }
}
