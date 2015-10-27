using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
        private Detector _detector = new Detector();

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
            BitmapImage bmp = new BitmapImage();
            ShowImage(_detector.GetScreenShot());
        }

        private void ShowImage(Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Bmp);
            byte[] buffer = ms.GetBuffer();
            MemoryStream bufferParser = new MemoryStream(buffer);
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = bufferParser;
            bitmap.EndInit();
            imgPreview.Source = bitmap;
        }

        private void butPreviewGetColour_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.SolidColorBrush _bg = new System.Windows.Media.SolidColorBrush();
            _bg.Color = _detector.GetAverageColour(1, 0, 0, 50, 0);
            labPreviewBox1.Background = _bg;
        }
    }
}
