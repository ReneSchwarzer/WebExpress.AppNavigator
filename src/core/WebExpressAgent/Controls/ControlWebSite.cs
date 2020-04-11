using Agent.Model;
using WebExpress.Pages;
using WebExpress.UI.Controls;
using WebServer.Html;

namespace Agent.Controls
{
    public class ControlWebSite : ControlPanelCard
    {
        /// <summary>
        /// Liefert oder setzt die Webseite
        /// </summary>
        //public WebSiteItem WebSite { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="page">Die zugehörige Seite</param>
        /// <param name="id">Die ID</param>
        public ControlWebSite(IPage page, string id = null)
            : base(page, id)
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
        /// In HTML konvertieren
        /// </summary>
        /// <returns>Das Control als HTML</returns>
        public override IHtmlNode ToHtml()
        {
            //var icon = WebSite.IconUrl;

            //if (!icon.StartsWith("/"))
            //{
            //    icon = "/" + icon;
            //}

            //icon = WebSite.Url + icon;

            //var media = new ControlPanelMedia(Page)
            //{
            //    Image = string.IsNullOrWhiteSpace(WebSite.IconUrl) ? Page.GetPath(0, "Assets/img/Logo.png") : new Path(null, icon),
            //    ImageWidth = 100,
            //    ImageHeight = 100,
            //    Title = new ControlLink(Page)
            //    {
            //        Text = WebSite.Name,
            //        Url = new Path(null, WebSite.Url),
            //        Color = TypesTextColor.Dark
            //    }
            //};

            //media.Content.Add(new ControlHtml(Page)
            //{
            //    Html = ViewModel.Instance.Status.ContainsKey(WebSite) ? ViewModel.Instance.Status[WebSite] : string.Empty
            //});

            //var flex = new ControlFlexbox(Page)
            //{
            //    Direction = TypesFlexboxDirection.Horizontal
            //};

            //media.Content.Add(flex);

            //Content.Add(media);

            return base.ToHtml();
        }
    }
}
