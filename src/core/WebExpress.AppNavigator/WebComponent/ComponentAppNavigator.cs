using System.Collections.Generic;
using System.Linq;
using WebExpress.Agent.Model;
using WebExpress.UI.WebAttribute;
using WebExpress.UI.WebComponent;
using WebExpress.UI.WebControl;
using WebExpress.Uri;
using WebExpress.WebApp.WebComponent;
using WebExpress.WebAttribute;
using WebExpress.WebPage;

namespace WebExpres.Agent.WebComponent
{
    [Section(Section.AppPrimary)]
    [Application("*")]
    [Context("general")]
    public sealed class ComponentAppNavigator : IComponentDynamic
    {
        /// <summary>
        /// Liefert der Kontext
        /// </summary>
        public IComponentContext Context { get; private set; }

        

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// 
        public ComponentAppNavigator()
            : base()
        {
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        /// <param name="context">Der Kontext</param>
        /// <param name="page">Die Seite, indem die Komonente aktiv ist</param>
        public void Initialization(IComponentContext context, IPage page)
        {
            Context = context;
        }

        /// <summary>
        /// Erstellt Komponenten eines gemeinsammen Typs T
        /// </summary>
        /// <returns>Die erzeugten Steuerelement</returns>
        public IEnumerable<T> Create<T>() where T : IControl
        {
            var list = new List<IControl>();

            foreach (var v in ViewModel.ApplicationDictionary.Values.OrderBy(x => x.Name))
            {
                list.Add(new ControlDropdownItemLink()
                {
                    Text = v.Name,
                    Icon = string.IsNullOrWhiteSpace(v.Icon) ? null : new PropertyIcon(new UriAbsolute(v.Host).Append(v.Icon), new PropertySizeIcon(-1, 1, TypeSizeUnit.Em)),
                    Uri = new UriAbsolute(v.Host).Append(v.ContextPath)
                });
            }

            return list.Select(x => (T)x);
        }
    }
}
