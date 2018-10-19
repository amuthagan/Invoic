/*
 * FileName: BrowserPreviewHandlerHost.cs
 * Author: Alexei Bouravtsev
 * Profiles: http://www.linkedin.com/pub/alexei-bouravtsev/13/201/293
 *           http://aleksey-buravtsev.moikrug.ru/
 */

using System.Linq;
using System.Windows.Forms;

namespace PreviewHandlers
{
    public class BrowserPreviewHandlerHost : PreviewHandlerHostBase
    {
        private WebBrowser _browser;
        private WebBrowser WebBrowser
        {
            get
            {
                if (_browser == null)
                {
                    _browser = new WebBrowser();
                    _browser.Dock = DockStyle.Fill;
                    _browser.Name = "webbrowser";
                    _browser.ScrollBarsEnabled = true;
                }
                return _browser;
            }
        }

        public override bool Open(string fileName)
        {
            WebBrowser.Navigate(fileName, false);

            if (!Controls.Find("webbrowser", true).Any())
                Controls.Add(WebBrowser);

            return true;
        }
    }
}
