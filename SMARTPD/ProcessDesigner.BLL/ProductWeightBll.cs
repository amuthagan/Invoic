using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System.Data;

namespace ProcessDesigner.BLL
{
    public class ProductWeightBll : Essential
    {
        public ProductWeightBll(UserInformation userInfo)
        {
            this.userInformation = userInfo;
        }

        public bool GetShapeDetails(ProductWeightModel productweight)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ToDataTable((from o in DB.DDSHAPE_MAST
                                  select new { o.SHAPE_CODE, o.SHAPE_NAME, o.ROWID }).ToList());
                if (dt != null)
                {

                    productweight.DVShape = dt.DefaultView;
                }
                else
                {
                    productweight.DVShape = null;
                }

                dt = ToDataTable((from o in DB.DDSHAPE_DETAILS
                                  where o.CI_REFERENCE == productweight.CIreference && o.WEIGHT_OPTION == productweight.WeightOption
                                  select new { o.CI_REFERENCE, o.WEIGHT_OPTION, o.SNO, o.SHAPE_CODE, o.HEAD1, o.VAL1, o.HEAD2, o.VAL2, o.HEAD3, o.VAL3, o.VOLUME, o.SIGN, o.ROWID }).ToList());
                if (dt != null)
                {
                    productweight.DVShapeDetails = dt.DefaultView;
                    productweight.DVShapeDetails.AddNew();
                    productweight.DTDeletedRecords = dt.Clone();
                }
                else
                {
                    productweight.DVShapeDetails = null;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void CopyProcess(ProductWeightModel productweight)
        {
            try
            {
                string varCopy = (productweight.WeightOption == "C") ? "F" : "C";

                int cnt = (from o in DB.DDSHAPE_DETAILS
                           where o.CI_REFERENCE == productweight.CIreference && o.WEIGHT_OPTION == productweight.WeightOption
                           select o).Count();
                if (cnt == 0)
                {
                    List<DDSHAPE_DETAILS> sdetails = (from o in DB.DDSHAPE_DETAILS
                                                      where o.CI_REFERENCE == productweight.CIreference && o.WEIGHT_OPTION == varCopy
                                                      select o).ToList<DDSHAPE_DETAILS>();
                    if (sdetails != null)
                    {
                        foreach (DDSHAPE_DETAILS sdetail in sdetails)
                        {
                            DDSHAPE_DETAILS shapedetail = new DDSHAPE_DETAILS();
                            shapedetail.CI_REFERENCE = productweight.CIreference;
                            shapedetail.WEIGHT_OPTION = productweight.WeightOption;
                            shapedetail.SHAPE_CODE = sdetail.SHAPE_CODE;
                            shapedetail.SNO = GenerateSNO(productweight);
                            shapedetail.HEAD1 = sdetail.HEAD1;
                            shapedetail.HEAD2 = sdetail.HEAD2;
                            shapedetail.HEAD3 = sdetail.HEAD3;
                            shapedetail.VAL1 = sdetail.VAL1;
                            shapedetail.VAL2 = sdetail.VAL2;
                            shapedetail.VAL3 = sdetail.VAL3;
                            shapedetail.VOLUME = sdetail.VOLUME;
                            shapedetail.SIGN = sdetail.SIGN;

                            shapedetail.ROWID = Guid.NewGuid();
                            DB.DDSHAPE_DETAILS.InsertOnSubmit(shapedetail);
                            DB.SubmitChanges();
                        }

                    }
                    sdetails = null;
                }

            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public DDSHAPE_MAST GetShape(string code)
        {
            try
            {

                return (from o in DB.DDSHAPE_MAST
                        where o.SHAPE_CODE == code
                        select o).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public bool UpdateProductWeight(ProductWeightModel productwt)
        {

            bool _status = false;
            productwt.Status = "";

            try
            {

                foreach (DataRowView dr in productwt.DVShapeDetails)
                {
                    if (dr["SHAPE_CODE"].ToString() != "")
                    {
                        try
                        {
                            if (dr["ROWID"].ToString() != "")
                            {
                                DDSHAPE_DETAILS sdetails = (from o in DB.DDSHAPE_DETAILS
                                                            where o.ROWID.ToString() == dr["ROWID"].ToString()
                                                            select o).FirstOrDefault<DDSHAPE_DETAILS>();
                                if (sdetails != null)
                                {
                                    sdetails.CI_REFERENCE = productwt.CIreference;
                                    //sdetails.CIREF_NO_FK = productwt.CIREF_NO_FK;
                                    sdetails.WEIGHT_OPTION = productwt.WeightOption;
                                    sdetails.SHAPE_CODE = dr["SHAPE_CODE"].ToString();
                                    sdetails.SNO = dr["SNO"].ToString().ToDecimalValue();
                                    sdetails.HEAD1 = dr["HEAD1"].ToString();
                                    sdetails.HEAD2 = dr["HEAD2"].ToString();
                                    sdetails.HEAD3 = dr["HEAD3"].ToString();
                                    sdetails.VAL1 = dr["VAL1"].ToString().ToDecimalValue();
                                    sdetails.VAL2 = dr["VAL2"].ToString().ToDecimalValue();
                                    sdetails.VAL3 = dr["VAL3"].ToString().ToDecimalValue();
                                    sdetails.VOLUME = dr["VOLUME"].ToString().ToDecimalValue();
                                    sdetails.SIGN = dr["SIGN"].ToString();
                                    DB.SubmitChanges();
                                    sdetails = null;
                                    productwt.Status = "Record Updated successfully.";
                                }
                            }
                            else
                            {
                                DDSHAPE_DETAILS sdetails = new DDSHAPE_DETAILS();
                                sdetails.CI_REFERENCE = productwt.CIreference;
                                
                                //if (productwt.CIREF_NO_FK > 0)
                                //    sdetails.CIREF_NO_FK = productwt.CIREF_NO_FK;

                                sdetails.WEIGHT_OPTION = productwt.WeightOption;
                                sdetails.SHAPE_CODE = dr["SHAPE_CODE"].ToString();
                                sdetails.SNO = GenerateSNO(productwt);
                                sdetails.HEAD1 = dr["HEAD1"].ToString();
                                sdetails.HEAD2 = dr["HEAD2"].ToString();
                                sdetails.HEAD3 = dr["HEAD3"].ToString();
                                sdetails.VAL1 = dr["VAL1"].ToString().ToDecimalValue();
                                sdetails.VAL2 = dr["VAL2"].ToString().ToDecimalValue();
                                sdetails.VAL3 = dr["VAL3"].ToString().ToDecimalValue();
                                sdetails.VOLUME = dr["VOLUME"].ToString().ToDecimalValue();
                                sdetails.SIGN = dr["SIGN"].ToString();

                                sdetails.ROWID = Guid.NewGuid();
                                DB.DDSHAPE_DETAILS.InsertOnSubmit(sdetails);
                                DB.SubmitChanges();
                                sdetails = null;
                                productwt.Status = "Record Updated successfully.";
                            }

                        }
                        catch (System.Data.Linq.ChangeConflictException)
                        {
                            DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                            productwt.Status = "Record Added successfully.";

                        }
                        catch (Exception ex)
                        {
                            throw ex.LogException();
                        }
                    }
                }

                foreach (DataRow dr in productwt.DTDeletedRecords.Rows)
                {
                    try
                    {

                        if (dr["ROWID"].ToString().Trim() != "")
                        {
                            DDSHAPE_DETAILS sdetails = (from o in DB.DDSHAPE_DETAILS
                                                        where o.ROWID.ToString() == dr["ROWID"].ToString()
                                                        select o).FirstOrDefault<DDSHAPE_DETAILS>();

                            if (sdetails != null)
                            {
                                DB.DDSHAPE_DETAILS.DeleteOnSubmit(sdetails);
                                DB.SubmitChanges();
                            }
                            sdetails = null;
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

                _status = true;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);

            }
            catch (Exception ex)
            {
                DB.Transaction.Rollback();
                throw ex.LogException();
            }
            return _status;
        }

        public decimal GenerateSNO(ProductWeightModel productwt)
        {

            try
            {
                decimal sno = (from o in DB.DDSHAPE_DETAILS where o.CI_REFERENCE == productwt.CIreference && o.WEIGHT_OPTION == productwt.WeightOption select o.SNO).Max();

                return Convert.ToDecimal(sno) + 1;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return 1;

            }
        }
    }
}
