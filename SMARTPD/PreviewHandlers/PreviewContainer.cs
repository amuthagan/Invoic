/*
 * FileName: PreviewContainer.cs
 * Author: Alexei Bouravtsev
 * Profiles: http://www.linkedin.com/pub/alexei-bouravtsev/13/201/293
 *           http://aleksey-buravtsev.moikrug.ru/
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PreviewHandlers
{
    public partial class PreviewContainer : UserControl, IPaginalPreviewHandlerHost
    {
        public DisplayType DisplayType { get; set; }

        private string _sourceDoc;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] 
        private PreviewHandlerHostBase PreviewHandlerHostControl
        {
            get
            {
                var controls = this.Controls.Find("previewHandlerHost", true);

                if (controls.Any())
                    return (PreviewHandlerHostBase)controls[0];
             
                return null;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] 
        public List<Tuple<PreviewHandlerHostBase, List<string>, bool>> PreviewHostsMapping { get; set; }

        public PreviewContainer()
        {
            InitializeComponent();

            InitializeDefaultPreviewHandlerHost();

            DisplayType = DisplayType.DisplayIcon;
        }

        private void InitializeDefaultPreviewHandlerHost()
        {
            PreviewHostsMapping = DefaultPreviewHostsMapping.PreviewHostsMapping;
        }

        public void CreateLink(string sourceDoc)
        {
            this._sourceDoc = sourceDoc;

            if (DisplayType == DisplayType.DisplayIcon)
            {
                this.pictureBox.Image = Icon.ExtractAssociatedIcon(sourceDoc).ToBitmap();
                this.previewHandlerHost.Visible = false;
                this.pictureBox.Visible = true;
            }
            else
            {
                this.pictureBox.Visible = false;
                this.Controls.RemoveByKey("previewHandlerHost");

                if (System.IO.File.Exists(sourceDoc))
                {
                    var previewHandlerHost = PreviewHandlerHostFactory.Create(sourceDoc, PreviewHostsMapping);
                    previewHandlerHost.Visible = false;
                    previewHandlerHost.Name = "previewHandlerHost";
                    previewHandlerHost.Dock = DockStyle.Fill;
                    previewHandlerHost.Open(sourceDoc);
                    this.Controls.Add(previewHandlerHost);
                    previewHandlerHost.Visible = true;
                }
            }
        }

        public void DoVerb(VerbActions action)
        {
            switch (action)
            {
                case VerbActions.Open:
                    //we must open it.
                    Process proc = new Process();
                    proc.StartInfo = new ProcessStartInfo(_sourceDoc);
                    proc.Start();
                    break;
                default:
                    MessageBox.Show(string.Format("Sorry! OLE Action {0} not implemented yet!", action));
                    break;
            }
        }

        #region Implementation of IPaginalPreviewHandlerHost

        /// <summary>
        /// Pages number
        /// </summary>
        public short PagesNumber
        {
            get
            {
                if (PreviewHandlerHostControl is IPaginalPreviewHandlerHost)
                    return ((IPaginalPreviewHandlerHost)PreviewHandlerHostControl).PagesNumber;

                return 1;
            }
        }

        /// <summary>
        /// Current page number
        /// </summary>
        public short CurrentPageNumber
        {
            get
            {
                if (PreviewHandlerHostControl is IPaginalPreviewHandlerHost)
                    return ((IPaginalPreviewHandlerHost)PreviewHandlerHostControl).CurrentPageNumber;
                else
                    return 0;
            }
        }

        /// <summary>
        /// Go to the next page
        /// </summary>
        public void NextPage()
        {
            if (PreviewHandlerHostControl is IPaginalPreviewHandlerHost)
                ((IPaginalPreviewHandlerHost)PreviewHandlerHostControl).NextPage();
        }

        /// <summary>
        /// Go to the previous page
        /// </summary>
        /// <returns></returns>
        public void PreviousPage()
        {
            if (PreviewHandlerHostControl is IPaginalPreviewHandlerHost)
                ((IPaginalPreviewHandlerHost)PreviewHandlerHostControl).PreviousPage();
        }

        /// <summary>
        /// Go to the page with specified number
        /// </summary>
        /// <param name="pageNumber">page number/номер страницы</param>
        /// <exception cref="ArgumentOutOfRangeException">If page number is out of the range/В случае, если номер страницы задан неверно</exception>
        public void GoToPage(short pageNumber)
        {
            if (PreviewHandlerHostControl is IPaginalPreviewHandlerHost)
                ((IPaginalPreviewHandlerHost)PreviewHandlerHostControl).GoToPage(pageNumber);
        }

        #endregion
    }

    public enum VerbActions
    {
        /// <summary>
        /// The default action for the object. 
        /// </summary>
        Primary = 0,
        /// <summary>
        ///  Activates the object for editing. If the application that created the object supports in-place activation, the object is activated within the OLE container control. 
        /// </summary>
        Show = -1,
        /// <summary>
        /// Opens the object in a separate application window. If the application that created the object supports in-place activation, the object is activated in its own window. 
        /// </summary>
        Open = -2,
        /// <summary>
        /// For embedded objects, hides the application that created the object. 
        /// </summary>
        Hide = -3,
        /// <summary>
        ///  If the object supports in-place activation, activates the object for in-place activation and shows any user interface tools. If the object doesn't support in-place activation, the object doesn't activate, and an error occurs. 
        /// </summary>
        UIActivate = -4,
        /// <summary>
        ///  If the user moves the focus to the OLE container control, creates a window for the object and prepares the object to be edited. An error occurs if the object doesn't support activation on a single mouse click. 
        /// </summary>
        InPlaceActivate = -5,
        /// <summary>
        /// Used when the object is activated for editing to discard all record of changes that the object's application can undo. 
        /// </summary>
        DiscardUndoState = -6
    }

    public enum DisplayType
    {
        DisplayIcon,
        DisplayContent
    }
}
