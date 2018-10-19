/*
 * FileName: DefaultPreviewHandlerHost.cs
 * Author: Alexei Bouravtsev
 * Profiles: http://www.linkedin.com/pub/alexei-bouravtsev/13/201/293
 *           http://aleksey-buravtsev.moikrug.ru/
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.ComponentModel;
using System.Drawing;

using Microsoft.Win32;
using PreviewHandlers.Resources;

namespace PreviewHandlers
{
    public class DefaultPreviewHandlerHost : PreviewHandlerHostBase
    {
        private object _currentPreviewHandler;
        private Guid _currentPreviewHandlerGuid;
        private Stream _currentPreviewHandlerStream;
        private string _errorMessage;
        private IPreviewHelper _appDescription;

        public List<IPreviewHelper> PreviewAppDescriptions { get; set; }

        private string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                Invalidate();	// repaint the control
            }
        }

        /// <summary>
        /// Gets or sets the background colour of this PreviewHandlerHost.
        /// </summary>
        [DefaultValue("White")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        /// <summary>
        /// Initialialises a new instance of the PreviewHandlerHost class.
        /// </summary>
        public DefaultPreviewHandlerHost()
        {
            _currentPreviewHandlerGuid = Guid.Empty;
            BackColor = Color.White;
            Size = new Size(320, 240);

            // display default error message (no file)
            ErrorMessage = Messages.FileNotLoaded; // "Файл не загружен.";

            InitializePreviewAppDescriptions();

            // enable transparency
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
        }

        private void InitializePreviewAppDescriptions()
        {
            PreviewAppDescriptions = new List<IPreviewHelper>();
            PreviewAppDescriptions.Add(new ExcelPreviewHelper());
            PreviewAppDescriptions.Add(new WordPreviewHelper());
            PreviewAppDescriptions.Add(new VisioPreviewHelper());
            PreviewAppDescriptions.Add(new AutoCadPreviewHelper());
            
        }

        /// <summary>
        /// Releases the unmanaged resources used by the PreviewHandlerHost and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            UnloadPreviewHandler();

            if (_currentPreviewHandler != null)
            {
                Marshal.FinalReleaseComObject(_currentPreviewHandler);
                _currentPreviewHandler = null;
                GC.Collect();
            }

            if (_appDescription != null)
            {
                try
                {
                    _appDescription.Cleanup();
                } catch(Exception) {}
            }

            base.Dispose(disposing);
        }

        private Guid GetPreviewHandlerGUID(string filename)
        {
            // open the registry key corresponding to the file extension
            RegistryKey ext = Registry.ClassesRoot.OpenSubKey(Path.GetExtension(filename));
            if (ext != null)
            {
                // open the key that indicates the GUID of the preview handler type
                RegistryKey test = ext.OpenSubKey("shellex\\{8895b1c6-b41f-4c1c-a562-0d564250836f}");
                if (test != null) return new Guid(Convert.ToString(test.GetValue(null)));

                // sometimes preview handlers are declared on key for the class
                string className = Convert.ToString(ext.GetValue(null));
                if (className != null)
                {
                    test = Registry.ClassesRoot.OpenSubKey(className + "\\shellex\\{8895b1c6-b41f-4c1c-a562-0d564250836f}");
                    if (test != null) return new Guid(Convert.ToString(test.GetValue(null)));
                }
            }

            return Guid.Empty;
        }

        /// <summary>
        /// Paints the error message text on the PreviewHandlerHost control.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_errorMessage != String.Empty)
            {
                // paint the error message
                TextRenderer.DrawText(
                    e.Graphics,
                    Messages.FileNotLoaded,
                    Font,
                    ClientRectangle,
                    ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis
                );
            }
        }

        /// <summary>
        /// Resizes the hosted preview handler when this PreviewHandlerHost is resized.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (_currentPreviewHandler is IPreviewHandler)
            {
                // update the preview handler's bounds to match the control's
                Rectangle r = ClientRectangle;

                if (_appDescription != null && _appDescription.DoPreviewOnResize)
                {
                    try
                    {
                        ((IPreviewHandler) _currentPreviewHandler).SetRect(ref r);
                        ((IPreviewHandler) _currentPreviewHandler).DoPreview();
                    } catch(COMException comException)
                    {
                        if (comException.ErrorCode == -2042494973)
                            throw new Exception("Possibly full path to document needed!", comException);
                    }
                }
                else
                    ((IPreviewHandler)_currentPreviewHandler).SetWindow(Handle, ref r);
            }
            else
            {
                this.Refresh();
            }
        }

        /// <summary>
        /// Opens the specified file using the appropriate preview handler and displays the result in this PreviewHandlerHost.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public override bool Open(string filename)
        {
            _appDescription = PreviewAppDescriptionFactory.Create(
                Path.GetExtension(filename),
                PreviewAppDescriptions
            );

            if (_appDescription != null)
                _appDescription.Startup();

            UnloadPreviewHandler();

            if (String.IsNullOrEmpty(filename))
            {
                ErrorMessage = Messages.FileNotLoaded;
                return false;
            }

            // try to get GUID for the preview handler
            Guid guid = GetPreviewHandlerGUID(filename);
            ErrorMessage = "";

            if (guid != Guid.Empty)
            {
                try
                {
                    if (guid != _currentPreviewHandlerGuid)
                    {
                        _currentPreviewHandlerGuid = guid;

                        // need to instantiate a different COM type (file format has changed)
                        if (_currentPreviewHandler != null) Marshal.FinalReleaseComObject(_currentPreviewHandler);

                        // use reflection to instantiate the preview handler type
                        Type comType = Type.GetTypeFromCLSID(_currentPreviewHandlerGuid);
                        _currentPreviewHandler = Activator.CreateInstance(comType);
                    }

                    if (_currentPreviewHandler is IInitializeWithFile)
                    {
                        // some handlers accept a filename
                        ((IInitializeWithFile)_currentPreviewHandler).Initialize(filename, 0);
                    }
                    else if (_currentPreviewHandler is IInitializeWithStream)
                    {
                        if (File.Exists(filename))
                        {
                            // other handlers want an IStream (in this case, a file stream)
                            _currentPreviewHandlerStream = File.Open(filename, FileMode.Open);
                            StreamWrapper stream = new StreamWrapper(_currentPreviewHandlerStream);
                            ((IInitializeWithStream)_currentPreviewHandler).Initialize(stream, 0);
                        }
                        else
                        {
                            ErrorMessage = Messages.FileNotFound;
                        }
                    }

                    if (_currentPreviewHandler is IPreviewHandler)
                    {
                        // bind the preview handler to the control's bounds and preview the content
                        Rectangle r = ClientRectangle;
                        ((IPreviewHandler)_currentPreviewHandler).SetWindow(Handle, ref r);
                        ((IPreviewHandler)_currentPreviewHandler).DoPreview();


                        return true;
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = Messages.PreviewError + ":\n" + ex.Message;
                }
            }
            else
            {
                ErrorMessage = Messages.NoPreviewAvailable;
            }

            return false;
        }

        /// <summary>
        /// Opens the specified stream using the preview handler COM type with the provided GUID and displays the result in this PreviewHandlerHost.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="previewHandler"></param>
        /// <returns></returns>
        public bool Open(Stream stream, Guid previewHandler)
        {
            UnloadPreviewHandler();

            if (stream == null)
            {
                ErrorMessage = Messages.FileNotLoaded;
                return false;
            }

            ErrorMessage = "";

            if (previewHandler != Guid.Empty)
            {
                try
                {
                    if (previewHandler != _currentPreviewHandlerGuid)
                    {
                        _currentPreviewHandlerGuid = previewHandler;

                        // need to instantiate a different COM type (file format has changed)
                        if (_currentPreviewHandler != null) Marshal.FinalReleaseComObject(_currentPreviewHandler);

                        // use reflection to instantiate the preview handler type
                        Type comType = Type.GetTypeFromCLSID(_currentPreviewHandlerGuid);
                        _currentPreviewHandler = Activator.CreateInstance(comType);
                    }

                    if (_currentPreviewHandler is IInitializeWithStream)
                    {
                        // must wrap the stream to provide compatibility with IStream
                        _currentPreviewHandlerStream = stream;
                        StreamWrapper wrapped = new StreamWrapper(_currentPreviewHandlerStream);
                        ((IInitializeWithStream)_currentPreviewHandler).Initialize(wrapped, 0);
                    }

                    if (_currentPreviewHandler is IPreviewHandler)
                    {
                        // bind the preview handler to the control's bounds and preview the content
                        Rectangle r = ClientRectangle;
                        ((IPreviewHandler)_currentPreviewHandler).SetWindow(Handle, ref r);
                        ((IPreviewHandler)_currentPreviewHandler).DoPreview();

                        return true;
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = Messages.PreviewError + ":\n" + ex.Message;
                }
            }
            else
            {
                ErrorMessage = Messages.NoPreviewAvailable;
            }

            return false;
        }

        /// <summary>
        /// Unloads the preview handler hosted in this PreviewHandlerHost and closes the file stream.
        /// </summary>
        public void UnloadPreviewHandler()
        {
            try
            {
                if (_currentPreviewHandler is IPreviewHandler)
                {
                    try
                    {
                        // explicitly unload the content
                        ((IPreviewHandler) _currentPreviewHandler).Unload();
                    }
                    catch (Exception)
                    {
                    }
                }
            } catch (Exception) { }

            if (_currentPreviewHandlerStream != null)
            {
                _currentPreviewHandlerStream.Close();
                _currentPreviewHandlerStream = null;
            }

            _currentPreviewHandlerGuid = new Guid();
        }
    }
}