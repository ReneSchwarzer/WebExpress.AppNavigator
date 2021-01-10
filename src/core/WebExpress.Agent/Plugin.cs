using System.Threading;
using WebExpress.Agent.Model;
using WebExpress.Attribute;
using WebExpress.Plugin;

namespace WebExpress.Agent
{
    [ID("WebExpress.Agent")]
    [Name("plugin.name")]
    [Description("plugin.description")]
    [Icon("/assets/img/Agent.png")]
    public sealed class Plugin : IPlugin
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public Plugin()
        {
        }

        /// <summary>
        /// Initialisierung des Plugins. Hier können z.B. verwaltete Ressourcen geladen werden. 
        /// </summary>
        /// <param name="context">Der Kontext, welcher für die Ausführung des Plugins gilt</param>
        public void Initialization(IPluginContext context)
        {
            ViewModel.Instance.Context = context;
            ViewModel.Instance.Initialization();
        }

        /// <summary>
        /// Wird aufgerufen, wenn das Plugin mit der Arbeit beginnt. Der Aufruf von Run erfolgt nebenläufig.
        /// </summary>
        public void Run()
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            // Loop
            while (true)
            {
                try
                {
                    ViewModel.Instance.Update();
                }
                finally
                {
                    Thread.Sleep(60000);
                }
            }
        }

        /// <summary>
        /// Freigeben von nicht verwalteten Ressourcen, welche wärend der Verwendung reserviert wurden.
        /// </summary>
        public void Dispose()
        {

        }
    }
}