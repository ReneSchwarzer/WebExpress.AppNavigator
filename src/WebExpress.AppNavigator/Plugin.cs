using WebExpress.WebAttribute;
using WebExpress.WebPlugin;

namespace WebExpress.AppNavigator
{
    [Name("plugin.name")]
    [Description("plugin.description")]
    [Icon("/assets/img/appnavigator.svg")]
    public sealed class Plugin : IPlugin
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Plugin()
        {
        }

        /// <summary>
        /// Initialization of the plugin. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="context">Der Kontext, welcher für die Ausführung des Plugins gilt</param>
        public void Initialization(IPluginContext context)
        {

        }

        /// <summary>
        /// Invoked when the plugin starts working. The call to Run is concurrent.
        /// </summary>
        public void Run()
        {

        }

        /// <summary>
        /// Release unmanaged resources that have been reserved during use.
        /// </summary>
        public void Dispose()
        {

        }
    }
}