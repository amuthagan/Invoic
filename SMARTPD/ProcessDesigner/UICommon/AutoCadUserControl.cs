using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcessDesigner.UICommon
{
    public partial class AutoCadUserControl : UserControl
    {
        public AutoCadUserControl()
        {
            InitializeComponent();
        }

        private string _src;

        public string Src
        {
            get
            {
                return _src;
            }
            set
            {
                _src = value;
                PreviewAutoCard.Src = value;
            }
        }

        public AxACCTRLLib.AxAcCtrl PreviewAutoCard
        {
            get
            {
                return viewautocad;
            }
        }

    }
}
