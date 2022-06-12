﻿using System.Threading;
using WebExpress.AppNavigator.Model;
using WebExpress.WebAttribute;
using WebExpress.WebModule;

namespace WebExpress.AppNavigator
{
    [Id("WebExpress.AppNavigator")]
    [Application("*")]
    [Name("module.name")]
    [Description("module.description")]
    [Icon("/assets/img/appnavigator.svg")]
    [AssetPath("/")]
    [ContextPath("/wxappnavigator")]
    public sealed class Module : IModule
    {
        /// <summary>
        /// BEstimmt, ob der Webserver läuft
        /// </summary>
        private bool IsStarted { get; set; } = false;

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Module()
        {
        }

        /// <summary>
        /// Initialisierung des Moduls. Hier können z.B. verwaltete Ressourcen geladen werden. 
        /// </summary>
        /// <param name="context">Der Kontext, welcher für die Ausführung des Plugins gilt</param>
        public void Initialization(IModuleContext context)
        {
            ViewModel.Context = context;
            ViewModel.Initialization();

            context.Plugin.Host.Host.Started += (s, e) =>
            {
                IsStarted = true;
            };
        }

        /// <summary>
        /// Wird aufgerufen, wenn das Modul mit der Arbeit beginnt. Der Aufruf erfolgt nebenläufig.
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
        /// Freigeben von nicht verwalteten Ressourcen, welche wärend der Verwendung reserviert wurden.
        /// </summary>
        public void Dispose()
        {

        }
    }
}
