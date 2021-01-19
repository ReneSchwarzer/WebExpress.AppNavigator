using System.Collections.Generic;
using System.Linq;
using WebExpress.Agent.Model;
using WebExpress.Attribute;
using WebExpress.UI.Attribute;
using WebExpress.UI.Component;
using WebExpress.UI.WebControl;
using WebExpress.Uri;
using WebExpress.WebApp.Components;

namespace WebExpress.Agent.WebControl
{
    [Section(Section.AppPrimary)]
    [Application("*")]
    [Context("general")]
    public sealed class ControlApplications : IComponentDynamic
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

            foreach (var v in ViewModel.Instance.GlobalApplications.OrderBy(x => x.Name))
            {
                list.Add(new ControlDropdownItemLink() 
                { 
                    Text = v.Name, 
                    Icon = string.IsNullOrWhiteSpace(v.Icon) ? null : new PropertyIcon(new UriAbsolute(v.Icon)),
                    Uri = new UriAbsolute(UriScheme.Http, new UriAuthority(v.Host), new UriRelative(v.ContextPath)) 
                });
            }

            return list.Select(x => (T)x);
        }
    }
}
