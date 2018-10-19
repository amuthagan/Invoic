using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
  public class ProcessSheetArrayModel
    {
        

        private string _part_no;
        public string PART_NO
        {
            get { return _part_no; }
            set
            {
                _part_no = value;
            }
        }

        private int _route_no;
        public int ROUTE_NO
        {
            get { return _route_no; }
            set
            {
                _route_no = value;
            }
        }

        private int _seq_no;
        public int SEQ_NO
        {
            get { return _seq_no; }
            set
            {
                _seq_no = value;
            }
        }

        //     private int _seq_no;
        //     public int SEQ_NO
        //     {
        //         get { return _seq_no; }
        //         set
        //         {
        //             _seq_no = value;
        //         }
        //     }

        //     private int _opn_cd;
        //     public int OPN_CD
        //     {
        //         get { return _opn_cd; }
        //         set
        //         {
        //             _opn_cd = value;
        //         }
        //     }


        //     private string _opn_desc;
        //     public string OPN_DESC
        //     {
        //         get { return _opn_desc; }
        //         set
        //         {
        //             _opn_desc = value;
        //         }
        //     }


        //     private int _setting_time;
        //     public int SETTING_TIME
        //     {
        //         get { return _setting_time; }
        //         set
        //         {
        //             _setting_time = value;
        //         }
        //     }


        //     private int _efficiency;
        //     public int EFFICIENCY
        //     {
        //         get { return _efficiency; }
        //         set
        //         {
        //             _efficiency = value;
        //         }
        //     }


        //     private int _stage_wt;
        //     public int STAGE_WT
        //     {
        //         get { return _stage_wt; }
        //         set
        //         {
        //             _stage_wt = value;
        //         }
        //     }


        //     private int _transport;
        //     public int TRANSPORT
        //     {
        //         get { return _transport; }
        //         set
        //         {
        //             _transport = value;
        //         }
        //     }


        //     private int _output;
        //     public int OUTPUT
        //     {
        //         get { return _output; }
        //         set
        //         {
        //             _output = value;
        //         }
        //     }


           


        
    }
}
