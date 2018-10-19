/*
 * FileName: IPreviewHandlerHost.cs
 * Author: Alexei Bouravtsev
 * Profiles: http://www.linkedin.com/pub/alexei-bouravtsev/13/201/293
 *           http://aleksey-buravtsev.moikrug.ru/
 */

namespace PreviewHandlers
{
    public interface IPreviewHandlerHost
    {
        bool Open(string filename);
    }
}
