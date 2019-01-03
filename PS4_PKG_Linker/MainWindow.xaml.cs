using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;

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
using WPFFolderBrowser;
using System.Reflection;
using System.Text.RegularExpressions;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Resources;
using System.ComponentModel;
using AutoUpdaterDotNET;

namespace PS4_PKG_Linker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private System.Windows.Forms.NotifyIcon MyNotifyIcon;
        public DataTable[] dtpkg = new DataTable[1];
        public DataTable dtpkg2 = new DataTable();
        public DataTable dtlinks = new DataTable();
        public DataTable COLORS = new DataTable();

        TestObject t = new TestObject();
        Cursor Cursor2;

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        public System.Drawing.Icon[] icons = new System.Drawing.Icon[16];
        private int currentIcon = 0;
       public string ip = "";
       public string port = "";
        string server = "";
        string ps4_ip = "";
        string folder = "";

        public string color;
        public string ctheme = "BaseDark";

        int port1;

        string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;//System.Windows.Shapes.Path.GetDirectoryName(Application.ExecutablePath.);
        string serve_path;
        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        //Window Loading
        #region<<Loading>>

        public MainWindow()
        {
            InitializeComponent();
            LoadSettings();
            ChangeAppStyle();
            set_colors();
            //Check_serve();
            MyNotifyIcon = new System.Windows.Forms.NotifyIcon();
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            Stream myStream = myAssembly.GetManifestResourceStream("PS4_PKG_Linker.tools.resources." + "0.ico");
            System.Drawing.Icon bmp = new System.Drawing.Icon(myStream);
            MyNotifyIcon.Icon = bmp;
            MyNotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(MyNotifyIcon_MouseDoubleClick);
            MyNotifyIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(notifier_MouseDown);
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 250);
            
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            int q = 0;
            while (q <= 7)
            {
                Assembly myAssembly = Assembly.GetExecutingAssembly();
                Stream myStream = myAssembly.GetManifestResourceStream("PS4_PKG_Linker.tools.resources." + q + ".ico");
                System.Drawing.Icon bmp = new System.Drawing.Icon(myStream);
                icons[q] = bmp;
                q++;
            }
            int r = q - 1;
            while (q > 7 && q < 15)
            {

                Assembly myAssembly = Assembly.GetExecutingAssembly();
                Stream myStream = myAssembly.GetManifestResourceStream("PS4_PKG_Linker.tools.resources." + r + ".ico");
                System.Drawing.Icon bmp = new System.Drawing.Icon(myStream);
                icons[q] = bmp;
                q++;
                r--;
            }

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
            UgTask.Visibility = Visibility.Collapsed;
            AutoUpdater.ApplicationExitEvent += AutoUpdater_ApplicationExitEvent;
            //AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;

            scan();

            set_ip();
            Load();
            Set_cursor();
            Check_server();
            Get_Version();
            

            //this.child01.IsOpen = true;
        }

        #endregion<<>>

        //Background
        #region<<tray>>

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

            MyNotifyIcon.Icon = icons[currentIcon];
            currentIcon++;
            if (currentIcon == 15)
                currentIcon = 0;
        }

        void MyNotifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.Focus();
            this.Activate();
            this.WindowState = WindowState.Normal;
        }

        void notifier_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //if (button1.Content.ToString() == "Start Server")
            if (b1tb1.Text == "Start Server")
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {

                    ContextMenu menu = (ContextMenu)this.FindResource("NotifierContextMenu");
                    //menu.Items.
                    menu.IsOpen = true;

                }
                else if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {

                    ContextMenu menu = (ContextMenu)this.FindResource("NotifierContextMenu");
                    //menu.Items.
                    menu.IsOpen = false;

                }
            }
            else
            {

                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {

                    ContextMenu menu = (ContextMenu)this.FindResource("NotifierContextMenu2");
                    //menu.Items.
                    menu.IsOpen = true;

                }
                else if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {

                    ContextMenu menu = (ContextMenu)this.FindResource("NotifierContextMenu2");
                    //menu.Items.
                    menu.IsOpen = true;

                }
            }
        }

        private void changicon()
        {
            //if (button1.Content.ToString() == "Stop Server")
            if (b1tb1.Text == "Stop Server")
            {
                MyNotifyIcon.Visible = false;
                dispatcherTimer.Start();
                MyNotifyIcon.Visible = true;
            }
            else
            {
                MyNotifyIcon.Visible = false;
                dispatcherTimer.Stop();
                Assembly myAssembly = Assembly.GetExecutingAssembly();
                Stream myStream = myAssembly.GetManifestResourceStream("PS4_PKG_Linker.tools.resources." + "0.ico");
                System.Drawing.Icon bmp = new System.Drawing.Icon(myStream);
                MyNotifyIcon.Icon = bmp;
                MyNotifyIcon.Visible = true;
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                /* if (button1.Content.ToString() == "Stop Server")
                 {
                     dispatcherTimer.Start();
                 }
                 else
                 {
                     MyNotifyIcon.Icon = new System.Drawing.Icon("bin\\package.ico");
                 }*/
                //changicon();
               // this.ShowInTaskbar = false;
                //MyNotifyIcon.BalloonTipText = "Minimize Sucessful";
                //MyNotifyIcon.BalloonTipText = "Minimized the app ";
                //MyNotifyIcon.Visible = true;
                //MyNotifyIcon.ShowBalloonTip(10000, "Minimize Sucessful", "Minimize Sucessful", System.Windows.Forms.ToolTipIcon.Error);

            }
            else if (this.WindowState == WindowState.Normal)
            {
                dispatcherTimer.Stop();
                // MyNotifyIcon.Visible = true;
                this.ShowInTaskbar = true;
                MyNotifyIcon.Visible = false;
                //this.WindowState = WindowState.Maximized;
                // normButton.Visibility = Visibility.Visible;
                // maxButton.Visibility = Visibility.Hidden;
                /*ret2.Visibility = Visibility.Visible;
                ret3.Visibility = Visibility.Hidden;
                ret2.Width = 10;
                ret3.Width = 0;*/
                //ret1.
            }
            else if (this.WindowState == WindowState.Maximized)
            {
                //this.WindowState = WindowState.Normal;
                //maxButton.Visibility = Visibility.Visible;
                //normButton.Visibility = Visibility.Hidden;
                /* ret2.Visibility = Visibility.Hidden;
                 ret3.Visibility = Visibility.Visible;
                 ret2.Width = 0;
                 ret3.Width = 10;*/

            }
        }
        
        #endregion<<>>

        #region<<settings>>
        private void LoadSettings()
        {
            ip = Properties.Settings.Default.ip;
            port = Properties.Settings.Default.port;
            ctheme = Properties.Settings.Default.theme;
            color = Properties.Settings.Default.color;
            server = Properties.Settings.Default.server;
            folder = Properties.Settings.Default.folder;
            ps4_ip = Properties.Settings.Default.ps4_ip;
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.ip = ip;
            Properties.Settings.Default.port = port;
            Properties.Settings.Default.theme = ctheme;
            Properties.Settings.Default.color = color;
            Properties.Settings.Default.server = server;
            Properties.Settings.Default.folder = folder;
            Properties.Settings.Default.ps4_ip = ps4_ip;
            Properties.Settings.Default.Save();
        }

        #endregion<<settings>>

        #region<<set_up>>

        public void Get_Version()
        {

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            FVersion.Content += "  v" + version;
            

        }

        

        public void Set_cursor()
        {
           
                StreamResourceInfo sriCurs = Application.GetResourceStream(
                    new Uri("/tools/resources/aero-middle-finger-2.cur", UriKind.Relative));
                // Cursor1 = new Cursor();
                Cursor2 = new Cursor(sriCurs.Stream);

                t.Cursor1 = Cursors.Hand;
            
            sva.DataContext = t;
            spl.DataContext = t;
        }

        public void Check_serve()
        {
            // Use ProcessStartInfo class.
           ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            //startInfo.WorkingDirectory = @"C://";
            startInfo.RedirectStandardError = true;
            //startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = " PKGserve";//@"cmd.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "-v";//"/c PKGserve -v";//"/c serve -v";
            Process exeProcess = Process.Start(startInfo);
            string output = exeProcess.StandardOutput.ReadToEnd();
            exeProcess.WaitForExit();


            if(!output.Contains("'serve' is not recognized as an internal or external command,"))
            {
                Nodejs_serve.IsEnabled = true;
            }
            else
            {
                Nodejs_serve.IsEnabled = false;
            }
        }

        public void Check_server()
        {
            Nodejs_serve.IsEnabled = true;

            if (Nodejs_serve.IsEnabled == true)
            {
                Nodejs_serve.IsChecked = true;
            }
            else
            {
                Uniserver.IsChecked = true;
            }
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

        public static int GetAvailablePort(int startingPort)
        {
            IPEndPoint[] endPoints;
            List<int> portArray = new List<int>();

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

            //getting active connections
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            portArray.AddRange(from n in connections
                               where n.LocalEndPoint.Port >= startingPort
                               select n.LocalEndPoint.Port);

            //getting active tcp listners - WCF service listening in tcp
            endPoints = properties.GetActiveTcpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= startingPort
                               select n.Port);

            //getting active udp listeners
            endPoints = properties.GetActiveUdpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= startingPort
                               select n.Port);

            portArray.Sort();

            for (int i = startingPort; i < UInt16.MaxValue; i++)
                if (!portArray.Contains(i))
                    return i;

            return 0;
        }

        private void scan()
        {
           if(Directory.Exists(folder))
            {
                Add_pkg();
                Load_Links();
                Wjson();
            }
        }

        private void Load()
        {
            textBox1.Text = ps4_ip;
            textBoxfile.Text = folder;
        }

        private void set_ip()
        {
            port1 = GetAvailablePort(80);
            port = port1.ToString();
            textBox2.Text = port;
            g_ip();


        }

        private void g_ip()
        {
            string hostName = Dns.GetHostName();
            ip = Dns.GetHostByName(hostName).AddressList[0].ToString();
            //ip = Dns.GetHostEntry(hostName).AddressList[0].ToString();

            // Get host name
            String strHostName = Dns.GetHostName();

            // Find host by name
            IPHostEntry iphostentry = Dns.GetHostByName(strHostName);


            if (iphostentry.AddressList.Count() != 0)
            {
                //comboBox1.Text.
            }
            // Enumerate IP addresses
            foreach (IPAddress ipaddress in iphostentry.AddressList)
            {
                comboBox1.Items.Add(ipaddress);

            }
            if (comboBox1.Items[0].ToString() != "999.999.999.999")
            {
                ip = comboBox1.Items[0].ToString();
                comboBox1.Text = ip;
            }

        }

        #endregion<<>>

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

        #region<<server>>

        private void start_node_server()
        {

            

            // Use ProcessStartInfo class.
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            //startInfo.WorkingDirectory = @"C://";
            startInfo.RedirectStandardError = true;
            //startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "PKGserve";//@"cmd.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "-l tcp://" + ip + ":" + port; //" /c PKGserve -l tcp://" + ip + ":" + port;//"/c serve -l tcp://" + ip +":" + port;
            Process exeProcess = Process.Start(startInfo);

            Thread.Sleep(100);
            test_server();
            changicon();

            
        }

        private void stop_node_server()
        {

            Process[] processes = Process.GetProcessesByName("PKGserve");//"node");
             foreach (var process in processes)
             {
                 process.Kill();
             }
            Process[] processes2 = Process.GetProcessesByName("ServiceHub.Host.Node.x86");
            foreach (var process in processes2)
            {
                process.Kill();
            }

            changicon();
        }


        private void start_server()
        {

            Server_conf();

            // Use ProcessStartInfo class.
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            //startInfo.WorkingDirectory = ;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "tools/UniServerZ/UniController.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "start_apache";
            Process exeProcess = Process.Start(startInfo);

            exeProcess.WaitForExit();
            test_server();
            changicon();
            //MyNotifyIcon.Visible = true;



        }

        private void stop_server()
        {


            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.WorkingDirectory = appPath;

            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "tools/UniServerZ/UniController.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "stop_apache";
            // Start the process with the info we specified.
            // Call WaitForExit and then the using-statement will close.
            Process exeProcess = Process.Start(startInfo);

            changicon();
            //MyNotifyIcon.Visible = true;
            /* int procID;
             Process[] processes;
             string procName = "cmd";
             processes = Process.GetProcessesByName(procName);
             foreach (Process proc in processes)
             {
                 string temps = proc.MainWindowTitle;
                 int tempi = proc.Id;
                 if (proc.MainWindowTitle == "Apache 2.4.6 Portable...")
                 {
                     procID = tempi;
                     Process tempProc = Process.GetProcessById(procID);
                     tempProc.CloseMainWindow();
                     tempProc.WaitForExit();
                 }
             }*/

            /*8 Process[] processes = Process.GetProcessesByName("HHTCtrlp");
             foreach (var process in processes)
             {
                 process.Kill();
             }*/
        }

        private void test_server()
        {
            int procID;
            Process[] processes;
            string procName = "";
            if (Uniserver.IsChecked == true)
            {
                procName = "httpd_z";
            }
            else if (Nodejs_serve.IsChecked == true)
            {
                procName = "PKGserve";//"node";
            }
            

            processes = Process.GetProcessesByName(procName);
            if (processes.Length == 2 || processes.Length == 1)
            {
                
                b1tb1.Text = "Stop Server";
                MyNotifyIcon.ShowBalloonTip(10000, "Server Started", "Server Started", System.Windows.Forms.ToolTipIcon.Info);
                
            }
            
            else
            {
                b1tb1.Text = "Start Server";
                MyNotifyIcon.ShowBalloonTip(10000, "Error Starting Server", "Error Starting Server", System.Windows.Forms.ToolTipIcon.Error);

            }
        }

        private void Server_conf()
        {


            if (File.Exists("tools\\UniServerZ\\home\\us_config\\us_config.ini"))
            {
                StreamReader conf;
                conf = new StreamReader("tools\\UniServerZ\\home\\us_config\\us_config.ini");
                string content = conf.ReadToEnd();
                conf.Close();
                content = Regex.Replace(content, "Nag_user=true", "Nag_user=false");

                StreamWriter writer;
                writer = new StreamWriter("tools\\UniServerZ\\home\\us_config\\us_config.ini");
                writer.Write(content);
                writer.Close();
            }

            string old1 = ""; string old2 = ""; string old3 = ""; string new1 = ""; string new2 = ""; string new3 = "";
            if (File.Exists("tools\\UniServerZ\\home\\us_config\\us_user.ini"))
            {
                string appPath2;
                serve_path = textBoxfile.Text;
                if (Directory.Exists(serve_path))
                {
                    appPath2 = appPath.Replace("\\", "/"); //serve_path.Replace("\\", "/");
                }
                else
                {
                    appPath2 = appPath.Replace("\\", "/");
                }
                StreamReader conf2;
                conf2 = new StreamReader("tools\\UniServerZ\\home\\us_config\\us_user.ini");
                string content = conf2.ReadToEnd();
                conf2.Close();
                StreamReader conf;
                conf = new StreamReader("tools\\UniServerZ\\home\\us_config\\us_user.ini");

                string line;
                if (content.Contains("US_ROOTF_WWW=") && content.Contains("AP_PORT="))
                {
                    while ((line = conf.ReadLine()) != null)
                    {

                        if (line.Contains("AP_PORT=") && !line.Contains("; "))
                        {
                            old1 = line;
                            //old1 = old1.Replace("AP_PORT=", "");
                            //old1 = old1.Replace(" ", "");
                            new1 = "AP_PORT=" + port;
                            line = new1;
                        }
                        else if (line.Contains("US_ROOTF_WWW=") && !line.Contains(";"))
                        {
                            old2 = line;
                            //old2 = old2.Replace("US_ROOTF_WWW=", "");
                            //old2 = old2.Replace(" ", "");
                            new2 = "US_ROOTF_WWW=" + appPath2;
                            line = new2;
                        }
                        else if (line.Contains("US_SERVERNAME=") && !line.Contains(";"))
                        {
                            old3 = line;
                            //old3 = old3.Replace("US_SERVERNAME=", "");
                            //old3 = old3.Replace(" ", "");
                            new3 = "US_SERVERNAME=" + ip;
                            line = new3;
                        }
                        //writer.WriteLine(line);
                    }

                    conf.Close();
                    content = content.Replace(old1, new1);
                    content = content.Replace(old2, new2);
                    content = content.Replace(old3, new3);

                    StreamWriter writer;
                    writer = new StreamWriter("tools\\UniServerZ\\home\\us_config\\us_user.ini");
                    writer.Write(content);
                    writer.Close();
                }
                else
                {
                    //Server_conf();
                }
            }


        }

        private void check_processes()
        {

            int procID;
            Process[] processes;
            string procName = "httpd_z";
            //processes = Process.GetProcesses();
            processes = Process.GetProcessesByName(procName);
            foreach (Process proc in processes)
            {
                string temps = proc.MainWindowTitle;
                int tempi = proc.Id;
                if (proc.MainWindowTitle == "")
                {
                    procID = tempi;
                    Process tempProc = Process.GetProcessById(procID);
                    tempProc.CloseMainWindow();
                    tempProc.Kill();

                    tempProc.WaitForExit();
                }
            }

            Process[] processes2 = Process.GetProcessesByName("PKGserve");//("node");
            foreach (var process in processes2)
            {
                process.Kill();
                process.WaitForExit();
            }
            Process[] processes3 = Process.GetProcessesByName("ServiceHub.Host.Node.x86");
            foreach (var process in processes3)
            {
                process.Kill();
                process.WaitForExit();
            }


        }

        #endregion<<>>

        #region<<loading>>

        public void Add_pkg()
        {
            DirectoryInfo dinfo;
            DirectoryInfo split_dinfo;
            FileInfo[] pkgs;
            FileInfo[] jsons;
            FileInfo[] spkgs;
            int i = 0;
            string s = null;
            string scid = null;
            if (Directory.Exists(appPath + "PS4\\"))
            {
                //Directory.CreateDirectory(appPath + "PS4\\");

                try
                {
                   // lbtest.Items.Clear();
                    dtpkg2.Clear();

                    System.Windows.Application.Current.Dispatcher.Invoke(
                    DispatcherPriority.Normal,
        (ThreadStart)delegate
        {
            lbtest.DataContext = null;
            dataGrid.DataContext = null;
        });

                    dinfo = new DirectoryInfo(appPath + "PS4\\");
                    split_dinfo = new DirectoryInfo(appPath + "PS4/Split/");
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
                                dr["tool"] = "  " + s + "\n  " + scid + "  " + sz;
                                dr["count"] = i;
                                dr["bl"] = stid;
                                dr["tileh"] = "";
                                dr["tilew"] = "";
                                dr["column1w"] = "";
                                dr["column2w"] = "";
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
                            dr["tileh"] = "";
                            dr["tilew"] = "";
                            dr["column1w"] = "";
                            dr["column2w"] = "";
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

                    if (Directory.Exists(appPath + "PS4/Split/"))
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
                                        dr["tileh"] = "";
                                        dr["tilew"] = "";
                                        dr["column1w"] = "";
                                        dr["column2w"] = "";
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
                                    dr["tileh"] = "";
                                    dr["tilew"] = "";
                                    dr["column1w"] = "";
                                    dr["column2w"] = "";
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
        }

        private void Load_Links()
        {
            dtlinks.Rows.Clear();
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

        public void Wjson()
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
            if (!File.Exists("Links.txt"))
            {
                File.Create("Links.txt");
            }
            using (StreamWriter writer = new StreamWriter("Links.txt"))
            {
                writer.Write(json);

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

        #endregion<<>>

        #region<<menu>>

        private void ContextMenu_MouseLeave(object sender, MouseEventArgs e)
        {


            ContextMenu cm = this.TryFindResource("NotifierContextMenu") as ContextMenu;
            if (cm != null)
                cm.ReleaseMouseCapture();

        }

        private void ContextMenu_Loaded(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = sender as ContextMenu;
            Border border = GetChildOfType<Border>(cm);
            border.MouseLeave += Border_MouseLeave;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            //do something here...
            ContextMenu cm = this.TryFindResource("NotifierContextMenu") as ContextMenu;
            if (cm != null)
                cm.ReleaseMouseCapture();

            ContextMenu cm2 = this.TryFindResource("NotifierContextMenu2") as ContextMenu;
            if (cm2 != null)
                cm2.ReleaseMouseCapture();


        }

        private static T GetChildOfType<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null)
                return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }

        private void Menu_Open_list(object sender, RoutedEventArgs e)
        {

            //MessageBox.Show("Open");
            this.Focus();
            this.Activate();
            this.WindowState = WindowState.Normal;
            Dispatcher.BeginInvoke((Action)(() => metroAnimatedTabControl.SelectedIndex = 0));

            //this.tabItem1.IsSelected = true;
            //MyNotifyIcon.Visible = false;
            this.ShowInTaskbar = true;


        }

        private void Menu_Open_settings(object sender, RoutedEventArgs e)
        {
            this.Focus();
            this.Activate();
            //MessageBox.Show("Open");
            this.WindowState = WindowState.Normal;
            Dispatcher.BeginInvoke((Action)(() => metroAnimatedTabControl.SelectedIndex = 1));

            //this.tabItem2.IsSelected = true;
            //  MyNotifyIcon.Visible = false;
            this.ShowInTaskbar = true;


        }

        private void Menu_Open_tools(object sender, RoutedEventArgs e)
        {
            this.Focus();
            this.Activate();
            //MessageBox.Show("Open");
            this.WindowState = WindowState.Normal;
            Dispatcher.BeginInvoke((Action)(() => metroAnimatedTabControl.SelectedIndex = 1));

            // this.tabItem3.IsSelected = true;
            //  MyNotifyIcon.Visible = false;
            this.ShowInTaskbar = true;


        }

        private void Menu_Open_help(object sender, RoutedEventArgs e)
        {
            this.Focus();
            this.Activate();
            //MessageBox.Show("Open");
            this.WindowState = WindowState.Normal;
            //this..IsSelected = true;
            //   MyNotifyIcon.Visible = false;
            //this.tabItem2.IsSelected = true;
            Dispatcher.BeginInvoke((Action)(() => metroAnimatedTabControl.SelectedIndex = 2));
            this.ShowInTaskbar = true;


        }

        private void Menu_Start_Stop(object sender, RoutedEventArgs e)
        {

            //MessageBox.Show("Start_Stop");
            if (b1tb1.Text == "Start Server")
            // if (button1.Content.ToString() == "Start Server")
            {
                port = textBox2.Text;
                ip = textBox3.Text;
                textBox1.Text = ip;

                b1tb1.Text = "Stop Server";
                //button1.Content = "Stop Server"; 
                MyNotifyIcon.ShowBalloonTip(10000, "Server Started", "Server Started", System.Windows.Forms.ToolTipIcon.Info);
                changicon();
                start_server();
            }
            else
            {
                b1tb1.Text = "Start Server";
                //button1.Content = "Start Server"; 
                MyNotifyIcon.ShowBalloonTip(10000, "Server Stopped", "Server Stopped", System.Windows.Forms.ToolTipIcon.Info);
                changicon();
                stop_server();
            }

        }

        private void Menu_Close(object sender, RoutedEventArgs e)
        {

            //MessageBox.Show("Close");
            App.Current.Shutdown();
        }

        private void Exit_Tray_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            this.ShowInTaskbar = false;
            changicon();
        }

        #endregion<<menu>>

        #region<<close>>

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;


            ContextMenu menu = (ContextMenu)this.FindResource("NotifierContextMenu3");
            //menu.Items.
            menu.IsOpen = true;

            //SaveSettings();
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            SaveSettings();
            MyNotifyIcon.Visible = false;
            check_processes();
        }

        #endregion<<>>


        //Tabs
        #region<<head>>

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            ps4_ip = textBox1.Text;
        }

        private void textBox1_LostFocus(object sender, RoutedEventArgs e)
        {
          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Uniserver.IsChecked == true)
            {
                if (b1tb1.Text == "Start Server")
                {
                    port = textBox2.Text;
                    

                    b1tb1.Text = "Stop Server";
                    ip = comboBox1.Text;
                    start_server();
                    

                }
                else
                {
                    

                    b1tb1.Text = "Start Server";
                    stop_server();

                    MyNotifyIcon.ShowBalloonTip(10000, "Server Stopped", "Server Stopped", System.Windows.Forms.ToolTipIcon.Info);
                    

                }
            }
            else if(Nodejs_serve.IsChecked == true)
            {
                if (b1tb1.Text == "Start Server")
                {
                    port = textBox2.Text;


                    b1tb1.Text = "Stop Server";
                    ip = comboBox1.Text;
                    start_node_server();


                }
                else
                {


                    b1tb1.Text = "Start Server";
                    stop_node_server();

                    MyNotifyIcon.ShowBalloonTip(10000, "Server Stopped", "Server Stopped", System.Windows.Forms.ToolTipIcon.Info);


                }
            }
        }

        private void textBoxfile_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            WPFFolderBrowserDialog test = new WPFFolderBrowserDialog();
            var tes2 = test.ShowDialog();
            // test.ShowDialog();
            if (tes2 == true)
            {



                if (test.FileName != "" && Directory.Exists(test.FileName))
                {
                    if (Directory.Exists("PS4"))
                    {
                        Directory.Delete("PS4", true);
                    }

                    folder = test.FileName;
                    textBoxfile.Text = folder;

                    make_shortcut(folder);
                    
                    scan();
                }
            }
        }

        #endregion<<>>



        #region<<pkg list>>

        private void lbtest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            DataRowView item1 = this.lbtest.SelectedItem as DataRowView;

            if (item1 != null)
            {

                Object[] items2 = item1.Row.ItemArray;

                textBoxtid.Text = items2[7].ToString();
                textBoxcid.Text = items2[1].ToString();
                textBoxname.Text = items2[0].ToString();
                textBoxfile_type.Text = items2[14].ToString();
                textBoxlink_type.Text = items2[2].ToString();
                button_list.Visibility = Visibility.Visible;
                if (ps4_ip != "")
                {
                    string out1 = Check_game();
                    if (out1 == "TimedOut")
                    {
                        try
                        {
                            LongRunningOperationAsync("Request TimedOut", "Request to " + ps4_ip + " TimedOut");
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    if (out1 == "BadIP")
                    {
                        try
                        {
                            LongRunningOperationAsync("Invalid IP", ps4_ip + " is not a valide ip address");
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

                if(items2[8].ToString() != "")
                {
                    textBoxtask.Text = items2[8].ToString();
                    UgTask.Visibility = Visibility.Visible;
                }
                else
                {
                    textBoxtask.Text = "";
                    UgTask.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void button_1_Click(object sender, RoutedEventArgs e)
        {
            //  curl--data '{"title_id":"CUSA09311"}' 'http://<PS4 IP>:12800/api/is_exists'

            if(textBoxtid.Text != "")
            { 
            string out1 = Check_game();
            }

        }

        private void button_2_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxtid.Text != "")
            {
                string link_name =  textBoxname.Text;
                string file_type = textBoxfile_type.Text;
                string link_type = textBoxlink_type.Text;
                string out1 = Send_File(link_name, link_type, file_type);
                if (out1 != "")
                {

                }
            }
        }

        private string Check_game()
        {
            //  curl--data '{"title_id":"CUSA09311"}' 'http://<PS4 IP>:12800/api/is_exists'
            string tid = textBoxtid.Text.Remove(9, 3);
            string out1 = RPI.Send(ps4_ip, "is_exists", "{\"title_id\":\"" + tid + "\"}");
            
                if (out1 != "TimedOut" && out1 != "BadIP")
                {
                try
                {
                    var json = _serialized_json_data<Read_Exists>(out1);
                    string status = json.status;
                    string error = json.error;
                    string exists = json.exists;
                    //string size = GetFileSize(json.size);
                    if (json.exists == "true")
                    {
                        try
                        {
                            button_1.Content = null;
                            button_1.Content = textBoxtid.Text + " is on the PS4";
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                        try
                        {
                            button_1.Content = null;
                            button_1.Content = textBoxtid.Text + " is not on the PS4";
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

                catch (Exception ex)
                {
                    /*added a try catch for this enitire method as well as something is causing it to fall over on some of the pkg's i tested */
                }
                }

                if (out1 == "TimedOut" || out1 == "BadIP")
                {
                    try
                    {
                        button_1.Content = null;
                        button_1.Content = "Check if app exists";
                    }
                    catch (Exception ex)
                    {

                    }
                }

            
            return (out1);
        }

        public string Send_File(string link_name, string link_type, string file_type)
        {
            string out1 = "";
            if (link_type == "PKG")
            {
                link_name = "PS4//" + link_name;
                //'http://<PS4 IP>:12800/api/install' --data '{"type":"direct","packages":["http://<local ip>:<local port>/UP1004-CUSA03041_00-REDEMPTION000002.pkg"]}'

                //string tid = textBoxtid.Text.Remove(9, 3);
                return RPI.Send(ps4_ip, "install", "{ \"type\":\"direct\",\"packages\":[\"http://" + ip + ":" + port + "//" + link_name + "\"]}");   //"{\"type\":\"" + tid + "\"}"); 
                
            }
            else if (link_type == "JSON")
            {
                link_name = "PS4//" + link_name;
                //'http://<PS4 IP>:12800/api/install' --data '{"type":"ref_pkg_url","url":"http://gs2.ww.prod.dl.playstation.net/gs2/appkgo/prod/CUSA02299_00/2/f_b215964ca72fc114da7ed38b3a8e16ca79bd1a3538bd4160b230867b2f0a92e0/f/UP9000-CUSA02299_00-MARVELSSPIDERMAN.json"}'

                //string tid = textBoxtid.Text.Remove(9, 3);
                return RPI.Send(ps4_ip, "install", "{ \"type\":\"ref_pkg_url\",\"url\":[\"http://" + ip + ":" + port + "//" + link_name + "\"]}");   //"{\"type\":\"" + tid + "\"}"); 

            }
            else if (link_type == "Split")
            {
                //'http://<PS4 IP>:12800/api/install' --data '{"type":"direct","packages":["http://<local ip>:<local port>/UP9000-CUSA02299_00-MARVELSSPIDERMAN-A0108-V0100_0.pkg","http://<local ip>:<local port>/UP9000-CUSA02299_00-MARVELSSPIDERMAN-A0108-V0100_1.pkg","http://<local ip>:<local port>/UP9000-CUSA02299_00-MARVELSSPIDERMAN-A0108-V0100_2.pkg"]}'

                //string tid = textBoxtid.Text.Remove(9, 3);
                return RPI.Send(ps4_ip, "install", "{ \"type\":\"direct\",\"packages\":[\"http://" + ip + ":" + port + "//" + link_name + "\"]}");   //"{\"type\":\"" + tid + "\"}"); 

            }
            else if (link_type == "Link")
            {
                bool endsInJSON = file_type.EndsWith(".json") || file_type.EndsWith(".JSON");
                bool endsInPKG = file_type.EndsWith(".pkg") || file_type.EndsWith(".PKG");
                if (endsInJSON == true)
                {
                    //'http://<PS4 IP>:12800/api/install' --data '{"type":"ref_pkg_url","url":"http://gs2.ww.prod.dl.playstation.net/gs2/appkgo/prod/CUSA02299_00/2/f_b215964ca72fc114da7ed38b3a8e16ca79bd1a3538bd4160b230867b2f0a92e0/f/UP9000-CUSA02299_00-MARVELSSPIDERMAN.json"}'

                    //string tid = textBoxtid.Text.Remove(9, 3);
                    return RPI.Send(ps4_ip, "install", "{ \"type\":\"ref_pkg_url\",\"url\":[\"http://" + ip + ":" + port + "//" + link_name + "\"]}");   //"{\"type\":\"" + tid + "\"}"); 

                }
                else if (endsInPKG == true)
                {
                    //'http://<PS4 IP>:12800/api/install' --data '{"type":"direct","packages":["http://<local ip>:<local port>/UP1004-CUSA03041_00-REDEMPTION000002.pkg"]}'

                   // string tid = textBoxtid.Text.Remove(9, 3);
                    return RPI.Send(ps4_ip, "install", "{ \"type\":\"direct\",\"packages\":[\"http://" + ip + ":" + port + "//" + link_name + "\"]}");   //"{\"type\":\"" + tid + "\"}"); 

                }
            }
            return out1;

        }

        private string Uninstall_game()
        {
            string tid = textBoxtid.Text.Remove(9, 3);
            string out1 = RPI.Send(ps4_ip, "uninstall_game", "{\"title_id\":\"" + tid + "\"}");

            try
            {
                var json = _serialized_json_data<Read_Exists>(out1);
                string status = json.status;
                string error = json.error;
                string exists = json.exists;
                //string size = GetFileSize(json.size);
                if (json.status == "success")
                {
                    try
                    {
                        LongRunningOperationAsync("Success", "Uninstalled " + textBoxtid.Text);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                    try
                    {
                        LongRunningOperationAsync("Error", "Error uninstalling " + textBoxtid.Text);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                /*added a try catch for this enitire method as well as something is causing it to fall over on some of the pkg's i tested */
            }

            return (out1);

        }
       
        private string Uninstall_patch()
        {

            string tid = textBoxtid.Text.Remove(9, 3);
            string out1 = RPI.Send(ps4_ip, "uninstall_patch", "{\"title_id\":\"" + tid + "\"}");

            try
            {
                var json = _serialized_json_data<Read_Exists>(out1);
                string status = json.status;
                string error = json.error;
                string exists = json.exists;
                //string size = GetFileSize(json.size);
                if (json.status == "success")
                {
                    try
                    {
                        LongRunningOperationAsync("Success", "Uninstalled " + textBoxtid.Text);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                    try
                    {
                        LongRunningOperationAsync("Error", "Error uninstalling " + textBoxtid.Text);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                /*added a try catch for this enitire method as well as something is causing it to fall over on some of the pkg's i tested */
            }

            return (out1);
        }

        private string Uninstall_theme()
        {

           // string tid = textBoxtid.Text.Remove(9, 3);
            string out1 = RPI.Send(ps4_ip, "uninstall_theme", "{\"content_id\":\"" + textBoxcid.Text + "\"}");


            try
            {
                var json = _serialized_json_data<Read_Exists>(out1);
                string status = json.status;
                string error = json.error;
                string exists = json.exists;
                //string size = GetFileSize(json.size);
                if (json.status == "success")
                {
                    try
                    {
                        LongRunningOperationAsync("Success", "Uninstalled " + textBoxcid.Text);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                    try
                    {
                        LongRunningOperationAsync("Error", "Error uninstalling " + textBoxcid.Text);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                /*added a try catch for this enitire method as well as something is causing it to fall over on some of the pkg's i tested */
            }

            return (out1);
        }

        private string Uninstall_ac()
        {

            //string tid = textBoxtid.Text.Remove(9, 3);
            string out1 = RPI.Send(ps4_ip, "uninstall_ac", "{\"content_id\":\"" + textBoxcid.Text + "\"}");


            try
            {
                var json = _serialized_json_data<Read_Exists>(out1);
                string status = json.status;
                string error = json.error;
                string exists = json.exists;
                //string size = GetFileSize(json.size);
                if (json.status == "success")
                {
                    try
                    {
                        LongRunningOperationAsync("Success", "Uninstalled " + textBoxcid.Text);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                    try
                    {
                        LongRunningOperationAsync("Error", "Error uninstalling " + textBoxcid.Text);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                /*added a try catch for this enitire method as well as something is causing it to fall over on some of the pkg's i tested */
            }

            return (out1);
        }

        public async Task<int> LongRunningOperationAsync(string title, string message) // assume we return an int from this long running operation 
        {
            var controller = await this.ShowMessageAsync(title, message);

            return 1;
        }

        #endregion<<>>

        #region<<server settings>>

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            Window_server win2 = new Window_server("http://127.0.0.1:" + port + "/server-info", color, ctheme);
            win2.Show();
            //Process.Start(new ProcessStartInfo("http://127.0.0.1:" + port + "/server-info"));

        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {

            Process.Start(new ProcessStartInfo("http://" + ip + ":" + port + "/server-status.lua"));

        }

        private void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (b1tb1.Text == "Stop Server")
            {

                MyNotifyIcon.ShowBalloonTip(10000, "Server Restart Required", "Server Restart Required for Changes to Take Effect", System.Windows.Forms.ToolTipIcon.Info);

            }
        }

        private void textBox2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            port1 = GetAvailablePort(80);
            port = port1.ToString();
            textBox2.Text = port;
        }

        #endregion<<>>

        #region<<links>>

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

        #endregion<<>>

        #region<<batch>>

        private void batch_Click(object sender, RoutedEventArgs e)
        {
            //dataGrid
            foreach (DataRow row in dtpkg2.Rows)
            {
                if (row.ItemArray[15].ToString() == "true")
                {
                    string link_name = row.ItemArray[0].ToString();
                    string file_type = row.ItemArray[14].ToString();
                    string link_type = row.ItemArray[2].ToString();

                    Send_File(link_name, link_type, file_type);
                }
            }
        }

        #endregion<<>>

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

                color2.Background = System.Windows.Media.Brushes.White;
                //page_icon.Fill = System.Windows.Media.Brushes.White;
                abouthead.Fill = System.Windows.Media.Brushes.White;

                
                //System.Windows.Media.ImageSource temp = new BitmapImage(new Uri("pack://application:,,,/PS4_PKG_Linker;component/tools/resources/default.png"));
               // this.Icon = new BitmapImage(new Uri("pack://application:,,,/PS4_PKG_Linker;component/Icon.ico"));

                //button7.Content = "Light Theme";
                //b7tb1.Text = "Light Theme";
                //ctheme = "BaseLight";
            }
            else if (ctheme == "BaseLight")
            {
                color2.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0x25, 0x25, 0x25));
                //page_icon.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0x25, 0x25, 0x25));
                abouthead.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0xFF, 0x25, 0x25, 0x25));

                //this.Icon = new BitmapImage(new Uri("pack://application:,,,/PS4_PKG_Linker;component/tools/resources/default.png"));

                //button7.Content = "Dark Theme";
                // b7tb1.Text = "Dark Theme";
                //ctheme = "BaseDark";
            }
            if (ctheme == "")
            {
                ctheme = "BaseDark";
                color2.Background = System.Windows.Media.Brushes.White;
                page_icon.Fill = System.Windows.Media.Brushes.White;

               // this.Icon = new BitmapImage(new Uri("pack://application:,,,/PS4_PKG_Linker;component/tools/resources/blue.ico"));

                //button7.Content = "Light Theme";
                //b7tb1.Text = "Light Theme";
                //ctheme = "BaseLight";
            }

            var theme = ThemeManager.DetectAppStyle(System.Windows.Application.Current);

            if (color == null || color == "")
            {
                color = "Pink";
            }
            ThemeManager.ChangeAppStyle(System.Windows.Application.Current,
                                        ThemeManager.GetAccent(color),

                                        ThemeManager.GetAppTheme(ctheme));

            //pink1.Foreground = color;

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

        #region<<about>>

        private void Octolus_MouseDoubleClick(object sender, RoutedEventArgs e)
        {

            Process.Start(new ProcessStartInfo("https://twitter.com/OctolusNET"));

        }

        private void ps4database_MouseDoubleClick(object sender, RoutedEventArgs e)
        {

            Process.Start(new ProcessStartInfo("https://ps4database.io/"));

        }

        private void pink1_MouseDoubleClick(object sender, RoutedEventArgs e)
        {

            Process.Start(new ProcessStartInfo("https://twitter.com/Pink1mods"));

        }

        private void flatz_MouseDoubleClick(object sender, RoutedEventArgs e)
        {

            Process.Start(new ProcessStartInfo("https://twitter.com/flat_z"));

        }

        private void rpi_MouseDoubleClick(object sender, RoutedEventArgs e)
        {

            Process.Start(new ProcessStartInfo("https://gist.githubusercontent.com/flatz/60956f2bf1351a563f625357a45cd9c8/raw/d89ab7df9e64446b8d30604d5ce2feb406ea63db/remote_pkg_installer.txt"));

        }

        private void mah_MouseDoubleClick(object sender, RoutedEventArgs e)
        {

            Process.Start(new ProcessStartInfo("https://mahapps.com/"));

        }

        private void wfb_MouseDoubleClick(object sender, RoutedEventArgs e)
        {

            Process.Start(new ProcessStartInfo("https://archive.codeplex.com/?p=wpffolderbrowser"));

        }

        private void jnet_MouseDoubleClick(object sender, RoutedEventArgs e)
        {

            Process.Start(new ProcessStartInfo("https://www.newtonsoft.com/json"));

        }

        private void place_MouseDoubleClick(object sender, RoutedEventArgs e)
        {

            Process.Start(new ProcessStartInfo("http://www.psx-place.com/"));

        }

        private void hax_MouseDoubleClick(object sender, RoutedEventArgs e)
        {

            Process.Start(new ProcessStartInfo("https://playstationhax.xyz/"));

        }

        private void uniform_Click(object sender, RoutedEventArgs e)
        {

            Process.Start(new ProcessStartInfo("http://www.uniformserver.com/"));

        }

        private void CelesteBlue_Click(object sender, RoutedEventArgs e)
        {

            Process.Start(new ProcessStartInfo("https://twitter.com/CelesteBlue123"));

        }

        private void Centrino_Click(object sender, RoutedEventArgs e)
        {

            Process.Start(new ProcessStartInfo("https://twitter.com/Centrinouk"));

        }

        private void sandungas_Click(object sender, RoutedEventArgs e)
        {

            Process.Start(new ProcessStartInfo("http://www.psx-place.com/members/sandungas.872/"));

        }

        private void StarMelter_Click(object sender, RoutedEventArgs e)
        {

            Process.Start(new ProcessStartInfo("https://twitter.com/StarMelter"));

        }

        private void leeful74_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://twitter.com/leeful74"));
        }

        #endregion<<>>



        public void set_colors()
        {
            string colors = "[{\"name\":\"Red\",\"fill\":\"Red\",\"text\":\"Red\",\"bg\":\"Transparent\",\"count\":\"0\"}, {\"name\":\"Green\",\"fill\":\"Green\",\"text\":\"Green\",\"bg\":\"Transparent\",\"count\":\"1\"}, {\"name\":\"Blue\",\"fill\":\"Blue\",\"text\":\"Blue\",\"bg\":\"Transparent\",\"count\":\"2\"}, {\"name\":\"Purple\",\"fill\":\"Purple\",\"text\":\"Purple\",\"bg\":\"Transparent\",\"count\":\"3\"}, {\"name\":\"Orange\",\"fill\":\"Orange\",\"text\":\"Orange\",\"bg\":\"Transparent\",\"count\":\"4\"}, {\"name\":\"Lime\",\"fill\":\"Lime\",\"text\":\"Lime\",\"bg\":\"Transparent\",\"count\":\"5\"}, {\"name\":\"Emerald\",\"fill\":\"#FF077517\",\"text\":\"Emerald\",\"bg\":\"Transparent\",\"count\":\"6\"}, {\"name\":\"Teal\",\"fill\":\"Teal\",\"text\":\"Teal\",\"bg\":\"Transparent\",\"count\":\"7\"}, {\"name\":\"Cyan\",\"fill\":\"Cyan\",\"text\":\"Cyan\",\"bg\":\"Transparent\",\"count\":\"8\"}, {\"name\":\"Cobalt\",\"fill\":\"#FF0747C6\",\"text\":\"Cobalt\",\"bg\":\"Transparent\",\"count\":\"9\"}, {\"name\":\"Indigo\",\"fill\":\"Indigo\",\"text\":\"Indigo\",\"bg\":\"Transparent\",\"count\":\"10\"}, {\"name\":\"Violet\",\"fill\":\"Violet\",\"text\":\"Violet\",\"bg\":\"Transparent\",\"count\":\"11\"}, {\"name\":\"Pink\",\"fill\":\"Pink\",\"text\":\"Pink\",\"bg\":\"Transparent\",\"count\":\"12\"}, {\"name\":\"Magenta\",\"fill\":\"Magenta\",\"text\":\"Magenta\",\"bg\":\"Transparent\",\"count\":\"13\"}, {\"name\":\"Crimson\",\"fill\":\"Crimson\",\"text\":\"Crimson\",\"bg\":\"Transparent\",\"count\":\"14\"}, {\"name\":\"Amber\",\"fill\":\"#FFC7890F\",\"text\":\"Amber\",\"bg\":\"Transparent\",\"count\":\"15\"}, {\"name\":\"Yellow\",\"fill\":\"Yellow\",\"text\":\"Yellow\",\"bg\":\"Transparent\",\"count\":\"16\"}, {\"name\":\"Brown\",\"fill\":\"Brown\",\"text\":\"Brown\",\"bg\":\"Transparent\",\"count\":\"17\"}, {\"name\":\"Olive\",\"fill\":\"Olive\",\"text\":\"Olive\",\"bg\":\"Transparent\",\"count\":\"18\"}, {\"name\":\"Steel\",\"fill\":\"#FF576573\",\"text\":\"Steel\",\"bg\":\"Transparent\",\"count\":\"19\"}, {\"name\":\"Mauve\",\"fill\":\"#FF655475\",\"text\":\"Mauve\",\"bg\":\"Transparent\",\"count\":\"20\"}, {\"name\":\"Taupe\",\"fill\":\"#FF736845\",\"text\":\"Taupe\",\"bg\":\"Transparent\",\"count\":\"21\"}, {\"name\":\"Sienna\",\"fill\":\"Sienna\",\"text\":\"Sienna\",\"bg\":\"Transparent\",\"count\":\"22\"}]";
            COLORS = (DataTable)JsonConvert.DeserializeObject(colors, (typeof(DataTable)));
            Color_Grid.DataContext = null;
            Color_Grid.DataContext = COLORS.DefaultView;
        }

        private void MouseEnter2(object sender, MouseEventArgs e)
        {

            Tile t = sender as Tile;
            string n = t.Title;
            string x = "name = " + '\'' + n + '\'';

            if (e.RoutedEvent.Name == "MouseEnter")
            {
                COLORS.Select(x)[0]["bg"] = t.BorderBrush;
            }
            else
                COLORS.Select(x)[0]["bg"] = System.Windows.Media.Brushes.Transparent;

        }

        private void TILE1_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Tile t = sender as Tile;
            string n = t.Title;
            color = n;
            ChangeAppStyle();
            string x = "name = " + '\'' + n + '\'';
            COLORS.Select(x)[0]["bg"] = t.BorderBrush;
        }


        private void make_shortcut(string file_name)
        {
            
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.WorkingDirectory = appPath;

            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "cmd.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "/c mklink /J PS4 " + file_name + "\\";
            Process exeProcess = Process.Start(startInfo);
            exeProcess.WaitForExit();
        }

        private void button_7_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxtid.Text != "")
            {
                string out1 = Uninstall_game();
            }
        }

        private void button_8_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxtid.Text != "")
            {
                string out1 = Uninstall_patch();
            }
        }

        private void button_9_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxtid.Text != "")
            {
                string out1 = Uninstall_ac();
            }
        }

        private void button_10_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxtid.Text != "")
            {
                string out1 = Uninstall_theme();
            }
        }

        private void button_11_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_12_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_13_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_14_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_15_Click(object sender, RoutedEventArgs e)
        {

        }

        private void textBox1_LostMouseCapture(object sender, MouseEventArgs e)
        {
            
        }

        private void twitter_lable_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://twitter.com/Pink1mods"));
        }

        private void github_lable_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/pink1stools"));
        }

        private void curser_Click(object sender, RoutedEventArgs e)
        {
            Set_cursor();
        }

        private void CursorTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox source = e.Source as ComboBox;

            if (source != null)
            {
                ComboBoxItem selectedCursor = source.SelectedItem as ComboBoxItem;

                // Changing the cursor of the Border control 
                // by setting the Cursor property
                switch (selectedCursor.Content.ToString())
                {
                    case "AppStarting":
                        t.Cursor1 = Cursors.AppStarting;
                        break;
                    case "ArrowCD":
                        t.Cursor1 = Cursors.ArrowCD;
                        break;
                    case "Arrow":
                        t.Cursor1 = Cursors.Arrow;
                        break;
                    case "Cross":
                        t.Cursor1 = Cursors.Cross;
                        break;
                    case "HandCursor":
                        t.Cursor1 = Cursors.Hand;
                        break;
                    case "Help":
                        t.Cursor1 = Cursors.Help;
                        break;
                    case "IBeam":
                        t.Cursor1 = Cursors.IBeam;
                        break;
                    case "No":
                        t.Cursor1 = Cursors.No;
                        break;
                    case "None":
                        t.Cursor1 = Cursors.None;
                        break;
                    case "Pen":
                        t.Cursor1 = Cursors.Pen;
                        break;
                    case "ScrollSE":
                        t.Cursor1 = Cursors.ScrollSE;
                        break;
                    case "ScrollWE":
                        t.Cursor1 = Cursors.ScrollWE;
                        break;
                    case "SizeAll":
                        t.Cursor1 = Cursors.SizeAll;
                        break;
                    case "SizeNESW":
                        t.Cursor1 = Cursors.SizeNESW;
                        break;
                    case "SizeNS":
                        t.Cursor1 = Cursors.SizeNS;
                        break;
                    case "SizeNWSE":
                        t.Cursor1 = Cursors.SizeNWSE;
                        break;
                    case "SizeWE":
                        t.Cursor1 = Cursors.SizeWE;
                        break;
                    case "UpArrow":
                        t.Cursor1 = Cursors.UpArrow;
                        break;
                    case "WaitCursor":
                        t.Cursor1 = Cursors.Wait;
                        break;
                    case "Middle Finger":
                        t.Cursor1 = Cursor2;
                        break;
                    default:
                        break;
                }

                
            }
        }

        private void CursorSelector2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox source = e.Source as ComboBox;

            if (source != null)
            {
                ComboBoxItem selectedCursor = source.SelectedItem as ComboBoxItem;

                // Changing the cursor of the Border control 
                // by setting the Cursor property
                switch (selectedCursor.Content.ToString())
                {
                    case "AppStarting":
                        t.Cursor1 = Cursors.AppStarting;
                        break;
                    case "ArrowCD":
                        t.Cursor1 = Cursors.ArrowCD;
                        break;
                    case "Arrow":
                        t.Cursor1 = Cursors.Arrow;
                        break;
                    case "Cross":
                        t.Cursor1 = Cursors.Cross;
                        break;
                    case "HandCursor":
                        t.Cursor1 = Cursors.Hand;
                        break;
                    case "Help":
                        t.Cursor1 = Cursors.Help;
                        break;
                    case "IBeam":
                        t.Cursor1 = Cursors.IBeam;
                        break;
                    case "No":
                        t.Cursor1 = Cursors.No;
                        break;
                    case "None":
                        t.Cursor1 = Cursors.None;
                        break;
                    case "Pen":
                        t.Cursor1 = Cursors.Pen;
                        break;
                    case "ScrollSE":
                        t.Cursor1 = Cursors.ScrollSE;
                        break;
                    case "ScrollWE":
                        t.Cursor1 = Cursors.ScrollWE;
                        break;
                    case "SizeAll":
                        t.Cursor1 = Cursors.SizeAll;
                        break;
                    case "SizeNESW":
                        t.Cursor1 = Cursors.SizeNESW;
                        break;
                    case "SizeNS":
                        t.Cursor1 = Cursors.SizeNS;
                        break;
                    case "SizeNWSE":
                        t.Cursor1 = Cursors.SizeNWSE;
                        break;
                    case "SizeWE":
                        t.Cursor1 = Cursors.SizeWE;
                        break;
                    case "UpArrow":
                        t.Cursor1 = Cursors.UpArrow;
                        break;
                    case "WaitCursor":
                        t.Cursor1 = Cursors.Wait;
                        break;
                    case "Middle Finger":
                        t.Cursor1 = Cursor2;
                        break;
                    default:
                        break;
                }


            }
        }

        private void CursorSelector2_Click(object sender, RoutedEventArgs e)
        {
            DropDownButton Q = sender as DropDownButton;
            ClickableLabel source = e.Source as ClickableLabel;

            if (source != null)
            {

                CursorSelector2.IsExpanded = false;
                // Changing the cursor of the Border control 
                // by setting the Cursor property
                switch (source.Content.ToString())
                {
                    case "AppStarting":
                        t.Cursor1 = Cursors.AppStarting;
                        break;
                    case "ArrowCD":
                        t.Cursor1 = Cursors.ArrowCD;
                        break;
                    case "Arrow":
                        t.Cursor1 = Cursors.Arrow;
                        break;
                    case "Cross":
                        t.Cursor1 = Cursors.Cross;
                        break;
                    case "HandCursor":
                        t.Cursor1 = Cursors.Hand;
                        break;
                    case "Help":
                        t.Cursor1 = Cursors.Help;
                        break;
                    case "IBeam":
                        t.Cursor1 = Cursors.IBeam;
                        break;
                    case "No":
                        t.Cursor1 = Cursors.No;
                        break;
                    case "None":
                        t.Cursor1 = Cursors.None;
                        break;
                    case "Pen":
                        t.Cursor1 = Cursors.Pen;
                        break;
                    case "ScrollSE":
                        t.Cursor1 = Cursors.ScrollSE;
                        break;
                    case "ScrollWE":
                        t.Cursor1 = Cursors.ScrollWE;
                        break;
                    case "SizeAll":
                        t.Cursor1 = Cursors.SizeAll;
                        break;
                    case "SizeNESW":
                        t.Cursor1 = Cursors.SizeNESW;
                        break;
                    case "SizeNS":
                        t.Cursor1 = Cursors.SizeNS;
                        break;
                    case "SizeNWSE":
                        t.Cursor1 = Cursors.SizeNWSE;
                        break;
                    case "SizeWE":
                        t.Cursor1 = Cursors.SizeWE;
                        break;
                    case "UpArrow":
                        t.Cursor1 = Cursors.UpArrow;
                        break;
                    case "WaitCursor":
                        t.Cursor1 = Cursors.Wait;
                        break;
                    case "Middle Finger":
                        t.Cursor1 = Cursor2;
                        break;
                    default:
                        break;
                }


            }
        }

        private void textBoxtid_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(textBoxtid.Text.Length == 12)
            {
                LB1.Visibility = Visibility.Visible;
            }
            else if (textBoxcid.Text.Length == 36)
            {
                LB1.Visibility = Visibility.Visible;
            }
            else
            {
                LB1.Visibility = Visibility.Hidden;
            }
        }

        private void textBoxcid_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxcid.Text.Length == 36)
            {
                LB1.Visibility = Visibility.Visible;
            }
            else if (textBoxtid.Text.Length == 12)
            {
                LB1.Visibility = Visibility.Visible;
            }
            else
            {
                LB1.Visibility = Visibility.Hidden;
            }
        }

        private void Uniserver_Checked(object sender, RoutedEventArgs e)
        {
            b6.Visibility = Visibility.Visible;
            b8.Visibility = Visibility.Visible;
        }

        private void Nodejs_serve_Checked(object sender, RoutedEventArgs e)
        {
            b6.Visibility = Visibility.Hidden;
            b8.Visibility = Visibility.Hidden;
        }

        private void MetroWindow_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

        private void update_lable_Click(object sender, RoutedEventArgs e)
        {
            AutoUpdater.LetUserSelectRemindLater = true;
            //AutoUpdater.RemindLaterTimeSpan = RemindLaterFormat.Minutes;
            //AutoUpdater.RemindLaterAt = 1;
            AutoUpdater.OpenDownloadPage = true;
            AutoUpdater.ReportErrors = true;
            AutoUpdater.DownloadPath = Environment.CurrentDirectory;

            AutoUpdater.Start("https://raw.githubusercontent.com/pink1stools/PS4_PKG_Linker/master/Updater.xml");
            
        }

        private void AutoUpdater_ApplicationExitEvent()
        {
            
        }


        // AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;

        /*private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args != null)
            {
                if (args.IsUpdateAvailable)
                {

                    DialogResult dialogResult;
                    if (args.Mandatory)
                    {
                        dialogResult =

                            MessageBox.Show(
                                $@"There is new version {args.CurrentVersion} available. You are using version {args.InstalledVersion}. This is required update. Press Ok to begin updating the application.", @"Update Available",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                    }
                    else
                    {
                        dialogResult =
                            MessageBox.Show(
                                $@"There is new version {args.CurrentVersion} available. You are using version {
                                        args.InstalledVersion
                                    }. Do you want to update the application now?", @"Update Available",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);
                    }

                    // Uncomment the following line if you want to show standard update dialog instead.
                    // AutoUpdater.ShowUpdateForm();

                    if (dialogResult.Equals(DialogResult.Yes))
                    {
                        try
                        {
                            if (AutoUpdater.DownloadUpdate())
                            {
                                Application.Exit();
                            }
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(@"There is no update available please try again later.", @"No update available",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(
                        @"There is a problem reaching update server please check your internet connection and try again later.",
                        @"Update check failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/





    }

     
    class TestObject : INotifyPropertyChanged
    {
        private Cursor _cursor;
        public Cursor Cursor1
        {
            get
            {
                return _cursor;
            }

            set
            {
                if (_cursor == value) return;

                _cursor = value;
                OnPropertyChanged("Cursor1");
            }
        }
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }



}
