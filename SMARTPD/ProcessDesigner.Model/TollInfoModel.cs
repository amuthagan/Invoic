using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Model;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ProcessDesigner.Model
{
    public class TollInfoModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DataView _dtProductSearchDetails;
        public DataView ProductSearchDetails
        {
            get { return _dtProductSearchDetails; }
            set
            {
                _dtProductSearchDetails = value;
                NotifyPropertyChanged("ProductSearchDetails");
            }
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        private string _family_cd;

        private string _tool_cd;

        private string _description;

        private string _material_size;

        private string _heat_treatment;

        private System.Nullable<decimal> _program_no;

        private string _p001;

        private string _p002;

        private string _p003;

        private string _p004;

        private string _p005;

        private string _p006;

        private string _p007;

        private string _p008;

        private string _p009;

        private string _p010;

        private string _p011;

        private string _p012;

        private string _p013;

        private string _p014;

        private string _p015;

        private string _p016;

        private string _p017;

        private string _p018;

        private string _p019;

        private string _p020;

        private string _p021;

        private string _p022;

        private string _p023;

        private string _p024;

        private string _p025;

        private string _p026;

        private string _p027;

        private string _p028;

        private string _p029;

        private string _p030;

        private string _p031;

        private string _p032;

        private string _p033;

        private string _p034;

        private string _p035;

        private string _p036;

        private string _p037;

        private string _p038;

        private string _p039;

        private string _p040;

        private string _p041;

        private string _p042;

        private string _p043;

        private string _p044;

        private string _p045;

        private string _p046;

        private string _p047;

        private string _p048;

        private string _p049;

        private string _p050;

        private string _s001;

        private string _s002;

        private string _s003;

        private string _s004;

        private string _s005;

        private string _s006;

        private string _s007;

        private string _s008;

        private string _s009;

        private string _s010;

        


     
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_family_cd", DbType = "VarChar(4) NOT NULL", CanBeNull = false)]
        public string family_cd
        {
            get
            {
                return this._family_cd;
            }
            set
            {
                if ((this._family_cd != value))
                {
                    this._family_cd = value;
                    this.NotifyPropertyChanged("family_cd");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_tool_cd", DbType = "VarChar(25) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string tool_cd
        {
            get
            {
                return this._tool_cd;
            }
            set
            {
                if ((this._tool_cd != value))
                {
                  
                  
                    this._tool_cd = value;
                    this.NotifyPropertyChanged("tool_cd");
                  
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DESCRIPTION", DbType = "VarChar(100)")]
        public string description
        {
            get
            {
                return this._description;
            }
            set
            {
                if ((this._description != value))
                {
                   
                  
                    this._description = value;
                    this.NotifyPropertyChanged("DESCRIPTION");
                  
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_MATERIAL_SIZE", DbType = "VarChar(50)")]
        public string MATERIAL_SIZE
        {
            get
            {
                return this._material_size;
            }
            set
            {
                if ((this._material_size != value))
                {
                   
                    this._material_size = value;
                    this.NotifyPropertyChanged("MATERIAL_SIZE");
                   
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_HEAT_TREATMENT", DbType = "VarChar(50)")]
        public string HEAT_TREATMENT
        {
            get
            {
                return this._heat_treatment;
            }
            set
            {
                if ((this._heat_treatment != value))
                {
                    
                    this._heat_treatment = value;
                    this.NotifyPropertyChanged("HEAT_TREATMENT");
                  
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PROGRAM_NO", DbType = "Decimal(10,0)")]
        public System.Nullable<decimal> PROGRAM_NO
        {
            get
            {
                return this._program_no;
            }
            set
            {
                if ((this._program_no != value))
                {
                  
                    this._program_no = value;
                    this.NotifyPropertyChanged("PROGRAM_NO");
                  
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P001", DbType = "VarChar(25)")]
        public string P001
        {
            get
            {
                return this._p001;
            }
            set
            {
                if ((this._p001 != value))
                {
                  
                    this._p001 = value;
                    this.NotifyPropertyChanged("P001");
                  
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P002", DbType = "VarChar(25)")]
        public string P002
        {
            get
            {
                return this._p002;
            }
            set
            {
                if ((this._p002 != value))
                {
                  
                    this._p002 = value;
                    this.NotifyPropertyChanged("P002");
                  
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P003", DbType = "VarChar(25)")]
        public string P003
        {
            get
            {
                return this._p003;
            }
            set
            {
                if ((this._p003 != value))
                {
                  
                    this._p003 = value;
                    this.NotifyPropertyChanged("P003");
                    
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P004", DbType = "VarChar(25)")]
        public string P004
        {
            get
            {
                return this._p004;
            }
            set
            {
                if ((this._p004 != value))
                {
                   
                    this._p004 = value;
                    this.NotifyPropertyChanged("P004");
                  
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P005", DbType = "VarChar(25)")]
        public string P005
        {
            get
            {
                return this._p005;
            }
            set
            {
                if ((this._p005 != value))
                {
                   
                    this._p005 = value;
                    this.NotifyPropertyChanged("P005");
                 
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P006", DbType = "VarChar(25)")]
        public string P006
        {
            get
            {
                return this._p006;
            }
            set
            {
                if ((this._p006 != value))
                {
                  
                    this._p006 = value;
                    this.NotifyPropertyChanged("P006");
                   
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P007", DbType = "VarChar(25)")]
        public string P007
        {
            get
            {
                return this._p007;
            }
            set
            {
                if ((this._p007 != value))
                {
                 
                    this._p007 = value;
                    this.NotifyPropertyChanged("P007");
                  
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P008", DbType = "VarChar(25)")]
        public string P008
        {
            get
            {
                return this._p008;
            }
            set
            {
                if ((this._p008 != value))
                {
                  
                    this._p008 = value;
                    this.NotifyPropertyChanged("P008");
                 
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P009", DbType = "VarChar(25)")]
        public string P009
        {
            get
            {
                return this._p009;
            }
            set
            {
                if ((this._p009 != value))
                {
                   
                    this._p009 = value;
                    this.NotifyPropertyChanged("P009");
             
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P010", DbType = "VarChar(25)")]
        public string P010
        {
            get
            {
                return this._p010;
            }
            set
            {
                if ((this._p010 != value))
                {
                    
                    this._p010 = value;
                    this.NotifyPropertyChanged("P010");
                 
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P011", DbType = "VarChar(25)")]
        public string P011
        {
            get
            {
                return this._p011;
            }
            set
            {
                if ((this._p011 != value))
                {
                  
                    this._p011 = value;
                    this.NotifyPropertyChanged("P011");
                 
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P012", DbType = "VarChar(25)")]
        public string P012
        {
            get
            {
                return this._p012;
            }
            set
            {
                if ((this._p012 != value))
                {
                  
                    this._p012 = value;
                    this.NotifyPropertyChanged("P012");
                 
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P013", DbType = "VarChar(25)")]
        public string P013
        {
            get
            {
                return this._p013;
            }
            set
            {
                if ((this._p013 != value))
                {
                
                    this._p013 = value;
                    this.NotifyPropertyChanged("P013");
                   
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P014", DbType = "VarChar(25)")]
        public string P014
        {
            get
            {
                return this._p014;
            }
            set
            {
                if ((this._p014 != value))
                {
                   
                    this._p014 = value;
                    this.NotifyPropertyChanged("P014");
                  
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P015", DbType = "VarChar(25)")]
        public string P015
        {
            get
            {
                return this._p015;
            }
            set
            {
                if ((this._p015 != value))
                {
                  
                    this._p015 = value;
                    this.NotifyPropertyChanged("P015");
                 
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P016", DbType = "VarChar(25)")]
        public string P016
        {
            get
            {
                return this._p016;
            }
            set
            {
                if ((this._p016 != value))
                {
                   
                    this._p016 = value;
                    this.NotifyPropertyChanged("P016");
                 
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P017", DbType = "VarChar(25)")]
        public string P017
        {
            get
            {
                return this._p017;
            }
            set
            {
                if ((this._p017 != value))
                {
                   
                    this._p017 = value;
                    this.NotifyPropertyChanged("P017");
                  
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P018", DbType = "VarChar(25)")]
        public string P018
        {
            get
            {
                return this._p018;
            }
            set
            {
                if ((this._p018 != value))
                {
                   
                    this._p018 = value;
                    this.NotifyPropertyChanged("P018");
                 
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P019", DbType = "VarChar(25)")]
        public string P019
        {
            get
            {
                return this._p019;
            }
            set
            {
                if ((this._p019 != value))
                {
                  
                    this._p019 = value;
                    this.NotifyPropertyChanged("P019");
                  
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P020", DbType = "VarChar(25)")]
        public string p020
        {
            get
            {
                return this._p020;
            }
            set
            {
                if ((this._p020 != value))
                {
                  
                    this._p020 = value;
                    this.NotifyPropertyChanged("P020");
                   
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_p021", DbType = "VarChar(25)")]
        public string p021
        {
            get
            {
                return this._p021;
            }
            set
            {
                if ((this._p021 != value))
                {
                  
                    this._p021 = value;
                    this.NotifyPropertyChanged("p021");
                  
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P022", DbType = "VarChar(25)")]
        public string P022
        {
            get
            {
                return this._p022;
            }
            set
            {
                if ((this._p022 != value))
                {
                   
                    this._p022 = value;
                    this.NotifyPropertyChanged("P022");
                
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P023", DbType = "VarChar(25)")]
        public string P023
        {
            get
            {
                return this._p023;
            }
            set
            {
                if ((this._p023 != value))
                {
                    
                    this._p023 = value;
                    this.NotifyPropertyChanged("P023");
                    
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P024", DbType = "VarChar(25)")]
        public string P024
        {
            get
            {
                return this._p024;
            }
            set
            {
                if ((this._p024 != value))
                {
                   
                    this._p024 = value;
                    this.NotifyPropertyChanged("P024");
                   
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P025", DbType = "VarChar(25)")]
        public string P025
        {
            get
            {
                return this._p025;
            }
            set
            {
                if ((this._p025 != value))
                {
                   
                    this._p025 = value;
                    this.NotifyPropertyChanged("P025");
                   
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P026", DbType = "VarChar(25)")]
        public string P026
        {
            get
            {
                return this._p026;
            }
            set
            {
                if ((this._p026 != value))
                {
                  
                    this._p026 = value;
                    this.NotifyPropertyChanged("P026");
                  
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P027", DbType = "VarChar(25)")]
        public string P027
        {
            get
            {
                return this._p027;
            }
            set
            {
                if ((this._p027 != value))
                {
                   
                    this._p027 = value;
                    this.NotifyPropertyChanged("P027");
                  
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P028", DbType = "VarChar(25)")]
        public string P028
        {
            get
            {
                return this._p028;
            }
            set
            {
                if ((this._p028 != value))
                {
                  
                    this._p028 = value;
                    this.NotifyPropertyChanged("P028");
                  
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P029", DbType = "VarChar(25)")]
        public string P029
        {
            get
            {
                return this._p029;
            }
            set
            {
                if ((this._p029 != value))
                {
                  
                    this._p029 = value;
                    this.NotifyPropertyChanged("P029");
                  
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P030", DbType = "VarChar(25)")]
        public string P030
        {
            get
            {
                return this._p030;
            }
            set
            {
                if ((this._p030 != value))
                {
                  
                    this._p030 = value;
                    this.NotifyPropertyChanged("P030");
                   
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P031", DbType = "VarChar(25)")]
        public string P031
        {
            get
            {
                return this._p031;
            }
            set
            {
                if ((this._p031 != value))
                {
                  
                    this._p031 = value;
                    this.NotifyPropertyChanged("P031");
                   
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P032", DbType = "VarChar(25)")]
        public string P032
        {
            get
            {
                return this._p032;
            }
            set
            {
                if ((this._p032 != value))
                {
                   
                    this._p032 = value;
                    this.NotifyPropertyChanged("P032");
                   
                }
            }
        }


        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P033", DbType = "VarChar(25)")]
        public string P033
        {
            get
            {
                return this._p033;
            }
            set
            {
                if ((this._p033 != value))
                {
                    
                    this._p033 = value;
                    this.NotifyPropertyChanged("P033");
                    
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P034", DbType = "VarChar(25)")]
        public string P034
        {
            get
            {
                return this._p034;
            }
            set
            {
                if ((this._p034 != value))
                {
                   
                    this._p034 = value;
                    this.NotifyPropertyChanged("P034");
                   
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P035", DbType = "VarChar(25)")]
        public string P035
        {
            get
            {
                return this._p035;
            }
            set
            {
                if ((this._p035 != value))
                {
                   
                    this._p035 = value;
                    this.NotifyPropertyChanged("P035");
                  
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P036", DbType = "VarChar(25)")]
        public string P036
        {
            get
            {
                return this._p036;
            }
            set
            {
                if ((this._p036 != value))
                {
                   
                    this._p036 = value;
                    this.NotifyPropertyChanged("P036");
                   
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P037", DbType = "VarChar(25)")]
        public string P037
        {
            get
            {
                return this._p037;
            }
            set
            {
                if ((this._p037 != value))
                {
                  
                    this._p037 = value;
                    this.NotifyPropertyChanged("P037");
                    
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P038", DbType = "VarChar(25)")]
        public string P038
        {
            get
            {
                return this._p038;
            }
            set
            {
                if ((this._p038 != value))
                {
                   
                    this._p038 = value;
                    this.NotifyPropertyChanged("P038");
                  
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P039", DbType = "VarChar(25)")]
        public string P039
        {
            get
            {
                return this._p039;
            }
            set
            {
                if ((this._p039 != value))
                {
                    
                    this._p039 = value;
                    this.NotifyPropertyChanged("P039");
                   
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P040", DbType = "VarChar(25)")]
        public string P040
        {
            get
            {
                return this._p040;
            }
            set
            {
                if ((this._p040 != value))
                {
                   
                    this._p040 = value;
                    this.NotifyPropertyChanged("P040");
                    
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P041", DbType = "VarChar(25)")]
        public string P041
        {
            get
            {
                return this._p041;
            }
            set
            {
                if ((this._p041 != value))
                {
                   
                    this._p041 = value;
                    this.NotifyPropertyChanged("P041");
                 
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P042", DbType = "VarChar(25)")]
        public string P042
        {
            get
            {
                return this._p042;
            }
            set
            {
                if ((this._p042 != value))
                {
                    
                    this._p042 = value;
                    this.NotifyPropertyChanged("P042");
                   
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P043", DbType = "VarChar(25)")]
        public string P043
        {
            get
            {
                return this._p043;
            }
            set
            {
                if ((this._p043 != value))
                {
                   
                    this._p043 = value;
                    this.NotifyPropertyChanged("P043");
                
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P044", DbType = "VarChar(25)")]
        public string P044
        {
            get
            {
                return this._p044;
            }
            set
            {
                if ((this._p044 != value))
                {
                   
                    this._p044 = value;
                    this.NotifyPropertyChanged("P044");
                    
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P045", DbType = "VarChar(25)")]
        public string P045
        {
            get
            {
                return this._p045;
            }
            set
            {
                if ((this._p045 != value))
                {
                   
                    this._p045 = value;
                    this.NotifyPropertyChanged("P045");
                  
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P046", DbType = "VarChar(25)")]
        public string P046
        {
            get
            {
                return this._p046;
            }
            set
            {
                if ((this._p046 != value))
                {
                    
                    this._p046 = value;
                    this.NotifyPropertyChanged("P046");
               
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P047", DbType = "VarChar(25)")]
        public string P047
        {
            get
            {
                return this._p047;
            }
            set
            {
                if ((this._p047 != value))
                {
                  
                    this._p047 = value;
                    this.NotifyPropertyChanged("P047");
                   
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P048", DbType = "VarChar(25)")]
        public string P048
        {
            get
            {
                return this._p048;
            }
            set
            {
                if ((this._p048 != value))
                {
                
                    this._p048 = value;
                    this.NotifyPropertyChanged("P048");
             
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P049", DbType = "VarChar(25)")]
        public string P049
        {
            get
            {
                return this._p049;
            }
            set
            {
                if ((this._p049 != value))
                {
                   
                    this._p049 = value;
                    this.NotifyPropertyChanged("P049");
                    
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_P050", DbType = "VarChar(25)")]
        public string P050
        {
            get
            {
                return this._p050;
            }
            set
            {
                if ((this._p050 != value))
                {
                   
                    this._p050 = value;
                    this.NotifyPropertyChanged("P050");
                   
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_S001", DbType = "VarChar(25)")]
        public string S001
        {
            get
            {
                return this._s001;
            }
            set
            {
                if ((this._s001 != value))
                {
                   
                    this._s001 = value;
                    this.NotifyPropertyChanged("S001");
                
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_S002", DbType = "VarChar(25)")]
        public string S002
        {
            get
            {
                return this._s002;
            }
            set
            {
                if ((this._s002 != value))
                {
                    this._s002 = value;
                    this.NotifyPropertyChanged("S002");
                    
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_S003", DbType = "VarChar(25)")]
        public string S003
        {
            get
            {
                return this._s003;
            }
            set
            {
                if ((this._s003 != value))
                {
                   
                    this._s003 = value;
                    this.NotifyPropertyChanged("S003");
                
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_S004", DbType = "VarChar(25)")]
        public string S004
        {
            get
            {
                return this._s004;
            }
            set
            {
                if ((this._s004 != value))
                {
                   
                    this._s004 = value;
                    this.NotifyPropertyChanged("S004");
                    
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_S005", DbType = "VarChar(25)")]
        public string S005
        {
            get
            {
                return this._s005;
            }
            set
            {
                if ((this._s005 != value))
                {
                    
                    this._s005 = value;
                    this.NotifyPropertyChanged("S005");
                   
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_S006", DbType = "VarChar(25)")]
        public string S006
        {
            get
            {
                return this._s006;
            }
            set
            {
                if ((this._s006 != value))
                {
                   
                    this._s006 = value;
                    this.NotifyPropertyChanged("S006");
                  
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_S007", DbType = "VarChar(25)")]
        public string S007
        {
            get
            {
                return this._s007;
            }
            set
            {
                if ((this._s007 != value))
                {
                   
                    this._s007 = value;
                    this.NotifyPropertyChanged("S007");
                    
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_S008", DbType = "VarChar(25)")]
        public string S008
        {
            get
            {
                return this._s008;
            }
            set
            {
                if ((this._s008 != value))
                {
                    
                    this._s008 = value;
                    this.NotifyPropertyChanged("S008");
                   
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_S009", DbType = "VarChar(25)")]
        public string S009
        {
            get
            {
                return this._s009;
            }
            set
            {
                if ((this._s009 != value))
                {
                  
                    this._s009 = value;
                    this.NotifyPropertyChanged("S009");
                   
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_S010", DbType = "VarChar(25)")]
        public string S010
        {
            get
            {
                return this._s010;
            }
            set
            {
                if ((this._s010 != value))
                {
                   
                    this._s010 = value;
                    this.NotifyPropertyChanged("S010");
              
                }
            }
        }

       

        //public DataTable GridData
        //{
        //    get;
        //    private set;
        //}

        //public DataView GridDtDataview
        //{
        //    get
        //    {
        //        return this.GridData.DefaultView;
        //    }
        //    set
        //    {
        //        this.GridData = value.ToTable();
        //        NotifyPropertyChanged("GridDtDataview");
        //    }
        //}
        

    }
}
