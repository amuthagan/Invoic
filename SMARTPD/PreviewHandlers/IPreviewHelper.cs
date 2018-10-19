/*
 * FileName: IPreviewHelper.cs
 * Author: Alexei Bouravtsev
 * Profiles: http://www.linkedin.com/pub/alexei-bouravtsev/13/201/293
 *           http://aleksey-buravtsev.moikrug.ru/
 */

using System.Collections.Generic;

namespace PreviewHandlers
{
    public interface IPreviewHelper
    {
        string ProcessName { get; set; }

        List<string> Extensions { get; set; }

        bool DoPreviewOnResize { get; set; }

        void Startup();

        void Cleanup();
    }
}
