
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Data;
using System.Reflection;
using ProcessDesigner.Model;
using System.Drawing.Imaging;

namespace ProcessDesigner.Common
{
    public static partial class ExtendedMethods
    {
        public static string[] ColumnNames(this DataTable dataTable)
        {
            if (!dataTable.IsNotNullOrEmpty()) return null;

            string[] columnNames = dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
            return columnNames;

        }

        /// <summary>
        /// Export DataTable to Excel file
        /// </summary>
        /// <param name="dataTable">Source DataTable</param>
        /// <param name="fileName">Path to result file name</param>
        public static bool ExportToExcel(this System.Data.DataTable dataTable, string fileName = null)
        {
            bool bResult = false;
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            // single worksheet
            Microsoft.Office.Interop.Excel._Worksheet worksheet = excel.ActiveSheet;

            try
            {
                int columnsCount;

                if (dataTable == null || (columnsCount = dataTable.Columns.Count) == 0)
                    throw new Exception("ExportToExcel: Null or empty input table!\n");

                // load excel, and create a new workbook

                excel.Workbooks.Add();

                if (!worksheet.IsNotNullOrEmpty())
                {
                    worksheet = excel.ActiveSheet;
                }

                object[] header = new object[columnsCount];
                header = dataTable.ColumnNames();

                Microsoft.Office.Interop.Excel.Range headerRange = worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)(worksheet.Cells[1, 1]), (Microsoft.Office.Interop.Excel.Range)(worksheet.Cells[1, columnsCount]));
                headerRange.Value = header;
                headerRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                headerRange.Font.Bold = true;
                // DataCells
                int rowsCount = dataTable.Rows.Count;
                object[,] cells = new object[rowsCount, columnsCount];

