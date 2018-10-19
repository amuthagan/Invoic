using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ProcessDesigner.Common;
using ProcessDesigner.Model;

namespace ProcessDesigner.BLL
{
    public class ProductCategoryBll : Essential
    {

        public ProductCategoryBll(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }
        public bool IsSubTypeDuplicate(string type)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from a in DB.FASTENERS_MASTER
                                  where a.SUBTYPE == type.Trim()
                                  select new { a.SUBTYPE }).ToList());
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                        return true;
                }
                else
                {
                    return false;
                }
                return false;
            }
            catch (Exception e)
            {
                throw e.LogException();
            }
        }
        public bool GetProductCategory(ProductCategoryModel productcat)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.FASTENERS_MASTER
                                  where o.TYPE == null && o.SUBTYPE == null && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  select o).ToList());
                if (dt != null)
                {
                    productcat.DVCategory = dt.DefaultView;
                    productcat.DTDeletedRecords = dt.Clone();
                }
                else
                {
                    productcat.DVCategory = null;
                }

                dt = ToDataTable((from o in DB.FASTENERS_MASTER
                                  where 1 == 2
                                  select o).ToList());
                if (dt != null)
                {
                    productcat.DVType = dt.DefaultView;

                    ///SRS:REQ_PD_71 to 74 – Product code removed from Product category master
                    ///Date: 01/11/2016

                    //productcat.DVType.AddNew();

                    DataRowView drv = productcat.DVType.AddNew();
                    drv.BeginEdit();
                    drv["PRD_CODE"] = Guid.NewGuid();
                    drv.EndEdit();
                    /// End

                    productcat.DVAllSubType = dt.Clone().DefaultView;


                }
                else
                {
                    productcat.DVType = null;
                    productcat.DVAllSubType = null;
                }

                dt = ToDataTable((from o in DB.FASTENERS_MASTER
                                  where 1 == 2
                                  select o).ToList());
                if (dt != null)
                {

                    productcat.DVSubType = dt.DefaultView;

                    ///SRS:REQ_PD_71 to 74 – Product code removed from Product category master
                    ///Date: 01/11/2016

                    // productcat.DVSubType.AddNew();

                    DataRowView drv = productcat.DVSubType.AddNew();
                    drv.BeginEdit();
                    drv["PRD_CODE"] = Guid.NewGuid();
                    drv.EndEdit();
                    /// End
                    /// 
                    productcat.DVAllLinkSubType = dt.Clone().DefaultView;
                }
                else
                {
                    productcat.DVSubType = null;
                }


                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool GetProductTypeSubType(ProductCategoryModel productcat)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.FASTENERS_MASTER
                                  where o.TYPE != null && o.SUBTYPE == null && o.PRODUCT_CATEGORY == productcat.PRODUCT_CATEGORY
                                  && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  select o).ToList());
                if (dt != null)
                {

                    productcat.DVType = dt.DefaultView;
                    if (productcat.Mode == "Add")
                    {
                        ///SRS:REQ_PD_71 to 74 – Product code removed from Product category master
                        ///Date: 01/11/2016

                        //productcat.DVType.AddNew();
                        DataRowView drv = productcat.DVType.AddNew();
                        drv.BeginEdit();
                        drv["PRD_CODE"] = Guid.NewGuid();
                        drv.EndEdit();
                        /// End
                    }

                }
                else
                {
                    productcat.DVType = null;
                }

                var innerQuery = (from o in DB.FASTENERS_MASTER
                                  where o.TYPE != null && o.SUBTYPE == null && o.PRODUCT_CATEGORY == productcat.PRODUCT_CATEGORY
                                  && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  select o.TYPE).Distinct();

                dt = ToDataTable((from row in DB.FASTENERS_MASTER
                                  where innerQuery.Contains(row.TYPE) && row.SUBTYPE != null
                                  && ((Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false) || (row.DELETE_FLAG == null))
                                  select row).ToList());

                if (dt != null)
                {

                    productcat.DVAllSubType = dt.DefaultView;
                    productcat.DVAllSubType.RowFilter = "1=2";
                }
                else
                {
                    productcat.DVAllSubType = null;
                }


                dt = ToDataTable((from o in DB.FASTENERS_MASTER
                                  where o.TYPE == null && o.SUBTYPE != null && o.PRODUCT_CATEGORY == productcat.PRODUCT_CATEGORY
                                  && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                  select o).ToList());
                if (dt != null)
                {

                    productcat.DVSubType = dt.DefaultView;
                    if (productcat.Mode == "Add")
                    {
                        ///SRS:REQ_PD_71 to 74 – Product code removed from Product category master
                        ///Date: 01/11/2016

                        // productcat.DVSubType.AddNew();
                        DataRowView drv = productcat.DVSubType.AddNew();
                        drv.BeginEdit();
                        drv["PRD_CODE"] = Guid.NewGuid();
                        drv.EndEdit();
                        /// End
                        /// 
                    }
                }
                else
                {
                    productcat.DVSubType = null;
                }

                var innerQuery1 = (from o in DB.FASTENERS_MASTER
                                   where o.TYPE == null && o.SUBTYPE != null && o.PRODUCT_CATEGORY == productcat.PRODUCT_CATEGORY
                                   && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                   select o.SUBTYPE).Distinct();

                dt = ToDataTable((from row in DB.FASTENERS_MASTER
                                  where innerQuery1.Contains(row.TYPE) && row.SUBTYPE != null
                                  && ((Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false) || (row.DELETE_FLAG == null))
                                  select row).ToList());

                if (dt != null)
                {

                    productcat.DVAllLinkSubType = dt.DefaultView;
                    productcat.DVAllLinkSubType.RowFilter = "1=2";
                }
                else
                {
                    productcat.DVAllLinkSubType = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private string prdCode = "";
        public bool UpdateProductCategory(ProductCategoryModel productcat)
        {

            bool _status = false;
            productcat.Status = "";
            //string GetQuery = "select 'PRD'+REPLICATE('0', 4 - LEN(isnull(max(CAST(substring(f.PRD_CODE,4,4) as int)),0)+1)) + CAST(isnull(max(CAST(isnull(substring(f.PRD_CODE,4,4),0) as int)),0)+1 AS varchar) as 'PRD_CODE' from FASTENERS_MASTER f where ISNUMERIC(substring(f.PRD_CODE,4,4)) = 1 and substring(f.PRD_CODE,0,3)='PRD'";

            try
            {

                if (productcat.Mode == "Add")
                {
                    prdCode = GenerateProductCode();
                    if (productcat.ROWID == "" && prdCode.Trim() != "" && productcat.PRODUCT_CATEGORY.Trim() != string.Empty)
                    {
                        FASTENERS_MASTER fastnersM = (from o in DB.FASTENERS_MASTER
                                                      where o.PRD_CODE == prdCode
                                                      select o).FirstOrDefault<FASTENERS_MASTER>();
                        try
                        {
                            if (fastnersM == null)
                            {
                                fastnersM = new FASTENERS_MASTER();
                                fastnersM.PRD_CODE = prdCode.Trim();
                                fastnersM.PRODUCT_CATEGORY = productcat.PRODUCT_CATEGORY;
                                fastnersM.ENTERED_BY = userInformation.UserName;
                                fastnersM.ENTERED_DATE = userInformation.Dal.ServerDateTime;

                                fastnersM.ROWID = Guid.NewGuid();
                                DB.FASTENERS_MASTER.InsertOnSubmit(fastnersM);
                                DB.SubmitChanges();
                                productcat.Status = "Record Added successfully.";
                            }
                            fastnersM = null;
                        }
                        catch (System.Data.Linq.ChangeConflictException)
                        {
                            DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                            productcat.Status = "Record Added successfully.";
                        }
                        catch (Exception ex)
                        {
                            DB.FASTENERS_MASTER.DeleteOnSubmit(fastnersM);
                            throw ex.LogException();
                        }
                    }


                    foreach (DataRowView dr in productcat.DVType)
                    {
                        prdCode = GenerateProductCode();
                        if (dr["ROWID"].ToString() == string.Empty && prdCode != "" && dr["TYPE"].ToString().Trim() != string.Empty)
                        {
                            FASTENERS_MASTER fastnersMType = (from o in DB.FASTENERS_MASTER
                                                              where o.PRD_CODE == prdCode.Trim()
                                                              select o).FirstOrDefault<FASTENERS_MASTER>();
                            try
                            {
                                if (fastnersMType == null)
                                {
                                    fastnersMType = new FASTENERS_MASTER();
                                    fastnersMType.PRD_CODE = prdCode.Trim().ToUpper();
                                    fastnersMType.PRODUCT_CATEGORY = productcat.PRODUCT_CATEGORY;
                                    fastnersMType.TYPE = dr["TYPE"].ToString();
                                    fastnersMType.ENTERED_BY = userInformation.UserName;
                                    fastnersMType.ENTERED_DATE = userInformation.Dal.ServerDateTime;

                                    fastnersMType.ROWID = Guid.NewGuid();
                                    DB.FASTENERS_MASTER.InsertOnSubmit(fastnersMType);
                                    DB.SubmitChanges();
                                    productcat.Status = "Record Added successfully.";
                                }
                                fastnersMType = null;
                            }
                            catch (System.Data.Linq.ChangeConflictException)
                            {
                                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                                productcat.Status = "Record Added successfully.";
                            }
                            catch (Exception ex)
                            {
                                DB.FASTENERS_MASTER.DeleteOnSubmit(fastnersMType);
                                throw ex.LogException();
                            }
                        }
                    }

                    foreach (DataRowView dr in productcat.DVSubType)
                    {
                        prdCode = GenerateProductCode();
                        if (dr["ROWID"].ToString() == string.Empty && prdCode.Trim() != "" && dr["SUBTYPE"].ToString().Trim() != string.Empty)
                        {
                            FASTENERS_MASTER fastnersMSType = (from o in DB.FASTENERS_MASTER
                                                               where o.PRD_CODE == prdCode.Trim()
                                                               select o).FirstOrDefault<FASTENERS_MASTER>();
                            try
                            {
                                if (fastnersMSType == null)
                                {
                                    fastnersMSType = new FASTENERS_MASTER();
                                    fastnersMSType.PRD_CODE = prdCode.Trim().ToUpper();
                                    fastnersMSType.PRODUCT_CATEGORY = productcat.PRODUCT_CATEGORY;
                                    fastnersMSType.SUBTYPE = dr["SUBTYPE"].ToString();
                                    fastnersMSType.ENTERED_BY = userInformation.UserName;
                                    fastnersMSType.ENTERED_DATE = userInformation.Dal.ServerDateTime;

                                    fastnersMSType.ROWID = Guid.NewGuid();
                                    DB.FASTENERS_MASTER.InsertOnSubmit(fastnersMSType);
                                    DB.SubmitChanges();
                                    productcat.Status = "Record Added successfully.";
                                }
                                fastnersMSType = null;
                            }
                            catch (System.Data.Linq.ChangeConflictException)
                            {
                                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                                productcat.Status = "Record Added successfully.";
                            }
                            catch (Exception ex)
                            {
                                DB.FASTENERS_MASTER.DeleteOnSubmit(fastnersMSType);
                                ex.LogException();
                            }
                        }
                    }

                }
                else if (productcat.Mode == "Edit")
                {
                    if (productcat.ROWID != string.Empty)
                    {
                        FASTENERS_MASTER fastnersM = (from o in DB.FASTENERS_MASTER
                                                      where o.PRD_CODE == productcat.PRD_CODE
                                                      select o).FirstOrDefault<FASTENERS_MASTER>();

                        if (fastnersM != null)
                        {
                            try
                            {
                                if (fastnersM.PRODUCT_CATEGORY != productcat.PRODUCT_CATEGORY)
                                {
                                    fastnersM.PRD_CODE = productcat.PRD_CODE.ToUpper();
                                    fastnersM.PRODUCT_CATEGORY = productcat.PRODUCT_CATEGORY;
                                    fastnersM.UPDATED_BY = userInformation.UserName;
                                    fastnersM.UPDATED_DATE = userInformation.Dal.ServerDateTime;
                                    DB.SubmitChanges();
                                    fastnersM = null;
                                    productcat.Status = "Record Updated successfully.";
                                }
                            }
                            catch (System.Data.Linq.ChangeConflictException)
                            {
                                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                                productcat.Status = "Record Updated successfully.";
                            }
                            catch (Exception ex)
                            {
                                DB.FASTENERS_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, fastnersM);
                                ex.LogException();
                            }
                        }

                        fastnersM = null;
                    }


                    foreach (DataRowView dr in productcat.DVType)
                    {

                        if (dr["ROWID"].ToString() != string.Empty && dr["TYPE"].ToString().Trim() != string.Empty)
                        {

                            FASTENERS_MASTER fastnersMType = (from o in DB.FASTENERS_MASTER
                                                              where o.PRD_CODE == dr["PRD_CODE"].ToString()
                                                              select o).FirstOrDefault<FASTENERS_MASTER>();
                            try
                            {
                                if (fastnersMType != null)
                                {
                                    if (fastnersMType.PRODUCT_CATEGORY.Trim() != productcat.PRODUCT_CATEGORY.Trim() || fastnersMType.TYPE != dr["TYPE"].ToString())
                                    {
                                        fastnersMType.PRD_CODE = dr["PRD_CODE"].ToString().Trim().ToUpper();
                                        fastnersMType.PRODUCT_CATEGORY = productcat.PRODUCT_CATEGORY;
                                        fastnersMType.TYPE = dr["TYPE"].ToString();
                                        fastnersMType.UPDATED_BY = userInformation.UserName;
                                        fastnersMType.UPDATED_DATE = userInformation.Dal.ServerDateTime;
                                        DB.SubmitChanges();
                                        fastnersMType = null;
                                        productcat.Status = "Record Updated successfully.";
                                    }
                                }
                                fastnersMType = null;
                            }
                            catch (System.Data.Linq.ChangeConflictException)
                            {
                                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                                productcat.Status = "Record Updated successfully.";
                            }
                            catch (Exception ex)
                            {
                                DB.FASTENERS_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, fastnersMType);
                                ex.LogException();
                            }
                        }

                    }

                    foreach (DataRowView dr in productcat.DVSubType)
                    {

                        if (dr["ROWID"].ToString() != string.Empty && dr["SUBTYPE"].ToString().Trim() != string.Empty)
                        {

                            FASTENERS_MASTER fastnersMSType = (from o in DB.FASTENERS_MASTER
                                                               where o.PRD_CODE == dr["PRD_CODE"].ToString()
                                                               select o).FirstOrDefault<FASTENERS_MASTER>();
                            try
                            {
                                if (fastnersMSType != null)
                                {
                                    if (fastnersMSType.PRODUCT_CATEGORY.Trim() != productcat.PRODUCT_CATEGORY.Trim() || fastnersMSType.SUBTYPE != dr["SUBTYPE"].ToString())
                                    {
                                        fastnersMSType.PRD_CODE = dr["PRD_CODE"].ToString().Trim().ToUpper();
                                        fastnersMSType.PRODUCT_CATEGORY = productcat.PRODUCT_CATEGORY;
                                        fastnersMSType.SUBTYPE = dr["SUBTYPE"].ToString();
                                        fastnersMSType.UPDATED_BY = userInformation.UserName;
                                        fastnersMSType.UPDATED_DATE = userInformation.Dal.ServerDateTime;
                                        DB.SubmitChanges();
                                        fastnersMSType = null;
                                        productcat.Status = "Record Updated successfully.";
                                    }
                                }
                                fastnersMSType = null;
                            }
                            catch (System.Data.Linq.ChangeConflictException)
                            {
                                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                                productcat.Status = "Record Updated successfully.";
                            }
                            catch (Exception ex)
                            {
                                DB.FASTENERS_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, fastnersMSType);
                                ex.LogException();
                            }

                        }

                    }
                }


                UpdateProductSubType(productcat);

                DeleteProducts(productcat.DTDeletedRecords);

                _status = true;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                _status = false;
                throw ex.LogException();
            }
            return _status;
        }


        public bool UpdateProductSubType(ProductCategoryModel productcat)
        {

            bool _status = false;
            productcat.Status = "";

            try
            {
                productcat.DVAllSubType.Table.AcceptChanges();
                foreach (DataRow dr in productcat.DVAllSubType.Table.Rows)
                {
                    if (productcat.Mode == "Add")
                    {
                        prdCode = GenerateProductCode();
                        if (dr["ROWID"].ToString() == string.Empty && prdCode.Trim() != "" && dr["SUBTYPE"].ToString().Trim() != "")
                        {
                            FASTENERS_MASTER fastnersMType = (from o in DB.FASTENERS_MASTER
                                                              where o.PRD_CODE == prdCode.Trim()
                                                              select o).FirstOrDefault<FASTENERS_MASTER>();
                            try
                            {

                                if (fastnersMType == null)
                                {
                                    fastnersMType = new FASTENERS_MASTER();
                                    fastnersMType.PRD_CODE = prdCode.Trim().ToUpper();
                                    fastnersMType.PRODUCT_CATEGORY = dr["PRODUCT_CATEGORY"].ToString().Trim();
                                    fastnersMType.TYPE = dr["TYPE"].ToString().Trim();
                                    fastnersMType.SUBTYPE = dr["SUBTYPE"].ToString().Trim();
                                    fastnersMType.ENTERED_BY = userInformation.UserName;
                                    fastnersMType.ENTERED_DATE = userInformation.Dal.ServerDateTime;

                                    fastnersMType.ROWID = Guid.NewGuid();
                                    DB.FASTENERS_MASTER.InsertOnSubmit(fastnersMType);
                                    DB.SubmitChanges();
                                    productcat.Status = "Record Added successfully.";
                                }
                                fastnersMType = null;

                            }
                            catch (System.Data.Linq.ChangeConflictException)
                            {
                                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                                productcat.Status = "Record Added successfully.";
                            }
                            catch (Exception ex)
                            {
                                DB.FASTENERS_MASTER.DeleteOnSubmit(fastnersMType);
                                ex.LogException();
                            }
                        }
                    }
                    else if (productcat.Mode == "Edit")
                    {
                        if (dr["ROWID"].ToString() != string.Empty && dr["PRD_CODE"].ToString().Trim() != "" && dr["SUBTYPE"].ToString().Trim() != "")
                        {


                            FASTENERS_MASTER fastnersMType = (from o in DB.FASTENERS_MASTER
                                                              where o.PRD_CODE == dr["PRD_CODE"].ToString().Trim()
                                                              select o).FirstOrDefault<FASTENERS_MASTER>();
                            try
                            {
                                if (fastnersMType != null)
                                {
                                    if (fastnersMType.PRODUCT_CATEGORY.Trim() != dr["PRODUCT_CATEGORY"].ToString().Trim() || fastnersMType.TYPE.Trim() != dr["TYPE"].ToString().Trim() || fastnersMType.SUBTYPE != dr["SUBTYPE"].ToString())
                                    {
                                        fastnersMType.PRD_CODE = dr["PRD_CODE"].ToString();
                                        fastnersMType.PRODUCT_CATEGORY = dr["PRODUCT_CATEGORY"].ToString().Trim();
                                        fastnersMType.TYPE = dr["TYPE"].ToString();
                                        fastnersMType.SUBTYPE = dr["SUBTYPE"].ToString();
                                        fastnersMType.UPDATED_BY = userInformation.UserName;
                                        fastnersMType.UPDATED_DATE = userInformation.Dal.ServerDateTime;
                                        DB.SubmitChanges();
                                        productcat.Status = "Record Updated successfully.";
                                    }
                                }
                                fastnersMType = null;
                            }
                            catch (System.Data.Linq.ChangeConflictException)
                            {
                                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                                productcat.Status = "Record Updated successfully.";
                            }
                            catch (Exception ex)
                            {
                                DB.FASTENERS_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, fastnersMType);
                                ex.LogException();
                            }
                        }
                    }
                }


                productcat.DVAllLinkSubType.Table.AcceptChanges();
                foreach (DataRow dr in productcat.DVAllLinkSubType.Table.Rows)
                {
                    if (productcat.Mode == "Add")
                    {
                        prdCode = GenerateProductCode();

                        if (dr["ROWID"].ToString() == string.Empty && prdCode.Trim() != "" && dr["SUBTYPE"].ToString().Trim() != "")
                        {
                            FASTENERS_MASTER fastnersMType = (from o in DB.FASTENERS_MASTER
                                                              where o.PRD_CODE == prdCode.Trim()
                                                              select o).FirstOrDefault<FASTENERS_MASTER>();
                            try
                            {

                                if (fastnersMType == null)
                                {
                                    fastnersMType = new FASTENERS_MASTER();
                                    fastnersMType.PRD_CODE = prdCode.Trim().ToUpper();
                                    fastnersMType.PRODUCT_CATEGORY = dr["PRODUCT_CATEGORY"].ToString().Trim();
                                    fastnersMType.TYPE = dr["TYPE"].ToString().Trim();
                                    fastnersMType.SUBTYPE = dr["SUBTYPE"].ToString().Trim();
                                    fastnersMType.ENTERED_BY = userInformation.UserName;
                                    fastnersMType.ENTERED_DATE = userInformation.Dal.ServerDateTime;

                                    fastnersMType.ROWID = Guid.NewGuid();
                                    DB.FASTENERS_MASTER.InsertOnSubmit(fastnersMType);
                                    DB.SubmitChanges();
                                    productcat.Status = "Record Added successfully.";
                                }
                                fastnersMType = null;

                            }
                            catch (System.Data.Linq.ChangeConflictException)
                            {
                                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                                productcat.Status = "Record Added successfully.";
                            }
                            catch (Exception ex)
                            {
                                DB.FASTENERS_MASTER.DeleteOnSubmit(fastnersMType);
                                ex.LogException();
                            }
                        }
                    }
                    else if (productcat.Mode == "Edit")
                    {
                        if (dr["ROWID"].ToString() != string.Empty && dr["PRD_CODE"].ToString().Trim() != "" && dr["SUBTYPE"].ToString().Trim() != "")
                        {


                            FASTENERS_MASTER fastnersMType = (from o in DB.FASTENERS_MASTER
                                                              where o.PRD_CODE == dr["PRD_CODE"].ToString().Trim()
                                                              select o).FirstOrDefault<FASTENERS_MASTER>();
                            try
                            {
                                if (fastnersMType != null)
                                {
                                    if (fastnersMType.PRODUCT_CATEGORY.Trim() != dr["PRODUCT_CATEGORY"].ToString().Trim() || fastnersMType.TYPE.Trim() != dr["TYPE"].ToString().Trim() || fastnersMType.SUBTYPE != dr["SUBTYPE"].ToString())
                                    {
                                        fastnersMType.PRD_CODE = dr["PRD_CODE"].ToString();
                                        fastnersMType.PRODUCT_CATEGORY = dr["PRODUCT_CATEGORY"].ToString().Trim();
                                        fastnersMType.TYPE = dr["TYPE"].ToString();
                                        fastnersMType.SUBTYPE = dr["SUBTYPE"].ToString();
                                        fastnersMType.UPDATED_BY = userInformation.UserName;
                                        fastnersMType.UPDATED_DATE = userInformation.Dal.ServerDateTime;
                                        DB.SubmitChanges();
                                        productcat.Status = "Record Updated successfully.";
                                    }
                                }
                                fastnersMType = null;
                            }
                            catch (System.Data.Linq.ChangeConflictException)
                            {
                                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                                productcat.Status = "Record Updated successfully.";
                            }
                            catch (Exception ex)
                            {
                                DB.FASTENERS_MASTER.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, fastnersMType);
                                ex.LogException();
                            }
                        }
                    }
                }

                _status = true;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                _status = false;
                throw ex.LogException();
            }
            return _status;
        }

        public bool DeleteProducts(DataTable dvRecords)
        {

            bool _status = false;

            foreach (DataRow dr in dvRecords.Rows)
            {
                try
                {

                    if (dr["PRD_CODE"].ToString().Trim() != "")
                    {
                        FASTENERS_MASTER fastnersM = (from o in DB.FASTENERS_MASTER
                                                      where o.PRD_CODE == dr["PRD_CODE"].ToString().Trim()
                                                      select o).FirstOrDefault<FASTENERS_MASTER>();

                        if (fastnersM != null)
                        {
                            DB.FASTENERS_MASTER.DeleteOnSubmit(fastnersM);
                            DB.SubmitChanges();
                        }
                        fastnersM = null;
                        _status = true;
                    }
                }
                catch (System.Data.Linq.ChangeConflictException)
                {
                    DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                    _status = true;
                }
                catch (Exception ex)
                {
                    _status = false;
                    throw ex.LogException();
                }
            }
            return _status;
        }


        public string GenerateProductCode()
        {
            string fastMast = "";
            try
            {
                fastMast = (from o in DB.FASTENERS_MASTER where o.PRD_CODE.ToString().StartsWith("PR") select o.PRD_CODE).Max();
                if (fastMast == null) fastMast = "0";
                fastMast = "PR" + (Convert.ToInt32(fastMast.Replace("PR", "").Trim()) + 1).ToString("0000");
                return fastMast;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool CheckProductCategory(ProductCategoryModel productcat)
        {
            try
            {

                int prdcat = (from o in DB.FASTENERS_MASTER
                              where o.PRODUCT_CATEGORY == productcat.PRODUCT_CATEGORY && o.TYPE == null && o.SUBTYPE == null && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                              select o).Count();

                if (prdcat > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public string GetOldCategory(string code)
        {

            FASTENERS_MASTER fastnersM = (from o in DB.FASTENERS_MASTER
                                          where o.PRD_CODE == code && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                          select o).FirstOrDefault<FASTENERS_MASTER>();

            if (fastnersM != null)
            {
                return fastnersM.PRODUCT_CATEGORY.ToString();
            }
            else
            {
                return "";
            }
        }

        public string GetOldType(string code)
        {

            FASTENERS_MASTER fastnersM = (from o in DB.FASTENERS_MASTER
                                          where o.PRD_CODE == code && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                          select o).FirstOrDefault<FASTENERS_MASTER>();

            if (fastnersM != null)
            {
                return fastnersM.TYPE.ToString();
            }
            else
            {
                return "";
            }
        }

        public string GetOldSubType(string code)
        {
            FASTENERS_MASTER fastnersM = (from o in DB.FASTENERS_MASTER
                                          where o.PRD_CODE == code && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                                          select o).FirstOrDefault<FASTENERS_MASTER>();

            if (fastnersM != null)
            {
                return fastnersM.SUBTYPE.ToString();
            }
            else
            {
                return "";
            }
        }

        public bool CheckCodeIsDuplicate(string code)
        {
            try
            {
                int prdcat = (from o in DB.FASTENERS_MASTER
                              where o.PRD_CODE == code
                              select o).Count();
                if (prdcat > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }
        }
    }
}
