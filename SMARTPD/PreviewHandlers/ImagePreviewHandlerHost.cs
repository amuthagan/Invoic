/*
 * FileName: ImagePreviewHandlerHost.cs
 * Author: Alexei Bouravtsev
 * Profiles: http://www.linkedin.com/pub/alexei-bouravtsev/13/201/293
 *           http://aleksey-buravtsev.moikrug.ru/
 */

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PreviewHandlers
{
    /// <summary>
    /// Summary for the PictureBox Ctrl
    /// </summary>
    public class ImagePreviewHandlerHost : PreviewHandlerHostBase, IPaginalPreviewHandlerHost
    {
        #region Members

        public PictureBox PicBox;
        public Panel OuterPanel;
        private Container _container = null;
        private string _picName = "";
        private Bitmap _bitmap = null;
        private IPaginalImageFile _paginalImageFile;

        #endregion

        #region Constants

        private const double ZOOMFACTOR = 1.25;	// = 25% smaller or larger
        private int _zoomPercent = 100;
        private const int MAXZOOMPERCENT = 800;
        private const int MINZOOMPERCENT = 10;

        #endregion

        #region Designer generated code

        private void InitializeComponent()
        {
            this.PicBox = new System.Windows.Forms.PictureBox();
            this.OuterPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).BeginInit();
            this.OuterPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // PicBox
            // 
            this.PicBox.Location = new System.Drawing.Point(29, 19);
            this.PicBox.Name = "PicBox";
            this.PicBox.Size = new System.Drawing.Size(153, 144);
            this.PicBox.TabIndex = 3;
            this.PicBox.TabStop = false;
            // 
            // OuterPanel
            // 
            this.OuterPanel.AutoScroll = true;
            this.OuterPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OuterPanel.Controls.Add(this.PicBox);
            this.OuterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OuterPanel.Location = new System.Drawing.Point(0, 0);
            this.OuterPanel.Name = "OuterPanel";
            this.OuterPanel.Size = new System.Drawing.Size(210, 190);
            this.OuterPanel.TabIndex = 4;
            // 
            // ZoomedPictureBox
            // 
            this.Controls.Add(this.OuterPanel);
            this.Name = "ZoomedPictureBox";
            this.Size = new System.Drawing.Size(210, 190);
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).EndInit();
            this.OuterPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region Constructors

        public ImagePreviewHandlerHost()
        {
            InitializeComponent();
            InitCtrl();	// my special settings for the ctrl
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property to select the picture which is displayed in the picturebox. If the 
        /// file doesnґt exist or we receive an exception, the picturebox displays 
        /// a red cross.
        /// </summary>
        /// <value>Complete filename of the picture, including path information</value>
        /// <remarks>Supported fileformat: *.gif, *.tif, *.jpg, *.bmp</remarks>
        [Browsable(false)]
        public string Picture
        {
            get { return _picName; }
            set
            {
                if (null != value)
                {
                    if (System.IO.File.Exists(value))
                    {
                        try
                        {
                            PicBox.Image = Image.FromFile(value);
                            _picName = value;
                            _zoomPercent = 100;
                        }
                        catch (OutOfMemoryException)
                        {
                            RedCross();
                        }
                    }
                    else
                    {
                        RedCross();
                    }
                }
            }
        }


        [Browsable(false)]
        public Bitmap Bitmap
        {
            get { return _bitmap; }
            private set
            {
                if (null != value)
                {
                    try
                    {
                        PicBox.Image = value;
                        PicBox.Width = value.Width;
                        PicBox.Height = value.Height;
                        _bitmap = value;
                        _zoomPercent = 100;
                    }
                    catch (OutOfMemoryException)
                    {
                        RedCross();
                    }
                }
            }
        }


        /// <summary>
        /// Set the frametype of the picturbox
        /// </summary>
        [Browsable(false)]
        public BorderStyle Border
        {
            get { return OuterPanel.BorderStyle; }
            set { OuterPanel.BorderStyle = value; }
        }

        #endregion

        #region Other Methods

        /// <summary>
        /// Special settings for the picturebox ctrl
        /// </summary>
        private void InitCtrl()
        {
            PicBox.SizeMode = PictureBoxSizeMode.StretchImage;
            PicBox.Location = new Point(0, 0);
            OuterPanel.Dock = DockStyle.Fill;
            OuterPanel.Cursor = new Cursor(GetType(), "zoom.cur");

            OuterPanel.AutoScroll = true;
            OuterPanel.MouseEnter += PicBox_MouseEnter;
            PicBox.MouseEnter += PicBox_MouseEnter;
            OuterPanel.MouseWheel += PicBox_MouseWheel;
            OuterPanel.MouseClick += PicBox_MouseClick;
            PicBox.MouseClick += PicBox_MouseClick;
        }

        /// <summary>
        /// Create a simple red cross as a bitmap and display it in the picturebox
        /// </summary>
        private void RedCross()
        {
            Bitmap bmp = new Bitmap(OuterPanel.Width, OuterPanel.Height, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
            Graphics gr;
            gr = Graphics.FromImage(bmp);
            Pen pencil = new Pen(Color.Red, 5);
            gr.DrawLine(pencil, 0, 0, OuterPanel.Width, OuterPanel.Height);
            gr.DrawLine(pencil, 0, OuterPanel.Height, OuterPanel.Width, 0);
            PicBox.Image = bmp;
            //PicBox.Width = bmp.Width;
            //PicBox.Height = bmp.Height;
            gr.Dispose();
        }

        #endregion

        #region Zooming Methods
        /// <summary>
        /// Make the PictureBox dimensions larger to effect the Zoom.
        /// </summary>
        /// <remarks>Maximum 5 times bigger</remarks>
        private void ZoomIn()
        {
            var preZoomPercent = Convert.ToInt32(_zoomPercent * ZOOMFACTOR);

            if (preZoomPercent < MAXZOOMPERCENT)
            {
                _zoomPercent = preZoomPercent;

                PicBox.Width = Convert.ToInt32(PicBox.Width * ZOOMFACTOR);
                PicBox.Height = Convert.ToInt32(PicBox.Height * ZOOMFACTOR);
                PicBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        /// <summary>
        /// Make the PictureBox dimensions smaller to effect the Zoom.
        /// </summary>
        /// <remarks>Minimum 5 times smaller</remarks>
        private void ZoomOut()
        {
            var preZoomPercent = Convert.ToInt32(_zoomPercent / ZOOMFACTOR);

            if (preZoomPercent > MINZOOMPERCENT)
            {
                _zoomPercent = preZoomPercent;

                PicBox.SizeMode = PictureBoxSizeMode.StretchImage;
                PicBox.Width = Convert.ToInt32(PicBox.Width / ZOOMFACTOR);
                PicBox.Height = Convert.ToInt32(PicBox.Height / ZOOMFACTOR);
            }
        }

        #endregion

        #region Mouse events

        /// <summary>
        /// We use the mousewheel to zoom the picture in or out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PicBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                ZoomOut();
            }
            else
            {
                ZoomIn();
            }
        }

        private void PicBox_MouseClick(object sender, MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.Button == MouseButtons.Left)
                ZoomIn();
            else if(mouseEventArgs.Button == MouseButtons.Right)
                ZoomOut();
        }

        /// <summary>
        /// Make sure that the PicBox have the focus, otherwise it doesnґt receive 
        /// mousewheel events !.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PicBox_MouseEnter(object sender, EventArgs e)
        {
            if (PicBox.Focused == false)
            {
                PicBox.Focus();
            }
        }

        #endregion

        #region Disposing

        /// <summary>
        /// Dispose resources.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_container != null)
                    _container.Dispose();
            }
            base.Dispose(disposing);
        }

        public override bool Open(string filename)
        {
            _paginalImageFile = ImageFactory.Create(filename);
            Bitmap = _paginalImageFile.CurrentPage;

            return true;
        }

        #endregion

        #region Implementation of IPaginalPreviewHandlerHost
        public short PagesNumber
        {
            get
            {
                return _paginalImageFile.PagesNumber;
            }
        }

        public short CurrentPageNumber
        {
            get
            {
                return _paginalImageFile.CurrentPageNumber;
            }
        }

        public void NextPage()
        {
            Bitmap = _paginalImageFile.GetNextPage();
        }

        public void PreviousPage()
        {
            Bitmap = _paginalImageFile.GetPreviousPage();
        }

        public void GoToPage(short pageNumber)
        {
            Bitmap = _paginalImageFile.GoToPage(pageNumber);
        }

        #endregion
    }
}
