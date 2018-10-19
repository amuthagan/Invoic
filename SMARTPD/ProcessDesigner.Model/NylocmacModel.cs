using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class NylocmacModel
    {
        private string _costCentCode;
        private Nullable<decimal> _minAf;
        private Nullable<decimal> _maxAf;
        private Nullable<decimal> _maxThickness;
        private Nullable<decimal> _maxDia;
        private string _prodHeadType;


        public string COST_CENT_CODE
        {
            get { return _costCentCode; }
            set
            {
                _costCentCode = value;
            }
        }


        public Nullable<decimal> MIN_AF
        {
            get { return _minAf; }
            set
            {
                _minAf = value;
            }
        }

        public Nullable<decimal> MAX_AF
        {
            get { return _maxAf; }
            set
            {
                _maxAf = value;
            }
        }

        public Nullable<decimal> MAX_THICKNESS
        {
            get { return _maxThickness; }
            set
            {
                _maxThickness = value;
            }
        }

        public Nullable<decimal> MAX_DIA
        {
            get { return _maxDia; }
            set
            {
                _maxDia = value;
            }
        }

        public string PROD_HEAD_TYPE
        {
            get { return _prodHeadType; }
            set
            {
                _prodHeadType = value;
            }
        }

    }
}
