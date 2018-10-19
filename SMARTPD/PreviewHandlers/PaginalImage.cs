/*
 * FileName: PaginalImage.cs
 * Author: Alexei Bouravtsev
 * Profiles: http://www.linkedin.com/pub/alexei-bouravtsev/13/201/293
 *           http://aleksey-buravtsev.moikrug.ru/
 */

using System;
using System.Drawing;
using System.Drawing.Imaging;

using PreviewHandlers.Resources;

namespace PreviewHandlers
{
    public class PaginalImage : IPaginalImageFile
    {
        public string FileName { get; private set; }

        private Bitmap _bitmap;

        private bool _isDisposed;

        /// <summary>
        /// GDI+ bitmap
        /// </summary>
        public Bitmap CurrentPage
        {
            get
            {
                if (_bitmap == null)
                    _bitmap = LoadImage();

                return _bitmap;
            }
        }

        /// <summary>
        /// Pages number
        /// </summary>
        public short PagesNumber
        {
            get
            {
                try
                {
                    return (short) _bitmap.GetFrameCount(FrameDimension.Page);
                } catch(Exception)
                {
                    return 1;
                }
            }
        }

        /// <summary>
        /// Current page number
        /// </summary>
        public short CurrentPageNumber { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public PaginalImage(string fileName)
        {
            FileName = fileName;
            CurrentPageNumber = 0;
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~PaginalImage()
        {
            Dispose(false);
        }

        /// <summary>
        /// Get next page
        /// </summary>
        /// <returns></returns>
        public Bitmap GetNextPage()
        {
            if (CurrentPageNumber < PagesNumber - 1)
                CurrentPageNumber++;
            else
                CurrentPageNumber = 0;

            _bitmap.SelectActiveFrame(FrameDimension.Page, CurrentPageNumber);

            return _bitmap;
        }

        /// <summary>
        /// Get previous page
        /// </summary>
        /// <returns></returns>
        public Bitmap GetPreviousPage()
        {
            if (CurrentPageNumber == 0)
                CurrentPageNumber = (short)(PagesNumber - 1);
            else
                CurrentPageNumber--;

            _bitmap.SelectActiveFrame(FrameDimension.Page, CurrentPageNumber);

            return _bitmap;
        }

        /// <summary>
        /// Go to the specified page
        /// </summary>
        /// <param name="pageNumber">page number</param>
        /// <returns></returns>
        public Bitmap GoToPage(short pageNumber)
        {
            if (pageNumber > PagesNumber - 1)
                throw new ArgumentOutOfRangeException(
                    "pageNumber",
                    Messages.ExceedeingPageNumber
                );

            CurrentPageNumber = pageNumber;

            _bitmap.SelectActiveFrame(FrameDimension.Page, pageNumber);

            return _bitmap;
        }

        /// <summary>
        /// Load image from file
        /// </summary>
        /// <returns></returns>
        private Bitmap LoadImage()
        {
            return (Bitmap)Image.FromFile(FileName);
        }

        #region Implementation of IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (_bitmap != null)
                    _bitmap.Dispose();
            }

            _isDisposed = true;
        }
        #endregion
    }
}
