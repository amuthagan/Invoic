/*
 * FileName: AppPreviewHelperBase.cs
 * Author: Alexei Bouravtsev
 * Profiles: http://www.linkedin.com/pub/alexei-bouravtsev/13/201/293
 *           http://aleksey-buravtsev.moikrug.ru/
 */

using System.Collections.Generic;

namespace PreviewHandlers
{
    public abstract class AppPreviewHelperBase : IPreviewHelper
    {
        public string ProcessName { get; set; }

        public List<string> Extensions { get; set; }

        public bool DoPreviewOnResize { get; set; }

        public abstract void Startup();

        public abstract void Cleanup();
    }
}
