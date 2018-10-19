/*
 * FileName: PreviewAppDescriptionFactory.cs
 * Author: Alexei Bouravtsev
 * Profiles: http://www.linkedin.com/pub/alexei-bouravtsev/13/201/293
 *           http://aleksey-buravtsev.moikrug.ru/
 */

using System.Collections.Generic;

namespace PreviewHandlers
{
    public class PreviewAppDescriptionFactory
    {
        public static IPreviewHelper Create(
            string extension,
            List<IPreviewHelper> previewAppDescriptions
            )
        {
            IPreviewHelper appDescription = null;

            string lowPointlessExt = extension.ToLower().Replace(@".", string.Empty);

            foreach (var previewAppDescription in previewAppDescriptions)
            {
                foreach (var ext in previewAppDescription.Extensions)
                {
                    if (lowPointlessExt == ext)
                    {
                        appDescription = previewAppDescription;
                        break;
                    }
                }

                if (appDescription != null)
                    break;
            }

            return appDescription;
        }
    }
}
