using ProcessDesigner.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Model;
using System.Data;

namespace ProcessDesigner.BLL
{
    public class ExhitbitMaster : Essential
    {
        public ExhitbitMaster(UserInformation userInformation)
        {
            this.userInformation = userInformation;
        }
        public List<EXHIBIT_DOC> getExhibitDocuments()
        {
            return (from e in DB.EXHIBIT_DOC
                    where e.DOC_NAME != "" && e.DELETE_FLAG == false
                    select e).ToList();
        }

        public DataView GetExhibitMaster()
        {
            DataTable dataValue;
            dataValue = ((from c in DB.EXHIBIT_DOC.AsEnumerable()
                          where ((Convert.ToBoolean(Convert.ToInt16(c.DELETE_FLAG)) == false) || (c.DELETE_FLAG == null))
                          orderby c.DOC_NAME ascending
                          select new { DOC_NAME = c.DOC_NAME, EX_NO = c.EX_NO }).ToList().ToDataTable());
            return dataValue.DefaultView;
        }

        public bool updateExhitbitMaster(string exhibitNumber, string exhibitDetails)
        {
           
            EXHIBIT_DOC exhibit = (from e in DB.EXHIBIT_DOC
                                   where e.DOC_NAME == exhibitDetails
                                       && ((Convert.ToBoolean(Convert.ToInt16(e.DELETE_FLAG)) == false) || (e.DELETE_FLAG == null))
                                   select e).FirstOrDefault<EXHIBIT_DOC>();
            try
            {
                if (exhibit != null)
                {
                    exhibit.EX_NO = exhibitNumber;
                    exhibit.UPDATED_DATE = DateTime.Now;
                    exhibit.UPDATED_BY = userInformation.UserName;
                    exhibit.DELETE_FLAG = false;
                    //exhibit.DOC_NAME = exhibitDetails;
                    DB.SubmitChanges();
                }


                exhibit = null;

                //string query = "UPDATE EXHIBIT_DOC set EX_NO='" + exhibitNumber + "' , UPDATED_DATE = GETDATE() , UPDATED_BY ='" + userInformation.UserName + "' WHERE DOC_NAME='" + exhibitDetails + "'";
                //IEnumerable<int> result = DB.ExecuteQuery<int>(query);
                //DB.SubmitChanges();
                return true;
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
                return false;
                
            }
            catch (Exception ex)
            {
                ex.LogException();
                DB.EXHIBIT_DOC.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, exhibit);

                
            }
            return false;

        }
    }
}
