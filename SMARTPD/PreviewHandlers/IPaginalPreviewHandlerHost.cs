/*
 * FileName: IPaginalPreviewHandlerHost.cs
 * Author: Alexei Bouravtsev
 * Profiles: http://www.linkedin.com/pub/alexei-bouravtsev/13/201/293
 *           http://aleksey-buravtsev.moikrug.ru/
 */

using System;

namespace PreviewHandlers
{
    public interface IPaginalPreviewHandlerHost
    {
        /// <summary>
        /// Pages number
        /// </summary>
        short PagesNumber { get; }

        /// <summary>
        /// Current page number
        /// </summary>
        short CurrentPageNumber { get; }

        /// <summary>
        /// Goes to the next page
        /// </summary>
        void NextPage();

        /// <summary>
        /// Goes to the previous page
        /// </summary>
        void PreviousPage();

        /// <summary>
        /// Goes to the page with specified number
        /// </summary>
        /// <param name="pageNumber">page number</param>
        /// <exception cref="ArgumentOutOfRangeException">In case if page number is wrong</exception>
        void GoToPage(short pageNumber);
    }
}
