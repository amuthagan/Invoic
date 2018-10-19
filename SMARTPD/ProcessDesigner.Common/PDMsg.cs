using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Common
{
    public static class PDMsg
    {
        static public string SavedSuccessfully = "Record added successfully".TrimWhiteSpace();
        static public string DeletedSuccessfully = "Record deleted successfully".TrimWhiteSpace();
        static public string UpdatedSuccessfully = "Record updated successfully".TrimWhiteSpace();
        static public string BeforeDeleteRecord = "Do you really want to delete the record?".TrimWhiteSpace();
        static public string CloseForm = "Do you want to close the form?".TrimWhiteSpace();
        static public string NoRecordsPrint = "No record to print".TrimWhiteSpace();
        static public string NoRecordFound = "No record found".TrimWhiteSpace();
        static public string NoRecordExcel = "No data to export".TrimWhiteSpace();
        static public string SelectDeleteRecord = "Do you want to delete the selected record?".TrimWhiteSpace();
        static public string BeforeCloseForm = "Do you want to save changes?".TrimWhiteSpace();
        static public string ProgressUpdateText = "Updating".TrimWhiteSpace();
        static public string DBNotConnected = "Database not Connected. Please Contact the Administrator".TrimWhiteSpace();
        static public string WrongUNamePassword = "Wrong User Name/Password Entered!".TrimWhiteSpace();
        static public string RoleNotDefined = "Role not defined. Please contact Administrator!".TrimWhiteSpace();
        static public string Search = "Searching".TrimWhiteSpace();
        static public string Load = "Loading".TrimWhiteSpace();
        static public string Copy = "Copying".TrimWhiteSpace();

        static public string BeforeDelete(string data)
        {
            return "Do you really want to delete this " + data.TrimWhiteSpace() + "?";
        }

        static public string VersionMisMatch(string data)
        {
            return "Version mismatches between Application & Database - << " + data.TrimWhiteSpace() + ">>";
        }

        static public string Invalid(string data)
        {
            return "Invalid " + data.TrimWhiteSpace();
        }

        static public string EnterValid(string data)
        {
            return "Enter Valid " + data.TrimWhiteSpace();
        }

        static public string NormalString(string data)
        {
            return data.TrimWhiteSpace();
        }

        static public string DoesNotExists(string data)
        {
            return data.TrimWhiteSpace() + " does not exists";
        }

        static public string AlreadyExists(string data)
        {
            return data.TrimWhiteSpace() + " already exists";
        }

        static public string NotEmpty(string data)
        {
            return data.TrimWhiteSpace() + " should not be empty";
        }

        static public string ToPrint(string data)
        {
            return "Please enter " + data.TrimWhiteSpace() + " to print";
        }

        static public string NotExceeds(string data, string value)
        {
            return data.TrimWhiteSpace() + " should not exceeds '" + value.TrimWhiteSpace() + "'";
        }

    }
}
