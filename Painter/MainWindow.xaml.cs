using System;
using System.Windows;

namespace Painter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru-RU");
            InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void chkPreviewDisableDrawing_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkPreviewDisableDrawing_Click(object sender, RoutedEventArgs e)
        {
            if (chkPreviewDisableDrawing.IsChecked==true)
            {
                chkPreviewShowLive.IsEnabled = false;
                chkPreviewShowLive.IsChecked = false;
            } else
            {
                chkPreviewShowLive.IsEnabled = true;
            }
        }
    }
}
