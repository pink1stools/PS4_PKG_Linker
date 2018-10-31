using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MahApps.Metro;
using MahApps.Metro.Controls;
using Newtonsoft.Json;

namespace PS4_PKG_Linker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        DataTable dtpkg = new DataTable();
        DataTable dtpkg2 = new DataTable();
        DataTable dt2 = new DataTable();
        int i = 0;
        static string s = null;
        static string scid = null;
        string pkg;
        string pkg_directory;
        FileInfo[] Files;
        FileInfo[] Files2;
        string[] directories;
        DirectoryInfo dinfo;

        string color;
        string ctheme = "BaseDark";


        string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;//System.Windows.Shapes.Path.GetDirectoryName(Application.ExecutablePath.);
        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public MainWindow()
        {
            InitializeComponent();
            ChangeAppStyle();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
           

            dtpkg.Columns.Add("IsSelected");
            dtpkg.Columns.Add("Name");
            dtpkg.Columns.Add("CID");
            dtpkg.Columns.Add("type");
            dtpkg.Columns.Add("Size");

            dtpkg2.Columns.Add("Name");
            dtpkg2.Columns.Add("CID");
            dtpkg2.Columns.Add("type");
            dtpkg2.Columns.Add("Size");
            dtpkg2.Columns.Add("icon");
            dtpkg2.Columns.Add("tool");
            dtpkg2.Columns.Add("count");
            dtpkg2.Columns.Add("bl");
            dtpkg2.Columns.Add("tileh");
            dtpkg2.Columns.Add("tilew");
            dtpkg2.Columns.Add("column1w");
            dtpkg2.Columns.Add("column2w");
            dtpkg2.Columns.Add("roww");
            dtpkg2.Columns.Add("imags");
            dtpkg2.Columns.Add("text1s");
            dtpkg2.Columns.Add("text2s");


            appPath = appPath.Replace("PS3gbs.exe", "");
            
            pkg_folder();
            
            //this.child01.IsOpen = true;
        }

        static string SizeSuffix(Int64 value, int decimalPlaces = 1)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }

            int c = 0;
            decimal dValue = (decimal)value;
            while (Math.Round(dValue, decimalPlaces) >= 1000)
            {
                dValue /= 1024;
                c++;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[c]);
        }

        #region<<style>>

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            if (ctheme == "BaseDark")
            {
                button7.Content = "Dark Theme";
                ctheme = "BaseLight";
            }
            else
            {
                button7.Content = "Light Theme";
                ctheme = "BaseDark";
            }
            ChangeAppStyle();
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {

            color = ((ListBoxItem)listBox1.SelectedValue).Content.ToString();
            ChangeAppStyle();
        }

        public void ChangeAppStyledark()
        {
            // get the theme from the window
            // get the theme from the current application
            var theme = ThemeManager.DetectAppStyle(System.Windows.Application.Current);

            // now set the Green accent and dark theme
            ThemeManager.ChangeAppStyle(System.Windows.Application.Current,
                                        ThemeManager.GetAccent("Indigo"),
                                        ThemeManager.GetAppTheme("BaseDark"));
        }

        public void ChangeAppStyle()
        {

            if (ctheme == "BaseDark")
            {
                //button7.Content = "Light Theme";
                //b7tb1.Text = "Light Theme";
                //ctheme = "BaseLight";
            }
            else
            {
                //button7.Content = "Dark Theme";
               // b7tb1.Text = "Dark Theme";
                //ctheme = "BaseDark";
            }
            // get the theme from the window
            // get the theme from the current application
            var theme = ThemeManager.DetectAppStyle(System.Windows.Application.Current);

            if (color == null)
            {
                color = "Crimson";
            }
            ThemeManager.ChangeAppStyle(System.Windows.Application.Current,
                                        ThemeManager.GetAccent(color),

                                        ThemeManager.GetAppTheme(ctheme));

           // Properties.Settings.Default.color = color;
           // Properties.Settings.Default.Save();

        }

        public void ChangeAppStylelight()
        {
            // get the theme from the window
            var theme = ThemeManager.DetectAppStyle(System.Windows.Application.Current);

            // now set the Red accent and dark theme
            ThemeManager.ChangeAppStyle(System.Windows.Application.Current,
                                        ThemeManager.GetAccent("Steel"),
                                        ThemeManager.GetAppTheme("BaseLight"));
        }

        #endregion<<style>>

        #region<<buttons>>

        private void lbtest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DataRowView item1 = this.lbtest.SelectedItem as DataRowView;

            if (item1 != null)
            {
                //pkgwb.Visibility = Visibility.Hidden;
                //pkgwb.Dispose();
                Object[] items2 = item1.Row.ItemArray;
                string item = items2[4].ToString();
                item = item.Replace(".png", ".PKG");
                //this.label4.Content = items2[0].ToString();
                //lvpkginfo.Items.Clear();
                int n = 0;
                while (n < 4)
                {
                    string[] t1 = new string[] { "Name: ", "CID:    ", "Type:  ", "Size:   " };
                    //Object nob = items2[n];
                    string t = items2[n].ToString();
                    //lvpkginfo.Items.Add(t1[n] + t);
                    n++;
                }
                textBoxtid.Text= items2[7].ToString();
                textBoxcid.Text = items2[1].ToString();
                textBoxname.Text = items2[0].ToString();
                button_list.Visibility = Visibility.Visible;
                /*Please note you can always do this another methid just get the sfo somehow so we can work with it*/




            }
        }

        private void plist_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            /*
            DataRowView item1 = this.lbtest.SelectedItem as DataRowView;

            if (item1 != null)
            {
                Object[] items2 = item1.Row.ItemArray;
                string item = items2[4].ToString();
                item = item.Replace(".PNG", ".PKG");
                //this.label4.Content = items2[0].ToString();
                lvpkginfo.Items.Clear();
                string t = "";
                int n = 0;
                while (n < 4)
                {
                    string[] t1 = new string[] { "Name: ", "CID:    ", "Type:  ", "Size:   " };
                    //Object nob = items2[n];
                    t = items2[n].ToString();
                    lvpkginfo.Items.Add(t1[n] + t);
                    n++;
                }


                foreach (DataRow row in dtpkg2.Rows)
                {
                    int tname = Convert.ToInt32(row["count"]);
                    if (tname == n)
                    {

                        row["bl"] = "8.0";
                        row["tileh"] = "150";
                        row["tilew"] = "150";
                        row["column1w"] = "150";
                        row["column2w"] = "650";
                        row["roww"] = "50";
                        row["imags"] = "100";
                        VisitPlanItems.DataContext = dtpkg2.DefaultView;
                        lbtest.DataContext = dtpkg2.DefaultView;
                        lvpkginfo.DataContext = dtpkg2.DefaultView;
                    }
                    else
                    {
                        row["bl"] = "0.0";
                        row["tileh"] = "100";
                        row["tilew"] = "100";
                        row["column1w"] = "150";
                        row["column2w"] = "650";
                        row["roww"] = "30";
                        row["imags"] = "50";
                        VisitPlanItems.DataContext = dtpkg2.DefaultView;
                        lbtest.DataContext = dtpkg2.DefaultView;
                        lvpkginfo.DataContext = dtpkg2.DefaultView;
                    }
                    VisitPlanItems.DataContext = dtpkg2.DefaultView;
                    lbtest.DataContext = dtpkg2.DefaultView;
                    lvpkginfo.DataContext = dtpkg2.DefaultView;


                    //dispatcherTimer2.Start();
                    //VisitPlanItems.DataContext = dt2.DefaultView;
                }


            }


            lbtest.SelectedItem = (sender as Border).DataContext;*/
            //string k = lbtest.SelectedItem
            //if (!lbtest.IsFocused)
            // lbtest.Focus();
        }

        private void plist_MouseExit(object sender, System.Windows.Input.MouseEventArgs e)
        {
            /*
            DataRowView item1 = this.lbtest.SelectedItem as DataRowView;

            if (item1 != null)
            {
                Object[] items2 = item1.Row.ItemArray;
                string item = items2[4].ToString();
                item = item.Replace(".PNG", ".PKG");
                //this.label4.Content = items2[0].ToString();
                lvpkginfo.Items.Clear();
                string t = "";
                int n = 0;
                while (n < 4)
                {
                    string[] t1 = new string[] { "Name: ", "CID:    ", "Type:  ", "Size:   " };
                    //Object nob = items2[n];
                    t = items2[n].ToString();
                    lvpkginfo.Items.Add(t1[n] + t);
                    n++;
                }


                foreach (DataRow row in dtpkg2.Rows)
                {
                    int tname = Convert.ToInt32(row["count"]);
                    if (tname == n)
                    {

                        row["bl"] = "8.0";
                        row["tileh"] = "150";
                        row["tilew"] = "150";
                        row["column1w"] = "150";
                        row["column2w"] = "650";
                        row["roww"] = "50";
                        row["imags"] = "100";
                        // VisitPlanItems.DataContext = dtpkg2.DefaultView;
                        // lbtest.DataContext = dtpkg2.DefaultView;
                        //lvpkginfo.DataContext = dtpkg2.DefaultView;
                    }
                    else
                    {
                        row["bl"] = "0.0";
                        row["tileh"] = "100";
                        row["tilew"] = "100";
                        row["column1w"] = "150";
                        row["column2w"] = "650";
                        row["roww"] = "30";
                        row["imags"] = "50";
                        //VisitPlanItems.DataContext = dtpkg2.DefaultView;
                        //lbtest.DataContext = dtpkg2.DefaultView;
                        //lvpkginfo.DataContext = dtpkg2.DefaultView;
                    }


                    //dispatcherTimer2.Start();
                    //VisitPlanItems.DataContext = dt2.DefaultView;
                }


            }

            VisitPlanItems.DataContext = dtpkg2.DefaultView;
            lbtest.DataContext = dtpkg2.DefaultView;
            lvpkginfo.DataContext = dtpkg2.DefaultView;

            lbtest.SelectedItem = (sender as Border).DataContext;*/
            //string k = lbtest.SelectedItem
            //if (!lbtest.IsFocused)
            // lbtest.Focus();
        }

        private void button_1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ContextMenu_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ContextMenu_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion<<buttons>>

        #region<<downloads>>

        void Download_Icon(string id)
        {
            string url = "https://ps4database.io/dataApi?id=" + id + "&env=NP&method=meta";

            var meta = _download_serialized_json_data<Meta>(url);
            Icon[] new1 = meta.icons;
            if (new1 != null)
            {
                string newl = new1[0].icon;
                WebClient webClient = new WebClient();
                webClient.DownloadFile(newl, "tools/icons/" + id + ".png");
            }
            else
            {
                File.Copy("tools/resources/default.png", "tools/icons/" + id + ".png");
            }
        }

        private static T _download_serialized_json_data<T>(string url) where T : new()
        {
            using (var w = new WebClient())
            {
                var json_data = string.Empty;
                // attempt to download JSON data as a string
                try
                {
                    json_data = w.DownloadString(url);
                }
                catch (Exception) { }
                // if string with JSON data is not empty, deserialize it to class and return its instance 
                try
                {
                    return !string.IsNullOrEmpty(json_data) ? JsonConvert.DeserializeObject<T>(json_data) : new T();

                }
                catch (Exception)
                {
                    return new T();
                }
            }
        }

        #endregion<<downloads>>

        private void pkg_folder()
        {
            try
            {


                i = 0;

                dt2.Rows.Clear();
                //should be wise to do this
                dtpkg2.Clear();
                dtpkg.Clear();
                /*xDPx*/
                //clean data context each time
                System.Windows.Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Normal,
    (ThreadStart)delegate
    {
        lbtest.DataContext = null;

    });





                appPath = appPath.Replace("PS4_PKG_Linker.exe", "");
                pkg = appPath;
               
                

                dinfo = new DirectoryInfo(appPath);
                Files = dinfo.GetFiles("*.pkg");



                foreach (FileInfo file in Files)
                {
                    try
                    {

                        string tname = file.Name.Replace(".pkg", "");
                       
                        FileStream pkgFilehead = File.Open(file.FullName, FileMode.Open);
                        byte[] testmagic = new byte[0x04];
                        pkgFilehead.Read(testmagic, 0, 0x04);
                        pkgFilehead.Close();
                        byte[] magic = new byte[] { 0x7F, 0x43, 0x4E, 0x54 };
                        string pkgtype;
                        bool isretail = testmagic.SequenceEqual(magic);
                        if (isretail == true)
                        {
                            pkgtype = "Retail";
                        }

                        else
                        {
                            pkgtype = "";

                        }

                        

                        if (pkgtype != "")
                        {

                            FileStream pkgFile = File.Open(file.FullName, FileMode.Open);
                            byte[] cid = new byte[0x24];
                            pkgFile.Seek(0x40, SeekOrigin.Begin);
                            pkgFile.Read(cid, 0, 0x24);
                            scid = Encoding.ASCII.GetString(cid);
                            byte[] tid = new byte[0x0C];
                            pkgFile.Seek(0x47, SeekOrigin.Begin);
                            pkgFile.Read(tid, 0, 0x0C);
                            pkgFile.Close();
                            string stid = Encoding.ASCII.GetString(tid);

                            string sz = SizeSuffix(file.Length);
                            s = file.ToString();

                            if (s != "")
                            {
                                DataRow dr1 = dtpkg.NewRow();
                                dr1["IsSelected"] = false;
                                dr1["Name"] = s;
                                dr1["CID"] = scid;
                                dr1["type"] = pkgtype;
                                dr1["Size"] = sz;
                                dtpkg.Rows.Add(dr1);
                            }

                            string iconpath = appPath + "tools/icons/" + stid + ".png";

                            if (!File.Exists(iconpath))
                            {
                                Download_Icon(stid);
                            }

                            if (!File.Exists(iconpath))
                            {
                                iconpath = appPath + "tools/resources/default.png";
                            }
                            
                            DataRow dr = dtpkg2.NewRow();
                            dr["Name"] = s;
                            dr["CID"] = scid;
                            dr["type"] = pkgtype;
                            dr["size"] = sz;
                            dr["icon"] = iconpath;
                            dr["tool"] = "  " + s + "  " + scid + "  " + sz;
                            dr["count"] = i;
                            dr["bl"] = stid;
                            dr["tileh"] = "50";
                            dr["tilew"] = "800";
                            dr["column1w"] = "150";
                            dr["column2w"] = "650";
                            dr["roww"] = "25";
                            dr["imags"] = "50";
                            dr["text2s"] = "true";
                            
                            dtpkg2.Rows.Add(dr);
                            i++;
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        //invalid pkg or invalid item 
                    }


                }

                System.Windows.Application.Current.Dispatcher.Invoke(
                               DispatcherPriority.Normal,
                               (ThreadStart)delegate
                               {
                                   try
                                   {
                                       //i think its a safer bet to remove data context before adding into to it
                                       //VisitPlanItems.DataContext = null;
                                       lbtest.DataContext = null;
                                       //lvpkginfo.DataContext = null;

                                       //VisitPlanItems.DataContext = dtpkg2.DefaultView;
                                       lbtest.DataContext = dtpkg2.DefaultView;
                                       //lvpkginfo.DataContext = dtpkg2.DefaultView;

                                       /*Add a refresh item 
                                         Since we are running another thread call the refresh method
                                        */
                                       //VisitPlanItems.Items.Refresh();
                                       lbtest.Items.Refresh();
                                       //lvpkginfo.Items.Refresh();
                                   }
                                   catch (Exception ex)
                                   {

                                   }
                               });



            }
            catch (Exception ex)
            {
                /*added a try catch for this enitire method as well as something is causing it to fall over on some of the pkg's i tested */
            }
        }

    }
}
