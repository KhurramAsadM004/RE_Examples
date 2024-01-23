using System;
using System.Windows.Forms;

namespace CodeQuality
{
    public class OOPMetrics
    {
        // define menu constants
        const string menuHeader = "-&OOP Metrics";
        const string menuPbCR = "&Public Class";
        const string menuPrCR = "&Private Classes";
        // remember if we have to say hello or goodbye
        private bool shouldWeSayHello = true;
        ///
        /// Called Before EA starts to check Add-In Exists
        /// Nothing is done here.
        /// This operation needs to exists for the addin to work
        ///
        /// <param name="Repository" />the EA repository
        /// a string
        public String EA_Connect(EA.Repository Repository)
        {
            //No special processing required.
            return "a string";
        }
        ///
        /// Called when user Clicks Add-Ins Menu item from within EA.
        /// Populates the Menu with our desired selections.
        /// Location can be "TreeView" "MainMenu" or "Diagram".
        ///
        /// <param name="Repository" />the repository
        /// <param name="Location" />the location of the menu
        /// <param name="MenuName" />the name of the menu
        ///
        public object EA_GetMenuItems(EA.Repository Repository, string Location, string MenuName)
        {
            switch (MenuName)
            {
                // defines the top level menu option
                case "":
                    return menuHeader;
                // defines the submenu options
                case menuHeader:
                    string[] subMenus = { menuPbCR, menuPrCR };
                    return subMenus;
            }
            return "";
        }
        ///
        /// returns true if a project is currently opened
        ///
        /// <param name="Repository" />the repository
        /// true if a project is opened in EA
        bool IsProjectOpen(EA.Repository Repository)
        {
            try
            {
                EA.Collection c = Repository.Models;
                return true;
            }
            catch
            {
                return false;
            }
        }
        ///
        /// Called once Menu has been opened to see what menu items should active.
        ///
        /// <param name="Repository" />the repository
        /// <param name="Location" />the location of the menu
        /// <param name="MenuName" />the name of the menu
        /// <param name="ItemName" />the name of the menu item
        /// <param name="IsEnabled" />boolean indicating whethe the menu item is enabled
        /// <param name="IsChecked" />boolean indicating whether the menu is checked
        public void EA_GetMenuState(EA.Repository Repository, string Location, string MenuName, string ItemName, ref bool IsEnabled, ref bool IsChecked)
        {
            if (IsProjectOpen(Repository))
            {
                switch (ItemName)
                {
                    // define the state of the hello menu option
                    case menuPbCR:
                        IsEnabled = true;
                        break;
                    // define the state of the goodbye menu option
                    case menuPrCR:
                        IsEnabled = false;
                        break;
                    // there shouldn't be any other, but just in case disable it.
                    default:
                        IsEnabled = false;
                        break;
                }
            }
            else
            {
                // If no open project, disable all menu options
                IsEnabled = false;
            }
        }
        ///
        /// Called when user makes a selection in the menu.
        /// This is your main exit point to the rest of your Add-in
        ///
        /// <param name="Repository" />the repository
        /// <param name="Location" />the location of the menu
        /// <param name="MenuName" />the name of the menu
        /// <param name="ItemName" />the name of the selected menu item
        public void EA_MenuClick(EA.Repository Repository, string Location, string MenuName, string ItemName)
        {
            switch (ItemName)
            {
                // user has clicked the menuHello menu option
                case menuPbCR:
                    this.PbCR(Repository);
                    break;
                    // user has clicked the menuGoodbye menu option
                    //case menuGoodbye:
                    //    this.sayGoodbye();
                    //    break;
            }
        }
        ///
        /// Say Hello to the world
        ///
        private void PbCR(EA.Repository Repository)
        {
            string sqlAllClasses = @"select * from t_object where Object_Type = 'Class'";
            string sqlPublicClasses = @"select * from t_object where Object_Type = 'Class' and scope = 'Public'";
            var allClasses = Repository.GetElementSet(sqlAllClasses, 2).Count;
            var publicClasses = Repository.GetElementSet(sqlPublicClasses, 2).Count;
            double ratio = Convert.ToDouble(publicClasses) / Convert.ToDouble(allClasses);
            MessageBox.Show("Total Classes = " + allClasses.ToString() + " and public classes = " + publicClasses.ToString() + " and ratio is  " + String.Format("{0:0.00}", (ratio)));
        }

        ///
        /// EA calls this operation when it exists. Can be used to do some cleanup work.
        ///
        public void EA_Disconnect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
