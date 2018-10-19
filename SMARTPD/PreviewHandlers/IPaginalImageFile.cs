/*
 * FileName: IPaginalImageFile.cs
 * Author: Alexei Bouravtsev
 * Profiles: http://www.linkedin.com/pub/alexei-bouravtsev/13/201/293
 *           http://aleksey-buravtsev.moikrug.ru/
 */

using System;
using System.Drawing;

namespace PreviewHandlers
{
    /// <summary>
    /// Defines methods for multipaged TIF files
    /// </summary>
    public interface IPaginalImageFile : IDisposable
    {
        /// <summary>
        /// File path
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Pages number
        /// </summary>
        short PagesNumber { get; }

        /// <summary>
        /// Current page number for multipaged TIF or 1
        /// </summary>
        short CurrentPageNumber { get; }

        /// <summary>
        /// Gets current page
        /// </summary>
        Bitmap CurrentPage { get; }

        /// <summary>
        /// Gets next page
        /// </summary>
        Bitmap GetNextPage();

        /// <summary>
        /// Gets previous page
        /// </summary>
        /// <returns></returns>
        Bitmap GetPreviousPage();

        /// <summary>
        /// Goes to the page with specified number
        /// </summary>
        /// <param name="pageNumber">page number</param>
        /// <exception cref="ArgumentOutOfRangeException">In case when page number is wrong</exception>
        Bitmap GoToPage(short pageNumber);
    }
}
