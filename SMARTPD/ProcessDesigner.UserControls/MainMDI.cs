using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using WPF.MDI;

namespace ProcessDesigner.UserControls
{
    public class MainMDI
    {
        public static MdiContainer Container = null;

        public static bool IsFormAlreadyOpen(string formtitle)
        {
            try
            {
                int containerCount = 0;
                string containertitle = string.Empty;
                containerCount = Container.Children.Count;
                if (containerCount == 0) return false;
                for (int i = 0; i < containerCount; i++)
                {
                    containertitle = Container.Children[i].Title.ToString().ToUpper();

                    int retval = 0;
                    retval = (containertitle.IndexOf(formtitle.ToString().ToUpper(), 0));
                    if (retval > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static object GetFormAlreadyOpened(string formtitle)
        {
            try
            {
                int containerCount = 0;
                string containertitle = string.Empty;
                containerCount = Container.Children.Count;
                if (containerCount == 0) return false;
                for (int i = 0; i < containerCount; i++)
                {
                    containertitle = Container.Children[i].Title.ToString().ToUpper();

                    int retval = 0;
                    retval = (containertitle.IndexOf(formtitle.ToString().ToUpper(), 0));
                    if (retval > 0)
                    {
                        return Container.Children[i];
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        public static void SetMDI(MdiChild mdiChild)
        {
            try
            {
                int maxZindex = 0;
                for (int i = 0; i < mdiChild.Container.Children.Count; i++)
                {
                    int zindex = System.Windows.Controls.Panel.GetZIndex(mdiChild.Container.Children[i]);
                    if (zindex > maxZindex)
                        maxZindex = zindex;
                    if (mdiChild.Container.Children[i] != mdiChild)
                    {
                        mdiChild.Container.Children[i].Focused = false;
                    }
                    else
                        mdiChild.Focused = true;
                }
                System.Windows.Controls.Panel.SetZIndex(mdiChild, maxZindex + 1);
            }
            catch (Exception ex)
            {

            }
        }


    }
}
