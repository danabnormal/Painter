using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
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

            _bg.Color = _detector.GetAverageColour(1, 51, 0, 100, 0);
            labPreviewBox2.Background = _bg;
        }

        private void butSaveConfig_Click(object sender, RoutedEventArgs e)
        {
            IOConfig.InputConfig cfg = new IOConfig.InputConfig();

            DetectionSettings settings = new DetectionSettings();
            if (chkSamplingConserveMemory.IsChecked == false) { settings.ConserveMemory = false; } else { settings.ConserveMemory = true; };
            if (chkPreviewShowLive.IsChecked == false) { settings.ShowPreview = false; } else { settings.ShowPreview = true; };
            if (chkPreviewDisableDrawing.IsChecked == false) { settings.DisableFormDrawing = false; settings.ShowPreview = false; } else { settings.DisableFormDrawing = true; settings.ShowPreview = true; };

            settings.SampleWidth = (int)Math.Ceiling(slidSamplingSampleWidth.Value);
            settings.SampleAccuracy = (int)Math.Ceiling(slidSamplingSampleAccuracy.Value);
            settings.SampleInterval = (int)Math.Ceiling(slidSamplingSampleInterval.Value);

            cfg.Settings = settings;

            List <Input> _regions = new List<Input>();
            Input region = new Input();
            region.StartX = 0;
            region.EndX = 50;
            region.StartY = 0;
            region.EndY = 0;
            region.Description = "Top Left";
            region.ID = 0;
            _regions.Add(region);

            cfg.Inputs = _regions; 
            cfg.Name = "Quick and not accurate";

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Hue File|*.hue";
            if (dlg.ShowDialog() == true)
            {
                Tools tools = new Tools();
                tools.ExportConfig(cfg, dlg.FileName);
            }

        }
    }
}
