using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using System.Data;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ProcessDesigner.Model;
using ProcessDesigner.UserControls;


namespace ProcessDesigner.ViewModel
{
    public class TempViewModel : Essential
    {
        public TempViewModel(UserInformation _userInformation)
        {
            this.userInformation = _userInformation;
            DtData = GetOpertionMaster();
            LstRawMaterial = GetRawMaterialsSize1();
        }

        public DataView GetOpertionMaster()
        {
            DataTable dataValue;
            dataValue = ToDataTable((from c in DB.DDOPER_MAST.AsEnumerable()
                                     where ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                     orderby c.OPER_CODE ascending
                                     select new { c.OPER_CODE, c.OPER_DESC, c.SHOW_IN_COST }).ToList());
            return dataValue.DefaultView;
        }

        private DataView _dtdata;
        public DataView DtData
        {
            get { return _dtdata; }
            set
            {
                _dtdata = value;
                //INotifyPropertyChanged ("DtData");
            }
        }

        //private DataTable lstRawMaterial;
        public DataTable GetRawMaterialsSize1(string rawMaterialCode = "51951")
        {

            DataTable dataValue;
            dataValue = ToDataTable((from row in DB.DDRM_SIZE_MAST.AsEnumerable()
                                     where Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false && row.RM_CODE == rawMaterialCode
                                     orderby row.RM_DIA_MIN
                                     select row).ToList());
            return dataValue;
        }

        //public List<DDRM_SIZE_MAST> GetRawMaterialsSize(string rawMaterialCode = "51951")
        //{
        //    List<DDRM_SIZE_MAST> lstRawMaterial = null;
        //    try
        //    {
        //        if (!DB.IsNotNullOrEmpty()) return lstRawMaterial;
        //        lstRawMaterial = (from row in DB.DDRM_SIZE_MAST
        //                          where Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false && row.RM_CODE == rawMaterialCode
        //                          orderby row.RM_DIA_MIN
        //                          select row).ToList<DDRM_SIZE_MAST>();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }

        //    return lstRawMaterial;
        //}

        private DataTable _lstRawMaterial;
        public DataTable LstRawMaterial
        {
            get { return _lstRawMaterial; }
            set { _lstRawMaterial = value; }
        }

    }
}
