using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        private void butPreviewGrabScreen_Click(object sender, RoutedEventArgs e)
        {
            //TODO THIS BLOODY BIT. What is an ImageSource and how do I use it!

            HueSystem _hue = new HueSystem();

            _hue.HubIPAddress = "1.1.1.1";
            _hue.HubUserName = "Me";

            HueSystem.Light _light = new HueSystem.Light();
            _light.Number = 10;
            _light.Name = "TV";
            _hue.Lights.Add(_light);
            if (_hue.Lights[0].IsOn==true)
            {
                _hue.SendCommand(HueSystem.Commands.SendColor, _hue.Lights[0], 255);
            }

            Detector detector = new Detector();

            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            //bmp. = detector.GetScreenShot();




            ImageSourceConverter converter = new ImageSourceConverter();
            
            ImageSource imagesource = (ImageSource)(converter.ConvertTo(detector.GetScreenShot(), typeof(ImageSource)));
            imgPreview.Source = imagesource;
            
        }
    }
}
