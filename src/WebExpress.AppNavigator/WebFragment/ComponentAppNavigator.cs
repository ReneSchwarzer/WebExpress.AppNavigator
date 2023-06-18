using System.Collections.Generic;
using System.Linq;
using WebExpress.AppNavigator.Model;
using WebExpress.UI.WebAttribute;
using WebExpress.UI.WebControl;
using WebExpress.UI.WebFragment;
using WebExpress.WebApp.WebFragment;
using WebExpress.WebAttribute;
using WebExpress.WebPage;

namespace WebExpres.AppNavigator.WebFragment
{
    [Section(Section.AppPrimary)]
    [Application("*")]
    public sealed class ComponentAppNavigator : IFragmentDynamic
    {
        /// <summary>
        /// Returns the context.
        /// </summary>
        public IFragmentContext Context { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ComponentAppNavigator()
            : base()
        {
        }

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="page">The page where the fragment is active.</param>
        public void Initialization(IFragmentContext context, IPage page)
        {
            Context = context;
        }

        /// <summary>
        /// Creates a fragment of a common type T.
        /// </summary>
        /// <returns>The control that was created.</returns>
        public IEnumerable<T> Create<T>() where T : IControl
        {
            var list = new List<IControl>();

            foreach (var v in ViewModel.ApplicationDictionary.Values.OrderBy(x => x.Name))
            {
                list.Add(new ControlDropdownItemLink()
                {
                    Text = v.Name,
                    Icon = string.IsNullOrWhiteSpace(v.Icon) ? null : new PropertyIcon(v.Icon, new PropertyMaxSizeIcon(1, 1, TypeSizeUnit.Rem)),
                    Uri = v.ContextPath
                });
            }

            return list.Select(x => (T)x);
        }
    }
}
