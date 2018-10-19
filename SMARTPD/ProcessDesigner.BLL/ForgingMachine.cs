using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.BLL
{
    public class ForgingMachine : Essential
    {
        public ForgingMachine(UserInformation userInformation)
        {
            try
            {
                this.userInformation = userInformation;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public List<DDFORGING_MAC> GetEntitiesByCode(DDFORGING_MAC paramEntity = null)
        {

            List<DDFORGING_MAC> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.COST_CENT_CODE.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.DDFORGING_MAC
                                 where Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false && row.COST_CENT_CODE == paramEntity.COST_CENT_CODE
                                 select row).ToList<DDFORGING_MAC>();
                }
                else
                {

                    lstEntity = (from row in DB.DDFORGING_MAC
                                 where Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false
                                 select row).ToList<DDFORGING_MAC>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public DDFORGING_MAC GetEntityByCode(DDFORGING_MAC paramEntity = null)
        {

            DDFORGING_MAC lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return lstEntity;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.COST_CENT_CODE.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.DDFORGING_MAC
                                 where Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false && row.COST_CENT_CODE == paramEntity.COST_CENT_CODE
                                 select row).SingleOrDefault<DDFORGING_MAC>();
                }
                else
                {
                    lstEntity = (from row in DB.DDFORGING_MAC
                                 where Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false
                                 select row).SingleOrDefault<DDFORGING_MAC>();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return lstEntity;
        }

        public ObservableCollection<ForgingMachineTypes> GetForgingMachineTypesByID(ForgingMachineTypes paramEntity = null)
        {

            ObservableCollection<ForgingMachineTypes> observableCollection = null;
            List<ForgingMachineTypes> lstEntity = null;
            try
            {
                if (!DB.IsNotNullOrEmpty()) return observableCollection;
                if (paramEntity.IsNotNullOrEmpty() && paramEntity.IDPK.IsNotNullOrEmpty())
                {
                    lstEntity = (from row in DB.ForgingMachineTypes
                                 where row.IDPK == paramEntity.IDPK
                                 select row).ToList<ForgingMachineTypes>();
                }
                else
                {

                    lstEntity = (from row in DB.ForgingMachineTypes
                                 select row).ToList<ForgingMachineTypes>();
                }

                if (lstEntity.IsNotNullOrEmpty())
                {
                    observableCollection = new ObservableCollection<ForgingMachineTypes>();
                    foreach (ForgingMachineTypes item in lstEntity)
                    {
                        observableCollection.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return observableCollection;
        }

        public bool Insert<T>(List<T> entities)
        {
            bool returnValue = false;
            if (!DB.IsNotNullOrEmpty()) return returnValue;


            foreach (T entity in entities)
            {

                if ((entity as DDFORGING_MAC).IsNotNullOrEmpty())
                {
                    DDFORGING_MAC obj = entity as DDFORGING_MAC;
                    try
                    {
                        obj.ROWID = Guid.NewGuid();
                        obj.DELETE_FLAG = false;
                        obj.ENTERED_BY = userName;
                        obj.ENTERED_DATE = serverDateTime;

                        DB.DDFORGING_MAC.InsertOnSubmit(obj);
                        DB.SubmitChanges();
                    }
                    catch (ChangeConflictException)
                    {
                        foreach (ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(RefreshMode.KeepChanges);
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.DDFORGING_MAC.DeleteOnSubmit(obj);
                        
                    }
                }


            }

            returnValue = true;

            return returnValue;
        }

        public bool Update<T>(List<T> entities)
        {
            bool returnValue = false;
            if (!DB.IsNotNullOrEmpty()) return returnValue;


            foreach (T entity in entities)
            {

                if ((entity as DDFORGING_MAC).IsNotNullOrEmpty())
                {
                    DDFORGING_MAC obj = null;
                    DDFORGING_MAC activeEntity = (entity as DDFORGING_MAC);
                    try
                    {
                        obj = (from row in DB.DDFORGING_MAC
                               where row.COST_CENT_CODE == activeEntity.COST_CENT_CODE
                               select row).SingleOrDefault<DDFORGING_MAC>();

                        if (obj.IsNotNullOrEmpty())
                        {
                            obj.MANUFACTURER = activeEntity.MANUFACTURER;
                            obj.MACHINE_TYPE = activeEntity.MACHINE_TYPE;
                            obj.NO_OF_STATIONS = activeEntity.NO_OF_STATIONS;
                            obj.TONNAGE = activeEntity.TONNAGE;
                            obj.MAX_PROD_DIA = activeEntity.MAX_PROD_DIA;
                            obj.MIN_PROD_DIA = activeEntity.MIN_PROD_DIA;
                            obj.MAX_PROD_LEN = activeEntity.MAX_PROD_LEN;
                            obj.MIN_PROD_LEN = activeEntity.MIN_PROD_LEN;
                            obj.CUTTER_TYPE = activeEntity.CUTTER_TYPE;
                            obj.MAX_CUTOFF_DIA = activeEntity.MAX_CUTOFF_DIA;
                            obj.MIN_CUTOFF_LEN = activeEntity.MIN_CUTOFF_LEN;
                            obj.MAX_CUTOFF_LEN = activeEntity.MAX_CUTOFF_LEN;
                            obj.CUTTER_OD = activeEntity.CUTTER_OD;
                            obj.CUTTER_TICKNESS = activeEntity.CUTTER_TICKNESS;
                            obj.QUILL_CUTTER_DIST = activeEntity.QUILL_CUTTER_DIST;
                            obj.QUILL_TYPE = activeEntity.QUILL_TYPE;
                            obj.QUILL_OD = activeEntity.QUILL_OD;
                            obj.QUILL_THICKNESS = activeEntity.QUILL_THICKNESS;
                            obj.MAX_BUTTON_DIA = activeEntity.MAX_BUTTON_DIA;
                            obj.MAX_KNOCK_OUT = activeEntity.MAX_KNOCK_OUT;
                            obj.PKO_AVAILABLE = activeEntity.PKO_AVAILABLE;
                            obj.PUNCH_DIA_1 = activeEntity.PUNCH_DIA_1;
                            obj.PUCH_LEN_1 = activeEntity.PUCH_LEN_1;
                            obj.PUCH_DIA_2 = activeEntity.PUCH_DIA_2;
                            obj.PUCH_LEN_2 = activeEntity.PUCH_LEN_2;
                            obj.PUCH_DIA_3 = activeEntity.PUCH_DIA_3;
                            obj.PUCH_LEN_3 = activeEntity.PUCH_LEN_3;
                            obj.PUCH_DIA_4 = activeEntity.PUCH_DIA_4;
                            obj.PUCH_LEN_4 = activeEntity.PUCH_LEN_4;
                            obj.FINGER_MECHANISM = activeEntity.FINGER_MECHANISM;
                            obj.TRANS_FINGER_FIX_DIM1 = activeEntity.TRANS_FINGER_FIX_DIM1;
                            obj.TRANS_FINGER_FIX_DIM2 = activeEntity.TRANS_FINGER_FIX_DIM2;
                            obj.TRANS_FINGER_FIX_DIM3 = activeEntity.TRANS_FINGER_FIX_DIM3;
                            obj.TRANS_FINGER_FIX_DIM4 = activeEntity.TRANS_FINGER_FIX_DIM4;
                            obj.CONE_TOOL_PIN_REF = activeEntity.CONE_TOOL_PIN_REF;
                            obj.DIE_PITCH = activeEntity.DIE_PITCH;
                            obj.MAX_PUCH_KICKOUT = activeEntity.MAX_PUCH_KICKOUT;
                            obj.PUNCH_CLOSED_DIM = activeEntity.PUNCH_CLOSED_DIM;
                            obj.PUCH_OPEN_DIM = activeEntity.PUCH_OPEN_DIM;
                            obj.DIE_DIA1 = activeEntity.DIE_DIA1;
                            obj.DIE_DIA2 = activeEntity.DIE_DIA2;
                            obj.DIE_DIA3 = activeEntity.DIE_DIA3;
                            obj.DIE_DIA4 = activeEntity.DIE_DIA4;
                            obj.DIE_LEN1 = activeEntity.DIE_LEN1;
                            obj.DIE_LEN2 = activeEntity.DIE_LEN2;
                            obj.DIE_LEN3 = activeEntity.DIE_LEN3;
                            obj.DIE_LEN4 = activeEntity.DIE_LEN4;
                            obj.MOTOR_POWER = activeEntity.MOTOR_POWER;
                            obj.ROWID = activeEntity.ROWID;

                            obj.DELETE_FLAG = false;
                            obj.UPDATED_BY = userName;
                            obj.UPDATED_DATE = serverDateTime;

                            DB.SubmitChanges();
                            returnValue = true;
                        }
                        else
                        {
                            returnValue = Insert<DDFORGING_MAC>(new List<DDFORGING_MAC>() { activeEntity });
                        }
                    }
                    catch (ChangeConflictException)
                    {
                        foreach (ObjectChangeConflict conflict in DB.ChangeConflicts)
                        {
                            conflict.Resolve(RefreshMode.KeepChanges);
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        DB.DDFORGING_MAC.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, obj);
                    }
                }

            }

            return returnValue;
        }


    }
}
