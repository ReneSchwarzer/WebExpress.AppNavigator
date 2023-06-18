using System.Threading;
using WebExpress.AppNavigator.Model;
using WebExpress.WebAttribute;
using WebExpress.WebModule;

namespace WebExpress.AppNavigator
{
    [Application("*")]
    [Name("module.name")]
    [Description("module.description")]
    [Icon("/assets/img/appnavigator.svg")]
    [AssetPath("/")]
    [ContextPath("/wxappnavigator")]
    public sealed class Module : IModule
    {
        /// <summary>
        /// Determines whether the web server is running.
        /// </summary>
        private bool IsStarted { get; set; } = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public Module()
        {
        }

        /// <summary>
        /// Initialization of the module. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="context">The context that applies to the execution of the plugin.</param>
        public void Initialization(IModuleContext context)
        {
            ViewModel.MuduleContext = context;
            ViewModel.Initialization();

            context.PluginContext.Host.Host.Started += (s, e) =>
            {
                IsStarted = true;
            };
        }

        /// <summary>
        /// Invoked when the module starts working. The call is concurrent.
        /// </summary>
        public void Run()
        {
            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            // Loop
            while (true)
            {
                if (IsStarted)
                {
                    try
                    {
                        ViewModel.Update();
                    }
                    finally
                    {
                        Thread.Sleep(1000 * 60 * 10);
                    }
                }
                else
                {
                    Thread.Sleep(1000 * 10);
                }
            }
        }

        /// <summary>
        /// Release unmanaged resources that have been reserved during use.
        /// </summary>
        public void Dispose()
        {

        }
    }
}
