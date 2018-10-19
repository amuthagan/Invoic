/*
 * FileName: ImageFactory.cs
 * Author: Alexei Bouravtsev
 * Profiles: http://www.linkedin.com/pub/alexei-bouravtsev/13/201/293
 *           http://aleksey-buravtsev.moikrug.ru/
 */

using System;
using System.Drawing;
using System.IO;
using PreviewHandlers.Resources;

namespace PreviewHandlers
{
    public static class ImageFactory
    {
        public static IPaginalImageFile Create(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException(Messages.FileNotFound, fileName);

            string extension = Path.GetExtension(fileName.ToLower());

            if (string.IsNullOrEmpty(extension))
                throw new ArgumentException(Messages.FileWithoutExtension, fileName);

            string lowPointlessExt = extension.ToLower().Replace(@".", string.Empty);

            IPaginalImageFile image = null;

            Bitmap bimap = null;

            if (lowPointlessExt == "tif" || lowPointlessExt == "tiff")
            {
                try
                {
                    // first of all will try to use native .net image classes
                    // because they work much faster then CachedLibTiffImage
                    image = new PaginalImage(fileName);
                    // tyr to load image into memory
                    bimap = image.CurrentPage;
                }
                catch
                {
                    image = new CachedLibTiffImage(fileName);
                    bimap = image.CurrentPage;
                }
            } else
                image = new PaginalImage(fileName);

            return image;
        }
    }
}
