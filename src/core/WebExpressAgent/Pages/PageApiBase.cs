using Agent.Model;
using System;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json;
using WebExpress.Pages;
using WebExpressAgent.Model;

namespace Agent.Pages
{
    public class PageApiBase : PageApi
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
        public override void Init()
        {
            base.Init();


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
            var version = Context.HttpServerVersion;
            var plugins = WebExpress.HttpServer.Context.Plugins.Select
            (
                x => new Plugin()
                {
                    Name = x.Name,
                    BasisUrl = x.UrlBasePath,
                    IconUrl = x.IconUrl,
                    Version = x.Version
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
                Plugins = plugins,
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            Content = JsonSerializer.Serialize(api, options);
        }
    }
}
