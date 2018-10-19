/*
 * FileName: PreviewHandlerHostBase.cs
 * Author: Alexei Bouravtsev
 * Profiles: http://www.linkedin.com/pub/alexei-bouravtsev/13/201/293
 *           http://aleksey-buravtsev.moikrug.ru/
 */

using System.Windows.Forms;

namespace PreviewHandlers
{
    /// <summary>
    /// Base class for all preview handler hosts
    /// <remarks>This class is not abstract because VS can't show derived controls in design mode
    /// if they are derived from asbtract class.</remarks>
    /// </summary>
    public class PreviewHandlerHostBase : Control, IPreviewHandlerHost
    {
        #region Implementation of IPreviewHandlerHost
        public virtual bool Open(string filename)
        {
            return false;
        }
        #endregion
    }
}
