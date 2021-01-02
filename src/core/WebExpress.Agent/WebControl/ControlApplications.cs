using System.Collections.Generic;
using System.Linq;
using WebExpress.Agent.Model;
using WebExpress.Application;
using WebExpress.Attribute;
using WebExpress.Html;
using WebExpress.UI.Attribute;
using WebExpress.UI.Component;
using WebExpress.UI.WebControl;
using WebExpress.Uri;
using WebExpress.WebApp.Components;
using WebExpress.WebApp.WebResource;

namespace WebExpress.Agent.WebControl
{
    [Section(Section.AppPrimary)]
    [Application("*")]
    [Context("general")]
    public sealed class ControlApplications : IComponentMulti
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// 
        public ControlApplications()
            : base()
        {
            Init();
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        private void Init()
        {
        }

        /// <summary>
        /// Erstellt Komponenten eines gemeinsammen Typs T
        /// </summary>
        /// <returns>Die erzeugten Komponenten</returns>
        public IEnumerable<T> Create<T>() where T : IControl
        {
            var list = new List<IControl>();

            foreach (var v in ViewModel.Instance.Applications)
            {
                list.Add(new ControlDropdownItemLink() { Text = v.Name, Uri = new UriRelative(v.ContextPath) });
            }

            return list.Select(x => (T)x);
        }
    }
}
