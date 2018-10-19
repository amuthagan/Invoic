using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using System.Data;
using System.Windows;
using System.ComponentModel;
using ProcessDesigner.Model;

namespace ProcessDesigner.BLL
{
    public class CategoryBll : Essential
    {
        public CategoryBll(UserInformation userInfo)
        {
            this.userInformation = userInfo;
        }

        //public int GetCC(CategoryMaterModel catemodel)
        //{
        //    double serialno = 0;
        //    serialno = (from s in DB.DDCATE_MAST
        //                select s).Max(sno => sno.CATE_CODE.ToDoubleValue());
        //        serialno = serialno + 1;
        //    return serialno.ToString().ToIntValue();
        //}
        public int GetCC(CategoryMaterModel catemodel)
        {
            string cateSno = "1";
            try
            {
                DataTable dataValue = new DataTable();
                dataValue = ToDataTable((from o in DB.DDCATE_MAST
                                         select new { o.CATE_CODE }).ToList());

                // cateSno = (from o in DB.DDCATE_MAST select o.CATE_CODE).Max();
                // if (dataValue.Rows.Count > 0)
                cateSno = (Convert.ToUInt64(Convert.ToUInt32(dataValue.Rows.Count) + 1)).ToValueAsString();
                return cateSno.ToIntValue();
            }
            catch (Exception ex)
            {
                cateSno = "1";
                ex.LogException();
            }
            return cateSno.ToIntValue();
        }

        public bool AddEditCategory(CategoryMaterModel catemodel, ref string mode)
        {
            bool _status = false;
            DDCATE_MAST cate = new DDCATE_MAST();
            try
            {
                cate = (from c in DB.DDCATE_MAST
                        where c.CATE_CODE == catemodel.CateCode.ToValueAsString()
                        select c).FirstOrDefault<DDCATE_MAST>();
                if (cate == null)
                {
                    try
                    {
                        cate = new DDCATE_MAST();
                        cate.CATE_CODE = catemodel.CateCode.ToString();
                        cate.CATEGORY = catemodel.Category;
                        cate.ENTERED_DATE = DateTime.Now;
                        cate.ENTERED_BY = userInformation.UserName;
                        cate.DELETE_FLAG = catemodel.Active;
                        cate.ROWID = Guid.NewGuid();
                        DB.DDCATE_MAST.InsertOnSubmit(cate);
                        DB.SubmitChanges();
                        mode = "Add";
                        return true;
                    }
                    catch (Exception e)
                    {
                        e.LogException();
                        DB.DDCATE_MAST.DeleteOnSubmit(cate);
                    }
                }

                else
                {
                    try
                    {
                        cate.CATEGORY = catemodel.Category;
                        cate.UPDATED_DATE = DateTime.Now;
                        cate.UPDATED_BY = userInformation.UserName;
                        cate.DELETE_FLAG = catemodel.Active;
                        DB.SubmitChanges();
                        mode = "Update";
                        return true;
                    }
                    catch (Exception e)
                    {
                        e.LogException();
                        DB.DDCATE_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, cate);
                    }
                }
            }
            catch (Exception e)
            {
                throw e.LogException();
            }
            return _status;
        }


        public DataView GetCategory()
        {
            DataTable dataValue;
            dataValue = ToDataTable((from c in DB.DDCATE_MAST.AsEnumerable()
                                     where (!string.IsNullOrEmpty(c.CATEGORY))
                                     orderby c.CATEGORY ascending
                                     select new { c.CATEGORY, c.CATE_CODE, STATUS = ((c.DELETE_FLAG == null) ? true : !c.DELETE_FLAG).FromBooleanAsString(false) }).ToList());
            return dataValue.DefaultView;
        }

        public bool DeletePswApplication(string category)
        {
            DDCATE_MAST cate = new DDCATE_MAST();
            try
            {
                cate = (from c in DB.DDCATE_MAST
                        where c.CATEGORY == category//   && ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                        select c).FirstOrDefault<DDCATE_MAST>();

                if (cate != null)
                {
                    if (cate.DELETE_FLAG == true)
                    {
                        cate.DELETE_FLAG = false;
                    }
                    else
                    {
                        cate.DELETE_FLAG = true;
                    }
                    //ddPswTitle.DELETE_FLAG = true;
                    cate.UPDATED_DATE = DateTime.Now;
                    cate.UPDATED_BY = userInformation.UserName;
                    DB.SubmitChanges();
                    return true;
                }
                else if (cate == null)
                {
                    return false;
                }
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                // DB.PSW_WAR_TITLE.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, ddPswTitle);

                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);

            }
            catch (Exception ex)
            {
                ex.LogException();
                DB.DDCATE_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, cate);

            }
            return true;
        }

        public bool CategoryAddDuplicate(CategoryMaterModel cate)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from c in DB.DDCATE_MAST
                                  where c.CATEGORY == cate.Category && c.CATE_CODE != cate.CateCode.ToString()
                                  //&& ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                  select new { c.CATEGORY }).ToList());
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                        return false;
                }
                else
                {
                    return true;
                }
                return true;
            }
            catch (Exception e)
            {
                throw e.LogException();
            }
        }

        public bool CategoryEditDuplicate(CategoryMaterModel cate)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from c in DB.DDCATE_MAST
                                  where c.CATE_CODE != Convert.ToString(cate.CateCode) && c.CATEGORY == cate.Category
                                  //&& ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                                  select new { c.CATEGORY }).ToList());
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                        return true;
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                throw e.LogException();
            }
        }
    }
}
