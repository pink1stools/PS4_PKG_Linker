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
        DataTable[] dtpkg = new DataTable[1];
        DataTable dtpkg2 = new DataTable();
        DataTable dtlinks = new DataTable();
        //DataTable dt2 = new DataTable();


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

            dtpkg2.Columns.Add("plinks");

            dtlinks.Columns.Add("CID");
            dtlinks.Columns.Add("Name");
            dtlinks.Columns.Add("link");
            dtlinks.Columns.Add("icon");

            appPath = appPath.Replace("PS4_PKG_Linker.exe", "");

            Add_pkg();
            Load_Links();
            Wjson();
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

                color2.Background = Brushes.White;
                head_icon.Foreground = Brushes.White;
                //button7.Content = "Light Theme";
                //b7tb1.Text = "Light Theme";
                //ctheme = "BaseLight";
            }
            else
            {
                color2.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x25, 0x25, 0x25));
                head_icon.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x25, 0x25, 0x25));

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
                textBoxtid.Text = items2[7].ToString();
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
            //if (button1.Content.ToString() == "Start Server")
            if (b1tb1.Text == "Start Server")
            {
                /*port = textBox2.Text;
                textBox3.Text = comboBox1.Text;
                ip = textBox3.Text;

                textBox1.Text = ip;
                w_settings();
                w_s_conf();*/

                //b1tb1.Content = "Stop Server";
                b1tb1.Text = "Stop Server";

                //start_server();
                // MyNotifyIcon.Visible = true;
                Server_Running.BadgeBackground = Brushes.Green;
                Server_Running.BadgeForeground = Brushes.Green;
                //MyNotifyIcon.ShowBalloonTip(10000, "Server Started", "Server Started", System.Windows.Forms.ToolTipIcon.Info);
                // MyNotifyIcon.Visible = false;
            }
            else
            {
                //button1.Content = "Start Server";
                //b1tb1.Text = "Start Server";
                b1tb1.Text = "Start Server";
                //stop_server();
                //MyNotifyIcon.Visible = true;
                // b1tb1.Text = "Start Server";
                //dispatcherTimer2.Stop();
                //MyNotifyIcon.ShowBalloonTip(10000, "Server Stopped", "Server Stopped", System.Windows.Forms.ToolTipIcon.Info);
                // MyNotifyIcon.Visible = false;
                Server_Running.BadgeBackground = Brushes.Red;
                Server_Running.BadgeForeground = Brushes.Red;
            }
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


        private void Add_pkg()
        {
            DirectoryInfo dinfo;
            DirectoryInfo split_dinfo;
            FileInfo[] pkgs;
            FileInfo[] jsons;
            FileInfo[] spkgs;
            int i = 0;
            string s = null;
            string scid = null;

            try
            {
                dtpkg2.Clear();

                System.Windows.Application.Current.Dispatcher.Invoke(
                DispatcherPriority.Normal,
    (ThreadStart)delegate
    {
        lbtest.DataContext = null;
        dataGrid.DataContext = null;
    });

                dinfo = new DirectoryInfo(appPath);
                split_dinfo = new DirectoryInfo(appPath + "Split");
                pkgs = dinfo.GetFiles("*.pkg");
                jsons = dinfo.GetFiles("*.json");

                foreach (FileInfo file in pkgs)
                {
                    try
                    {
                        FileStream pkgFilehead = File.Open(file.FullName, FileMode.Open);
                        byte[] testmagic = new byte[0x04];
                        pkgFilehead.Read(testmagic, 0, 0x04);
                        pkgFilehead.Close();
                        byte[] magic = new byte[] { 0x7F, 0x43, 0x4E, 0x54 };
                        string pkgtype;
                        bool isretail = testmagic.SequenceEqual(magic);
                        if (isretail == true)
                        {
                            pkgtype = "PKG";

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
                            dr["text1s"] = "pkg";
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

                foreach (FileInfo file in jsons)
                {
                    try
                    {
                        string stid = "";
                        string text;
                        long fsz = 0;
                        var fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
                        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                        {
                            text = streamReader.ReadToEnd();
                        }
                        fileStream.Close();

                        var json = _serialized_json_data<JSON>(text);
                        Piece[] new1 = json.pieces;
                        if (new1 != null)
                        {
                            string newl = new1[0].url;
                            Uri uri = new Uri(newl);
                            string filename = System.IO.Path.GetFileName(uri.LocalPath); scid = filename.Remove(0x24, filename.Length - 0x24);

                            stid = scid.Remove(0, 7);
                            stid = stid.Remove(12, stid.Length - 12);
                            fsz = json.originalFileSize;

                        }

                        string sz = SizeSuffix(fsz);
                        s = file.ToString();



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
                        dr["type"] = "JSON";
                        dr["size"] = sz;
                        dr["icon"] = iconpath;
                        dr["tool"] = "  " + s + "  " + scid;
                        dr["count"] = i;
                        dr["bl"] = stid;
                        dr["tileh"] = "50";
                        dr["tilew"] = "800";
                        dr["column1w"] = "150";
                        dr["column2w"] = "650";
                        dr["roww"] = "25";
                        dr["imags"] = "50";
                        dr["text1s"] = "json";
                        dr["text2s"] = "true";

                        dtpkg2.Rows.Add(dr);
                        i++;


                    }
                    catch (Exception ex)
                    {
                        /*added a try catch for this enitire method as well as something is causing it to fall over on some of the pkg's i tested */
                    }
                }

                if (Directory.Exists(appPath + "Split"))
                {
                    spkgs = split_dinfo.GetFiles("*.pkg");
                    foreach (FileInfo file in spkgs)
                    {
                        s = file.ToString();
                        int starti = s.LastIndexOf("_");

                        string pname = s.Remove(starti, s.Length - starti);
                        string pnum = s.Remove(0, starti + 1);
                        int endi = pnum.LastIndexOf(".");
                        pnum = pnum.Remove(endi, 4);
                        if (pnum == "0" && File.Exists("Split/" + pname + "_1.pkg"))
                        {
                            try
                            {
                                FileStream pkgFilehead = File.Open(file.FullName, FileMode.Open);
                                byte[] testmagic = new byte[0x04];
                                pkgFilehead.Read(testmagic, 0, 0x04);
                                pkgFilehead.Close();
                                byte[] magic = new byte[] { 0x7F, 0x43, 0x4E, 0x54 };
                                string pkgtype;
                                bool isretail = testmagic.SequenceEqual(magic);
                                if (isretail == true)
                                {
                                    pkgtype = "Split";

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
                                    long fLength = file.Length;

                                    bool next = true;
                                    int q = 1;
                                    while (next == true)
                                    {
                                        if (File.Exists("Split/" + pname + "_" + q + " .pkg"))
                                        {
                                            fLength = fLength + File.Open("Split/" + pname + "_" + q + " .pkg", FileMode.Open).Length;
                                            q++;
                                        }
                                        if (!File.Exists("Split/" + pname + "_" + q + " .pkg"))
                                        {
                                            next = false;
                                        }
                                    }

                                    string sz = SizeSuffix(fLength);

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
                                    dr["text1s"] = "split";
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
                    }
                }

                if (File.Exists("Links.txt"))
                {
                    try
                    {
                        string stid = "";
                        string text;
                        var fileStream = new FileStream("Links.txt", FileMode.Open, FileAccess.Read);
                        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                        {
                            text = streamReader.ReadToEnd();
                        }
                        fileStream.Close();

                        var json = _serialized_json_data<LINKS>(text);
                        Plink[] new1 = json.plinks;
                        if (new1 != null)
                        {
                            foreach (Plink link in new1)
                            {
                                scid = link.content_id;
                                stid = scid.Remove(0, 7);
                                stid = stid.Remove(12, stid.Length - 12);

                                string iconlink = link.icon_link;
                                string filelink = link.link;
                                s = link.name;

                                string sz = GetFileSize(filelink);
                                //SizeSuffix(0);

                                string iconpath = appPath + "tools/icons/" + stid + ".png";

                                if (iconlink != "")
                                {


                                    if (!File.Exists(iconpath))
                                    {
                                        Download_Icon(stid);
                                    }

                                    if (!File.Exists(iconpath))
                                    {
                                        iconpath = appPath + "tools/resources/default.png";
                                    }
                                }
                                else
                                {
                                    if (new1 != null)
                                    {
                                        WebClient webClient = new WebClient();
                                        webClient.DownloadFile(iconlink, "tools/icons/" + stid + ".png");
                                    }
                                    else
                                    {
                                        File.Copy("tools/resources/default.png", "tools/icons/" + stid + ".png");
                                    }
                                }


                                DataRow dr = dtpkg2.NewRow();
                                dr["Name"] = s;
                                dr["CID"] = scid;
                                dr["type"] = "Link";
                                dr["size"] = sz;
                                dr["icon"] = iconpath;
                                dr["tool"] = "  " + s + "  " + scid;
                                dr["count"] = i;
                                dr["bl"] = stid;
                                dr["tileh"] = "50";
                                dr["tilew"] = "800";
                                dr["column1w"] = "150";
                                dr["column2w"] = "650";
                                dr["roww"] = "25";
                                dr["imags"] = "50";
                                dr["text1s"] = filelink;
                                dr["text2s"] = "true";

                                dtpkg2.Rows.Add(dr);
                                i++;


                            }
                        }



                    }
                    catch (Exception ex)
                    {
                        /*added a try catch for this enitire method as well as something is causing it to fall over on some of the pkg's i tested */
                    }
                }

                System.Windows.Application.Current.Dispatcher.Invoke(
                               DispatcherPriority.Normal,
                               (ThreadStart)delegate
                               {
                                   try
                                   {

                                       lbtest.DataContext = null;
                                       lbtest.DataContext = dtpkg2.DefaultView;

                                       lbtest.Items.Refresh();

                                       dataGrid.DataContext = null;
                                       dataGrid.DataContext = dtpkg2.DefaultView;

                                       dataGrid.Items.Refresh();

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

        private void Load_Links()
        {

            if (File.Exists("Links.txt"))
            {
                try
                {
                    string stid = "";
                    string text;
                    var fileStream = new FileStream("Links.txt", FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        text = streamReader.ReadToEnd();
                    }
                    fileStream.Close();

                    var json = _serialized_json_data<LINKS>(text);
                    Plink[] new1 = json.plinks;
                    if (new1 != null)
                    {
                        int i = 0;
                        foreach (Plink link in new1)
                        {
                            string cid = link.content_id;
                            stid = cid.Remove(0, 7);
                            stid = stid.Remove(12, stid.Length - 12);

                            string iconlink = link.icon_link;
                            string filelink = link.link;
                            string s = link.name;

                            string sz = GetFileSize(filelink);
                            //SizeSuffix(0);

                            string iconpath = link.icon_link;



                            DataRow dr = dtlinks.NewRow();
                            dr["Name"] = s;
                            dr["CID"] = cid;
                            dr["link"] = filelink;
                            dr["icon"] = iconpath;


                            dtlinks.Rows.Add(dr);
                            i++;

                            try
                            {

                                lvpkgsfo.DataContext = null;
                                lvpkgsfo.DataContext = dtlinks.DefaultView;

                                lvpkgsfo.Items.Refresh();

                            }
                            catch (Exception ex)
                            {

                            }

                        }
                    }



                }
                catch (Exception ex)
                {
                    /*added a try catch for this enitire method as well as something is causing it to fall over on some of the pkg's i tested */
                }
            }

        }

        private static T _serialized_json_data<T>(string url) where T : new()
        {

            var json_data = string.Empty;
            // attempt to download JSON data as a string
            try
            {
                json_data = url;
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

        private static string GetFileSize(string uriPath)
        {
            var webRequest = HttpWebRequest.Create(uriPath);
            webRequest.Method = "HEAD";

            using (var webResponse = webRequest.GetResponse())
            {

                var fileSize = webResponse.Headers.Get("Content-Length");
                string sz = SizeSuffix(Convert.ToInt64(fileSize));
                //var fileSizeInMegaByte = Math.Round(Convert.ToDouble(fileSize) / 1024.0 / 1024.0, 2);
                return sz;
            }
        }

        private void lvpkgsfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DataRowView item1 = this.lvpkgsfo.SelectedItem as DataRowView;

            if (item1 != null)
            {

                Object[] items2 = item1.Row.ItemArray;

                //this.label4.Content = items2[0].ToString();
                //lvpkginfo.Items.Clear();
                new_cid.Text = items2[0].ToString();
                new_name.Text = items2[1].ToString();
                new_link.Text = items2[2].ToString();
                new_icon.Text = items2[3].ToString();

                //button_list.Visibility = Visibility.Visible;
                /*Please note you can always do this another methid just get the sfo somehow so we can work with it*/

            }
        }

        private void Wjson()
        {
            DataSet dataSet = new DataSet("dataSet");
            dataSet.Namespace = "NetFrameWork";
            DataTable table = new DataTable("plinks");
            //DataColumn idColumn = new DataColumn("id", typeof(int));
            // idColumn.AutoIncrement = true;

            DataColumn itemColumn1 = new DataColumn("content_id");
            DataColumn itemColumn2 = new DataColumn("name");
            DataColumn itemColumn3 = new DataColumn("link");
            DataColumn itemColumn4 = new DataColumn("icon_link");
            //table.Columns.Add(idColumn);
            table.Columns.Add(itemColumn1);
            table.Columns.Add(itemColumn2);
            table.Columns.Add(itemColumn3);
            table.Columns.Add(itemColumn4);
            dataSet.Tables.Add(table);


            foreach (DataRow row in dtlinks.Rows)
            {
                ///Console.WriteLine("--- Row ---");

                DataRow newRow = table.NewRow();
                newRow["content_id"] = row.ItemArray[0];
                newRow["name"] = row.ItemArray[1];
                newRow["link"] = row.ItemArray[2];
                newRow["icon_link"] = row.ItemArray[3];
                table.Rows.Add(newRow);

            }

            dataSet.AcceptChanges();

            string json = JsonConvert.SerializeObject(dataSet, Formatting.None);

            /*
                        string json;
                        json = JsonConvert.SerializeObject(dtlinks);
                        dtpkg[0] = dtlinks;

                        json = JsonConvert.SerializeObject(dtpkg, Formatting.Undented);
                        JsonConvert.SerializeObject<LINKS>(json);
                        */
            using (StreamWriter writer = new StreamWriter("Links.txt"))
            {
                writer.Write(json);

            }
        }

        private void Add_Link()
        {
            if (new_name.Text != "" && new_cid.Text != "" && new_link.Text != "")
            {
                DataRow dr = dtlinks.NewRow();
                dr["Name"] = new_name.Text;
                dr["CID"] = new_cid.Text;
                dr["link"] = new_link.Text;
                dr["icon"] = new_icon.Text;

                dtlinks.Rows.Add(dr);
                try
                {

                    lvpkgsfo.DataContext = null;
                    lvpkgsfo.DataContext = dtlinks.DefaultView;

                    lvpkgsfo.Items.Refresh();

                }
                catch (Exception ex)
                {

                }
            }
        }

        private void Clear_Link()
        {
            new_name.Text = "";
            new_cid.Text = "";
            new_link.Text = "";
            new_icon.Text = "";

            this.lvpkgsfo.SelectedIndex = -1;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (new_name.Text != "" && new_cid.Text != "" && new_link.Text != "")
            {
                Add_Link();
                Wjson();
                Add_pkg();

                this.lvpkgsfo.SelectedIndex = this.lvpkgsfo.Items.Count - 1;
            }
            //Load_Links();
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            Clear_Link();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dtlinks.Rows.RemoveAt(this.lvpkgsfo.SelectedIndex);

                Wjson();
                Clear_Link();
                Add_pkg();
            }
            catch (Exception ex)
            {

            }
        }

        private void batch_Click(object sender, RoutedEventArgs e)
        {
            //dataGrid
            foreach(DataRow row in dtpkg2.Rows)
            {
                if(row.ItemArray[15].ToString() == "true")
                {
                    string link_name = row.ItemArray[0].ToString();
                    string file_type = row.ItemArray[14].ToString();
                    string link_type = row.ItemArray[2].ToString();

                    Send_File(link_name, link_type, file_type);
                }
            }
        }

        private void Send_File(string link_name, string link_type, string file_type)
        {
            if(link_type == "PKG")
            {
                //'http://<PS4 IP>:12800/api/install' --data '{"type":"direct","packages":["http://<local ip>:<local port>/UP1004-CUSA03041_00-REDEMPTION000002.pkg"]}'

            }
            else if (link_type == "JSON")
            {
                //'http://<PS4 IP>:12800/api/install' --data '{"type":"ref_pkg_url","url":"http://gs2.ww.prod.dl.playstation.net/gs2/appkgo/prod/CUSA02299_00/2/f_b215964ca72fc114da7ed38b3a8e16ca79bd1a3538bd4160b230867b2f0a92e0/f/UP9000-CUSA02299_00-MARVELSSPIDERMAN.json"}'

            }
            else if (link_type == "Split")
            {
                //'http://<PS4 IP>:12800/api/install' --data '{"type":"direct","packages":["http://<local ip>:<local port>/UP9000-CUSA02299_00-MARVELSSPIDERMAN-A0108-V0100_0.pkg","http://<local ip>:<local port>/UP9000-CUSA02299_00-MARVELSSPIDERMAN-A0108-V0100_1.pkg","http://<local ip>:<local port>/UP9000-CUSA02299_00-MARVELSSPIDERMAN-A0108-V0100_2.pkg"]}'

            }
            else if (link_type == "Link")
            {
                bool endsInJSON = file_type.EndsWith(".json") || file_type.EndsWith(".JSON");
                bool endsInPKG = file_type.EndsWith(".pkg") || file_type.EndsWith(".PKG");
                if(endsInJSON == true)
                {
                    //'http://<PS4 IP>:12800/api/install' --data '{"type":"ref_pkg_url","url":"http://gs2.ww.prod.dl.playstation.net/gs2/appkgo/prod/CUSA02299_00/2/f_b215964ca72fc114da7ed38b3a8e16ca79bd1a3538bd4160b230867b2f0a92e0/f/UP9000-CUSA02299_00-MARVELSSPIDERMAN.json"}'

                }
                else if (endsInPKG == true)
                {
                    //'http://<PS4 IP>:12800/api/install' --data '{"type":"direct","packages":["http://<local ip>:<local port>/UP1004-CUSA03041_00-REDEMPTION000002.pkg"]}'

                }
            }
        }
    }
}
