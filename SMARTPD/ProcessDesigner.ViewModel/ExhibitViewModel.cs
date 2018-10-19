using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.Common;
using ProcessDesigner.BLL;
using System.Windows;

namespace ProcessDesigner
{
    [Export]   
    public class ExhibitViewModel : BindableBase  
    {
        private string _exhibitNumber;
        private string _exhibitDetails;
        private string _message;      
        private readonly ICommand clearExhibitCommand;
        private readonly ICommand updateExhibitCommand;
        private UserInformation userinformation;
        private BLL.ExhitbitMaster exhit;

        [ImportingConstructor]
        public ExhibitViewModel()
        {
            UserInformation user = (UserInformation)App.Current.Properties["userinfo"];
            userinformation = user;
            exhit = new ExhitbitMaster();
            this.ExhitbitDocs = exhit.getExhibitDocuments(); 

            this.clearExhibitCommand = new DelegateCommand(this.ClearExhibit);
            this.updateExhibitCommand = new DelegateCommand(this.UpdateExhibit);
           
        }
        public string ExhibitDetails
        {
            get
            {
                return this._exhibitDetails;
            }
            set
            {
                SetProperty(ref this._exhibitDetails, value);
            }
        }
        public string ExhibitNumber
        {
            get
            {
                return this._exhibitNumber;
            }
            set
            {
                SetProperty(ref this._exhibitNumber, value);
            }
        }
        public string Message
        {
            get
            {
                return this._message;
            }
            set
            {
                SetProperty(ref this._message, value);
            }
        }
        public List<EXHIBIT_DOC> ExhitbitDocs { get; private set; }
        public ICommand ClearExhibitCommand { get { return this.clearExhibitCommand; } }
        public ICommand UpdateExhibitCommand { get { return this.updateExhibitCommand; } }

        private void ClearExhibit()
        {
            ExhibitDetails = "";
            ExhibitNumber = "";
        }
        private void UpdateExhibit()
        {
            if (String.IsNullOrEmpty(ExhibitNumber))
            { 
                MessageBox.Show("Please select Exhibit Document");
            }
            else if (String.IsNullOrEmpty(ExhibitDetails))
            { 
                MessageBox.Show("Please enter Exhibit Document");
            }
            else
            {
               bool val = exhit.updateExhitbitMaster(ExhibitNumber, ExhibitDetails);
                if (val)
                {
                    MessageBox.Show("Exhibit Updated");
                    ClearExhibit();
                }
            }
            //string query = "UPDATE [EXHIBIT_DOC] set [EX_NO]='" + ExhibitNumber + "' WHERE [DOC_NAME]='" + ExhibitDetails + "'";
            //IEnumerable<int> result = userinformation.Dal.SFLPDDatabase.ExecuteQuery<int>(query);
            //MessageBox.Show("Exhibit Updated");
                //Message = "Exhibit Updated";
           
            //using (SFLPD db = new SFLPD(userinformation.Dal.SFLPDDatabase.Connection))
            //{
            //    IEnumerable<int> result = db.ExecuteQuery<int>(query);
            //    //var exhibit = (from e in db.EXHIBIT_DOC where e.DOC_NAME == ExhibitDetails select e).FirstOrDefault();
            //    //exhibit.EX_NO = ExhibitNumber;
            //    //db.SubmitChanges();
            //    ExhibitDetails = "";
            //    ExhibitNumber = "";  
            //}                    
        }
    }
}
