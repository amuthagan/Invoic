/*
 * FileName: CachedLibTiffImage.cs
 * Author: Alexei Bouravtsev
 * Profiles: http://www.linkedin.com/pub/alexei-bouravtsev/13/201/293
 *           http://aleksey-buravtsev.moikrug.ru/
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using BitMiracle.LibTiff.Classic;
using PreviewHandlers.Resources;

namespace PreviewHandlers
{
    public class CachedLibTiffImage : IPaginalImageFile
    {
        /// <summary>
        /// Converted to disk pages of multipaged TIF
        /// </summary>
        private readonly Dictionary<short, Tuple<string, bool, string>> _pageFiles;

        private static volatile object _locker = new object();

        /// <summary>
        /// TIF file name
        /// </summary>
        public string FileName { get; protected set; }

        private Bitmap _bitmap;

        private Thread _thread;

        /// <summary>
        /// GDI+ bitmap
        /// </summary>
        public Bitmap CurrentPage
        {
            get
            {
                if (_bitmap == null)
                    _bitmap = Convert();

                return _bitmap;
            }
        }

        private short _pagesNumber = -1;

        private bool _isDisposed;

        /// <summary>
        /// Pages number
        /// </summary>
        public short PagesNumber
        {
            get
            {
                if (_pagesNumber < 0)
                    _pagesNumber = this.GetPagesNumber(this.FileName);
                return _pagesNumber;
            }
        }

        /// <summary>
        /// Current page number
        /// </summary>
        public short CurrentPageNumber { get; private set; }

        public CachedLibTiffImage(string fileName)
        {
            FileName = fileName;
            CurrentPageNumber = 0;
            _pageFiles = new Dictionary<short, Tuple<string, bool, string>>();
            ConvertAll();
        }

        public Bitmap GetNextPage()
        {
            if (CurrentPageNumber < PagesNumber - 1)
                CurrentPageNumber++;
            else
                CurrentPageNumber = 0;

            _bitmap = Convert();

            return _bitmap;
        }

        public Bitmap GetPreviousPage()
        {
            if (CurrentPageNumber == 0)
                CurrentPageNumber = (short)(PagesNumber - 1);
            else
                CurrentPageNumber--;

            _bitmap = Convert();

            return _bitmap;
        }

        public Bitmap GoToPage(short pageNumber)
        {
            if (pageNumber > PagesNumber - 1)
                throw new ArgumentOutOfRangeException(
                    "pageNumber",
                     Messages.ExceedeingPageNumber
                );

            CurrentPageNumber = pageNumber;
            _bitmap = Convert();
            return _bitmap;
        }

        private void ConvertAll()
        {
            var threadStart = new ThreadStart(
                delegate
                {
                    for (short i = 0; i < PagesNumber; i++)
                    {
                        Convert32Bit(i);
                    }
                }
                );
            _thread = new Thread(threadStart);
            _thread.Start();
        }

        private Bitmap Convert()
        {
            Convert32Bit(CurrentPageNumber);

            if (_pageFiles.ContainsKey(CurrentPageNumber))
            {
                while (_pageFiles[CurrentPageNumber].Item2 == true)
                {
                    Thread.Sleep(100);
                }
                return (Bitmap)Bitmap.FromFile(_pageFiles[CurrentPageNumber].Item1);
            }

            return null;
        }

        private void Convert32Bit(short currentPageNumber)
        {
            try
            {
                if (!File.Exists(FileName))
                    throw new FileNotFoundException(Messages.FileNotFound, FileName);

                Bitmap bitmap = null;

                string tempFileName = Path.GetTempFileName();
                var lockedFileName = new Tuple<string, bool, string>(
                    tempFileName,
                    true, // lock tuple
                    string.Empty
                    );

                lock (_locker)
                {
                    if (_pageFiles.ContainsKey(currentPageNumber))
                        return;

                    _pageFiles.Add(
                        currentPageNumber,
                        lockedFileName
                        );
                }

                using (Tiff tif = Tiff.Open(FileName, "r"))
                {
                    tif.SetDirectory(currentPageNumber);

                    // Find the width and height of the image
                    FieldValue[] value = tif.GetField(TiffTag.IMAGEWIDTH);
                    int width = value[0].ToInt();

                    value = tif.GetField(TiffTag.IMAGELENGTH);
                    int height = value[0].ToInt();

                    // Read the image into the memory buffer
                    int[] raster = new int[height * width];
                    if (!tif.ReadRGBAImage(width, height, raster))
                        throw new Exception(Messages.CantReadFile + ": " + FileName);

                    bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);

                    Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

                    BitmapData bmpdata = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
                    byte[] bits = new byte[bmpdata.Stride * bmpdata.Height];

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        int rasterOffset = y * bitmap.Width;
                        int bitsOffset = (bitmap.Height - y - 1) * bmpdata.Stride;

                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            int rgba = raster[rasterOffset++];
                            bits[bitsOffset++] = (byte)((rgba >> 16) & 0xff);
                            bits[bitsOffset++] = (byte)((rgba >> 8) & 0xff);
                            bits[bitsOffset++] = (byte)(rgba & 0xff);
                            bits[bitsOffset++] = (byte)((rgba >> 24) & 0xff);
                        }
                    }

                    Marshal.Copy(bits, 0, bmpdata.Scan0, bits.Length);
                    bitmap.UnlockBits(bmpdata);


                    bitmap.Save(_pageFiles[currentPageNumber].Item1);

                    _pageFiles[currentPageNumber] = new Tuple<string, bool, string>(
                        tempFileName,
                        false, // unlock
                        string.Empty
                        );
                }
            }
            catch (Exception exception)
            {
                string tempFileName = Path.GetTempFileName();
                string userErrorMessage = Messages.ErrorOccured + ": " + exception.Message;
                var errorBitmap = CreateBitmapWithText(userErrorMessage);
                errorBitmap.Save(tempFileName);
                var tuple = new Tuple<string, bool, string>(
                        tempFileName,
                        false,
                        userErrorMessage
                        );
                if (_pageFiles.ContainsKey(currentPageNumber))
                    _pageFiles[currentPageNumber] = tuple;
                else
                    _pageFiles.Add(currentPageNumber, tuple);
            }
        }

        private Bitmap CreateBitmapWithText(string errorText)
        {
            const float size = 10;

            string errText = errorText.Replace("\n", " ");

            var bitmap = new Bitmap(errText.Length * (int)size, (int)size * 2);

            var font = new Font(FontFamily.GenericMonospace, size);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                TextRenderer.DrawText(
                    g,
                    errText,
                    font,
                    Point.Empty,
                    Color.Black,
                    SystemColors.Control
                );
            }

            return bitmap;
        }

        private short GetPagesNumber(string fileName)
        {
            using (Tiff image = Tiff.Open(fileName, "r"))
                return GetPagesNumber(image);
        }

        private short GetPagesNumber(Tiff image)
        {
            short pageCount = 0;
            do
            {
                ++pageCount;
            } while (image.ReadDirectory());

            return pageCount;
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

                if (_thread != null && _thread.IsAlive)
                    _thread.Abort();
            }

            _isDisposed = true;
        }
        #endregion
    }
}
