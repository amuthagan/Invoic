/*
 * FileName: PreviewHandlerHostFactory.cs
 * Author: Alexei Bouravtsev
 * Profiles: http://www.linkedin.com/pub/alexei-bouravtsev/13/201/293
 *           http://aleksey-buravtsev.moikrug.ru/
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PreviewHandlers
{
    public class PreviewHandlerHostFactory
    {
        public static PreviewHandlerHostBase Create(
            string fileName,
            List<Tuple<PreviewHandlerHostBase, List<string>, bool>> previewHandlerHostMapping
            )
        {
            PreviewHandlerHostBase previewHandlerHost = null;

            string extension = Path.GetExtension(fileName.ToLower());

            if (!string.IsNullOrEmpty(extension))
            {
                string lowPointlessExt = extension.ToLower().Replace(@".", string.Empty);

                foreach (var mapping in previewHandlerHostMapping)
                {
                    if (mapping.Item2 != null)
                    foreach (var ext in mapping.Item2)
                    {
                        if (lowPointlessExt == ext)
                        {
                            previewHandlerHost = mapping.Item1;
                            break;
                        }
                    }

                    if (previewHandlerHost != null)
                        break;
                }
            }

            if (previewHandlerHost == null)
            {
                var defaultHandlerHostMapping = previewHandlerHostMapping.FirstOrDefault(t => t.Item3);
                if (defaultHandlerHostMapping != null)
                    previewHandlerHost = defaultHandlerHostMapping.Item1;
            }

            return previewHandlerHost;
        }
    }
}
