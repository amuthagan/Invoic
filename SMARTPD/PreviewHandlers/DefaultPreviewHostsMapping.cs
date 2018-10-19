/*
 * FileName: DefaultPreviewHostsMapping.cs
 * Author: Alexei Bouravtsev
 * Profiles: http://www.linkedin.com/pub/alexei-bouravtsev/13/201/293
 *           http://aleksey-buravtsev.moikrug.ru/
 */

using System;
using System.Collections.Generic;

namespace PreviewHandlers
{
    public static class DefaultPreviewHostsMapping
    {
        public static List<Tuple<PreviewHandlerHostBase, List<string>, bool>> PreviewHostsMapping { get; set; }

        static DefaultPreviewHostsMapping()
        {
            PreviewHostsMapping = new List<Tuple<PreviewHandlerHostBase, List<string>, bool>>();

            PreviewHandlerHostBase defaultHandlerHost = new DefaultPreviewHandlerHost();
            var defaultHandlerHostMapping = new Tuple<PreviewHandlerHostBase, List<string>, bool>(
                defaultHandlerHost,
                null,
                true
            );
            PreviewHostsMapping.Add(defaultHandlerHostMapping);

            PreviewHandlerHostBase browsertHandlerHost = new BrowserPreviewHandlerHost();
            var browserHandlerHostMapping = new Tuple<PreviewHandlerHostBase, List<string>, bool>(
                browsertHandlerHost,
                new List<string>() { "xml", "htm", "html" }, 
                false
            );
            PreviewHostsMapping.Add(browserHandlerHostMapping);

            PreviewHandlerHostBase imageHandlerHost = new ImagePreviewHandlerHost();
            var imageHandlerHostMapping = new Tuple<PreviewHandlerHostBase, List<string>, bool>(
                imageHandlerHost,
                new List<string>() { "tif", "tiff", "jpg", "jpeg", "bmp", "gif", "png" },
                false
            );

            PreviewHostsMapping.Add(imageHandlerHostMapping);
        }
    }
}
