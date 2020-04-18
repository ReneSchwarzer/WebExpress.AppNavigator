using Agent.Model;
using Agent.Pages;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WebExpress.Pages;
using WebExpress.Workers;

namespace Agent
{
    public class AgentPlugin : WebExpress.Plugins.Plugin
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public AgentPlugin()
        : base("Agent", "/Asserts/img/Agent.svg")
        {
        }

        /// <summary>
        /// Initialisierung des Prozesszustandes. Hier können z.B. verwaltete Ressourcen geladen werden. 
        /// </summary>
        /// <param name="configFileName">Der Dateiname der Konfiguration oder null</param>
        public override void Init(string configFileName = null)
        {
            base.Init(configFileName);

            ViewModel.Instance.Context = Context;
            ViewModel.Instance.Init();
            Context.Log.Info(MethodBase.GetCurrentMethod(), "Agent Plugin initialisierung");

            var siteMap = new SiteMap(Context);

            siteMap.AddPage("Api", "", (x) => { return new WorkerPage<PageApiBase>(x); });
            siteMap.AddPath("Api");

            Register(siteMap);
            // Register(new WorkerFile(new Path(Context, "", "Assets/.*"), Context.AssetBaseFolder));

            Task.Run(() => { Run(); });
        }

        /// <summary>
        /// Diese Methode wird aufgerufen, nachdem das Fenster aktiv ist.
        /// </summary>
        private void Run()
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            // Loop
            while (true)
            {
                try
                {
                    Update();
                }
                finally
                {
                    Thread.Sleep(60000);
                }
            }
        }

        /// <summary>
        /// Diese Methode wird aufgerufen, nachdem das Fenster aktiv ist.
        /// </summary>
        private void Update()
        {
            ViewModel.Instance.Update();
        }
    }
}