                for (int j = 0; j < rowsCount; j++)
                    for (int i = 0; i < columnsCount; i++)
                        cells[j, i] = dataTable.Rows[j][i];
                worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)(worksheet.Cells[2, 1]), (Microsoft.Office.Interop.Excel.Range)(worksheet.Cells[rowsCount + 1, columnsCount])).Value = cells; // dataTable.AsEnumerable().Select(row => row.ItemArray).ToArray();
                worksheet.Columns.AutoFit();
                worksheet.Columns[3].NumberFormat = "dd/MM/yyyy";
                if (fileName != null && fileName != "")
                {
                    try
                    {
                        excel.DisplayAlerts = false;
                        worksheet.SaveAs(fileName);
                        bResult = true;
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                            + ex.Message);
                    }
                }
                else    // no filepath is given
                {
                    excel.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
                throw new Exception("ExportToExcel: \n" + ex.Message);
            }
            finally
            {
                excel.Quit();
                while (System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet) != 0) { }
                worksheet = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return bResult;
        }

        public static bool ExportMFMToExcel(this System.Data.DataTable dataTable, string fileName = null)
        {
            bool bResult = false;
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            // single worksheet
            Microsoft.Office.Interop.Excel._Worksheet worksheet = excel.ActiveSheet;

            try
            {
                int columnsCount;

                if (dataTable == null || (columnsCount = dataTable.Columns.Count) == 0)
                    throw new Exception("ExportToExcel: Null or empty input table!\n");

                // load excel, and create a new workbook

                excel.Workbooks.Add();

                if (!worksheet.IsNotNullOrEmpty())
                {
                    worksheet = excel.ActiveSheet;
                }

                object[] header = new object[columnsCount];
                header = dataTable.ColumnNames();

                Microsoft.Office.Interop.Excel.Range headerRange = worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)(worksheet.Cells[1, 1]), (Microsoft.Office.Interop.Excel.Range)(worksheet.Cells[1, columnsCount]));
                headerRange.Value = header;
                headerRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                headerRange.Font.Bold = true;
                // DataCells
                int rowsCount = dataTable.Rows.Count;
                object[,] cells = new object[rowsCount, columnsCount];

                for (int j = 0; j < rowsCount; j++)
                    for (int i = 0; i < columnsCount; i++)
                        cells[j, i] = dataTable.Rows[j][i];
                worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)(worksheet.Cells[2, 1]), (Microsoft.Office.Interop.Excel.Range)(worksheet.Cells[rowsCount + 1, columnsCount])).Value = cells; // dataTable.AsEnumerable().Select(row => row.ItemArray).ToArray();
                worksheet.Columns.AutoFit();
                worksheet.Columns[2].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                worksheet.Columns[3].NumberFormat = "dd/MM/yyyy";
                worksheet.Columns[4].NumberFormat = "dd/MM/yyyy";
                worksheet.Columns[5].NumberFormat = "dd/MM/yyyy";
                worksheet.Columns[6].NumberFormat = "dd/MM/yyyy";
                worksheet.Columns[7].NumberFormat = "dd/MM/yyyy";
                worksheet.Columns[8].NumberFormat = "dd/MM/yyyy";
                worksheet.Columns[9].NumberFormat = "dd/MM/yyyy";
                worksheet.Columns[10].NumberFormat = "dd/MM/yyyy";
                worksheet.Columns[11].NumberFormat = "dd/MM/yyyy";
                worksheet.Columns[12].NumberFormat = "dd/MM/yyyy";
                worksheet.Columns[13].NumberFormat = "dd/MM/yyyy";
                worksheet.Columns[14].NumberFormat = "dd/MM/yyyy";
                worksheet.Columns[15].NumberFormat = "dd/MM/yyyy";
                worksheet.Columns[16].NumberFormat = "dd/MM/yyyy";
                worksheet.Columns[17].NumberFormat = "dd/MM/yyyy";
                //Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;                
                if (fileName != null && fileName != "")
                {
                    try
                    {
                        excel.DisplayAlerts = false;
                        worksheet.SaveAs(fileName);
                        bResult = true;
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                        throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                            + ex.Message);
                    }
                }
                else    // no filepath is given
                {
                    excel.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
                throw new Exception("ExportToExcel: \n" + ex.Message);
            }
            finally
            {
                excel.Quit();
                while (System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet) != 0) { }
                worksheet = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return bResult;
        }

        public static System.Windows.Media.Imaging.BitmapImage ToBitmapImage(this System.Drawing.Bitmap bitmap)
        {
            try
            {
                using (var memory = new System.IO.MemoryStream())
                {
                    bitmap.Save(memory, ImageFormat.Png);
                    memory.Position = 0;

                    var bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = memory;
                    bitmapImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();

                    return bitmapImage;
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
                return null;
            }
        }

        public static void SetUserRights(this System.Windows.Controls.UserControl container, RolePermission permission, Button addButton, Button editButton, Button deleteButton, Button printButton, Button viewButton = null)
        {
            string sourcename = container.Tag.ToValueAsString();
            try
            {
                if (sourcename.IsNotNullOrEmpty())
                {
                    if (addButton.IsNotNullOrEmpty()) addButton.IsEnabled = permission.AddNew;
                    if (editButton.IsNotNullOrEmpty()) editButton.IsEnabled = permission.Edit;
                    if (deleteButton.IsNotNullOrEmpty()) deleteButton.IsEnabled = permission.Delete;
                    if (printButton.IsNotNullOrEmpty()) printButton.IsEnabled = permission.Print;
                    if (viewButton.IsNotNullOrEmpty()) viewButton.IsEnabled = permission.View;

                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
            }
        }

        /// <summary>
        /// Helps to find list is null
        /// </summary>
        /// <param name="dataTable">object</param>
        /// <returns>True - NonEmpty, False - Empty</returns>
        public static bool IsNotNullOrEmptyl<T>(this List<T> list) where T : System.Windows.DependencyObject
        {
            bool hasNotNullOrEmpty = false;
            try
            {
                if (list != null) hasNotNullOrEmpty = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return hasNotNullOrEmpty;
        }




        /// <summary>
        /// Helps to clear or unclear particular control in a container
        /// </summary>
        /// <typeparam name="T">Type of control like button, textbox....</typeparam>
        /// <param name="container">Type of container like window, usercontrol, grid,panel...</param>
        /// <param name="isClear">True - Clear, False - Unclear</param>
        /// <param name="exceptControls">Clear or Unclear all control in a container except this</param>
        private static void ClearOrUnClearAllControl<T>(System.Windows.Controls.UserControl container, bool isClear, params T[] exceptControls) where T : System.Windows.DependencyObject
        {
            if (container == null) return;
            foreach (T control in FindVisualChildren<T>(container))
            {
                if (control as Control != null)
                {
                    ClearOrUnClearControl(control, isClear);
                }
            }

            if (exceptControls != null && exceptControls.Length > 0)
            {
                foreach (T control in exceptControls)
                {
                    if (control as Control != null)
                    {
                        ClearOrUnClearControl(control, !isClear);
                    }

                }
            }
        }

        /// <summary>
        /// Helps to clear all control text/content in a container
        /// </summary>
        /// <typeparam name="T">Type of control like button, textbox....</typeparam>
        /// <param name="container">Type of container like window, usercontrol, grid,panel...</param>
        /// <param name="exceptControls">Clear all control text/content in a container except this</param>
        public static void ClearAllControl<T>(this System.Windows.Controls.UserControl container, params T[] exceptControls) where T : System.Windows.DependencyObject
        {
            ClearOrUnClearAllControl(container, true, exceptControls);
        }

        /// <summary>
        /// Helps to Clear or UnClear particular control in a container
        /// </summary>
        /// <typeparam name="T">Type of control like textbox....</typeparam>
        /// <param name="control">Type of control like textbox....</param>
        /// <param name="isClear">True - Clear, False -UnClear</param>
        public static void ClearOrUnClearControl(object control, bool isClear)
        {
            if (control == null || control as Control == null) return;
            if (control as TextBox is TextBox && isClear == true)
                (control as TextBox).Clear();
            else if (control as CheckBox is CheckBox)
                (control as CheckBox).IsChecked = isClear;
            else if (control as RadioButton is RadioButton)
                (control as RadioButton).IsChecked = isClear;

        }


        /// <summary>
        /// Helps to Lock or Unlock particular control in a container
        /// </summary>
        /// <typeparam name="T">Type of control like button, textbox....</typeparam>
        /// <param name="container">Type of container like window, usercontrol, grid,panel...</param>
        /// <param name="isEnabled">True - Lock, False - Unlock</param>
        /// <param name="exceptControls">Lock or Unlock all control in a container except this</param>
        private static void LockOrUnlockAllControl<T>(this System.Windows.Controls.UserControl container, bool isReadOnly, params T[] exceptControls) where T : System.Windows.DependencyObject
        {
            if (container == null) return;
            foreach (T control in FindVisualChildren<T>(container))
            {
                if (control as Control != null)
                {
                    LockOrUnlockControl(control, isReadOnly);
                }
            }

            if (exceptControls != null && exceptControls.Length > 0)
            {
                foreach (T control in exceptControls)
                {
                    if (control as Control != null)
                    {
                        LockOrUnlockControl(control, !isReadOnly);
                    }

                }
            }
        }

        /// <summary>
        /// Helps to Lock all control in a container
        /// </summary>
        /// <typeparam name="T">>Type of control like button, textbox....</typeparam>
        /// <param name="container">Type of container like window, usercontrol, grid,panel...</param>
        /// <param name="exceptControls">Lock all control in a container except this</param>
        public static void LockAllControl<T>(this System.Windows.Controls.UserControl container, params T[] exceptControls) where T : System.Windows.DependencyObject
        {
            LockOrUnlockAllControl(container, true, exceptControls);
        }

        /// <summary>
        /// Helps to UnLock all control in a container
        /// </summary>
        /// <typeparam name="T">>Type of control like button, textbox....</typeparam>
        /// <param name="container">Type of container like window, usercontrol, grid,panel...</param>
        /// <param name="exceptControls">UnLock all control in a container except this</param>
        public static void UnlockAllControl<T>(this System.Windows.Controls.UserControl container, params T[] exceptControls) where T : System.Windows.DependencyObject
        {
            LockOrUnlockAllControl(container, false, exceptControls);
        }

        /// <summary>
        /// Helps to Lock or Unlock particular control in a container
        /// </summary>
        /// <typeparam name="T">Type of control like textbox....</typeparam>
        /// <param name="control">Type of control like textbox....</param>
        /// <param name="isReadOnly">True - Lock, False -Unlock</param>
        private static void LockOrUnlockControl(object control, bool isReadOnly)
        {
            if (control == null && control as Control == null) return;
            if (control as TextBox is TextBox)
                (control as TextBox).IsReadOnly = isReadOnly;
        }

        /// <summary>
        /// Helps to enable or disable particular control in a container
        /// </summary>
        /// <typeparam name="T">Type of control like button, textbox....</typeparam>
        /// <param name="container">Type of container like window, usercontrol, grid,panel...</param>
        /// <param name="isEnabled">True - Enable, False -Disable</param>
        /// <param name="exceptControls">Enable or disable all control in a container except this</param>
        private static void EnableOrDisableAllControl<T>(System.Windows.Controls.UserControl container, bool isEnabled, params T[] exceptControls) where T : System.Windows.DependencyObject
        {
            if (container == null) return;
            foreach (T control in FindVisualChildren<T>(container))
            {
                if (control as Control != null)
                {
                    (control as Control).IsEnabled = isEnabled;
                }
            }

            if (exceptControls != null && exceptControls.Length > 0)
            {
                foreach (T control in exceptControls)
                {
                    if (control as Control != null)
                    {
                        (control as Control).IsEnabled = !isEnabled;
                    }

                }
            }
        }

        /// <summary>
        /// Helps to enable particular control in a container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        public static void EnableAllControl<T>(this System.Windows.Controls.UserControl container, params T[] exceptControls) where T : System.Windows.DependencyObject
        {
            EnableOrDisableAllControl(container, true, exceptControls);
        }

        public static void DisableAllControl<T>(this System.Windows.Controls.UserControl container, params T[] exceptControls) where T : System.Windows.DependencyObject
        {
            EnableOrDisableAllControl(container, false, exceptControls);
        }

        /// <summary>
        /// Helps to enable or disable particular control in a container
        /// </summary>
        /// <typeparam name="T">Type of control like button, textbox....</typeparam>
        /// <param name="container">Type of container like window, usercontrol, grid,panel...</param>
        /// <param name="isEnabled">True - Enable, False -Disable</param>
        /// <param name="exceptControls">Enable or disable all control in a container except this</param>
        public static void SetColumnLength<T>(this System.Windows.Controls.UserControl container, List<V_TABLE_DESCRIPTION> listObject) where T : System.Windows.DependencyObject
        {
            if (container == null) return;
            foreach (T control in FindVisualChildren<T>(container))
            {
                if (control as Control != null && (control as Control).Tag.ToValueAsString().IsNotNullOrEmpty())
                {
                    V_TABLE_DESCRIPTION foundRow = (from row in listObject
                                                    where row.COLUMN_NAME == Convert.ToString((control as Control).Tag)
                                                    select row).SingleOrDefault<V_TABLE_DESCRIPTION>();

                    if (foundRow != null)
                        (control as TextBox).MaxLength = Convert.ToInt32(foundRow.COLUMN_LENGTH);

                }
            }

        }


        /// <summary>
        /// Helps to enable or disable particular control in a container
        /// </summary>
        /// <typeparam name="T">Type of control like button, textbox....</typeparam>
        /// <param name="container">Type of container like window, usercontrol, grid,panel...</param>
        /// <param name="isEnabled">True - Enable, False -Disable</param>
        /// <param name="exceptControls">Enable or disable all control in a container except this</param>
        public static void SetColumnLength<T>(this System.Windows.Window container, List<V_TABLE_DESCRIPTION> listObject) where T : System.Windows.DependencyObject
        {
            if (container == null) return;
            foreach (T control in FindVisualChildren<T>(container))
            {
                if (control as Control != null && (control as Control).Tag.ToValueAsString().IsNotNullOrEmpty())
                {
                    V_TABLE_DESCRIPTION foundRow = (from row in listObject
                                                    where row.COLUMN_NAME == Convert.ToString((control as Control).Tag)
                                                    select row).SingleOrDefault<V_TABLE_DESCRIPTION>();

                    if (foundRow != null)
                        (control as TextBox).MaxLength = Convert.ToInt32(foundRow.COLUMN_LENGTH);

                }
            }

        }

        /// <summary>
        /// Helps to find particular control in a container
        /// </summary>
        /// <typeparam name="T">Type of control like button, textbox....</typeparam>
        /// <param name="obj">Type of container like window, usercontrol, grid,panel...</param>
        /// <returns>All controls in a container</returns>
        public static IEnumerable<T> FindVisualChildren<T>(System.Windows.DependencyObject obj) where T : System.Windows.DependencyObject
        {
            if (obj != null)
            {
                if (obj is T)
                    yield return obj as T;

                foreach (System.Windows.DependencyObject child in System.Windows.LogicalTreeHelper.GetChildren(obj).OfType<System.Windows.DependencyObject>())
                    foreach (T c in FindVisualChildren<T>(child))
                        yield return c;
            }
        }

    }
}
