using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro;

namespace PS4_PKG_Linker
{
    /// <summary>
    /// Interaction logic for Window_MahApps.xaml
    /// </summary>
    public partial class Window_server : MetroWindow
    {
        string newsite;
        string ctheme;
        string color;
        public Window_server(string site, string cl, string ct)
        {
            newsite = site;
            ctheme = ct;
            color = cl;
            InitializeComponent();


        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // ... Load this site.
            ChangeAppStyle();
            this.browser1.Navigate(newsite);
        }


        public void ChangeAppStyle()
        {

            var theme = ThemeManager.DetectAppStyle(Application.Current);


            ThemeManager.ChangeAppStyle(Application.Current,
                                        ThemeManager.GetAccent(color),

                                        ThemeManager.GetAppTheme(ctheme));


        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.browser1.Dispose();
        }

    }
}