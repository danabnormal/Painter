using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Painter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Detector _detector = new Detector();
        private IOConfig.InputConfig _currentioconfig = new IOConfig.InputConfig();
        
        public MainWindow()
        {
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru-RU");
            InitializeComponent();
            
            LoadDefaultSettings();
        }
        
        private void LoadDefaultSettings()
        {
            Tools tools = new Tools();

            try
            {
                _currentioconfig = tools.ImportConfig<IOConfig.InputConfig>(AppDomain.CurrentDomain.BaseDirectory + "Plugins\\IO\\Default.hue");
                ReadSettingsToControls(_currentioconfig.Settings);
            }
            catch (Exception ex) when (ex is FileNotFoundException)
            {
                tools.CreateDefaultHueFile();
                _currentioconfig = tools.ImportConfig<IOConfig.InputConfig>(AppDomain.CurrentDomain.BaseDirectory + "Plugins\\IO\\Default.hue");
                ReadSettingsToControls(_currentioconfig.Settings);
                return;
            }

            catch (Exception)
            {
                throw;
            }
            
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
            ShowImage(_detector.GetScreenShot());
        }

        /// <summary>
        /// Sets a given Image object as the source for an Image control within the UI.
        /// </summary>
        /// <param name="image">File to render.</param>
        private void ShowImage(System.Drawing.Image image)
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
            ReadColour();
        }

        /// <summary>
        /// Initiates functionality for reading a color value and outputting that cvalue to form controls.
        /// </summary>
        private void ReadColour()
        {
            foreach (Input _input in _currentioconfig.Inputs)
            {
                System.Windows.Media.SolidColorBrush _bg = new System.Windows.Media.SolidColorBrush();
                Label _label = (Label)this.FindName("labPreviewBox" + _input.ID);
                _bg.Color = _detector.GetAverageColour(_currentioconfig.Settings.SampleAccuracy, _input.StartX, _input.StartY, _input.EndX, _input.EndY);
                _label.Background = _bg;
            }
        }

        private void butSaveConfig_Click(object sender, RoutedEventArgs e)
        {
            SaveConfig();
        }

        /// <summary>
        /// Exports a populated InputConfig object to file.
        /// </summary>
        private void SaveConfig()
        {
            
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = Properties.Resources.MAIN_FILETYPE_DESCRIPTION + "|*.hue";

            if (dlg.ShowDialog() == true)
            {
                Tools tools = new Tools();
                tools.ExportConfig(CreateConfigObject(), dlg.FileName);
            }
        }

        /// <summary>
        /// Opens a given file, creating an InputConfig object with it and rendering the UI to reflect its settings.
        /// </summary>
        private void LoadConfig()
        {
            IOConfig.InputConfig _cfg = new IOConfig.InputConfig();
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = Properties.Resources.MAIN_FILETYPE_DESCRIPTION + "|*.hue";
            if (dlg.ShowDialog() == true)
            {
                Tools tools = new Tools();
                _currentioconfig = tools.ImportConfig<IOConfig.InputConfig>(dlg.FileName);
                ReadSettingsToControls(_currentioconfig.Settings);
            }
        }
        
        /// <summary>
        /// Creates a new InputConfig object and populates it for use
        /// </summary>
        /// <returns>A populated InputConfig object that reflects the settings configured in the UI by the user.</returns>
        private IOConfig.InputConfig CreateConfigObject()
        {
            IOConfig.InputConfig cfg = new IOConfig.InputConfig();
            cfg.Settings = ReadControlsToSettings();
            cfg.Inputs = SampleRegionList();
            cfg.Name = "Quick and not accurate";
            return cfg;
        }

        /// <summary>
        /// Creates a Settings object that mimics the configuration set by the user through the UI.
        /// </summary>
        /// <returns></returns>
        private DetectionSettings ReadControlsToSettings()
        {
            DetectionSettings settings = new DetectionSettings();
            if (chkSamplingConserveMemory.IsChecked == false) { settings.ConserveMemory = false; } else { settings.ConserveMemory = true; };
            if (chkPreviewShowLive.IsChecked == false) { settings.ShowPreview = false; } else { settings.ShowPreview = true; };
            if (chkPreviewDisableDrawing.IsChecked == false) { settings.DisableFormDrawing = false; settings.ShowPreview = false; } else { settings.DisableFormDrawing = true; settings.ShowPreview = true; };

            settings.SampleWidth = (int)Math.Ceiling(slidSamplingSampleWidth.Value);
            settings.SampleAccuracy = (int)Math.Ceiling(slidSamplingSampleAccuracy.Value);
            settings.SampleInterval = (int)Math.Ceiling(slidSamplingSampleInterval.Value);

            return settings;

        }

        /// <summary>
        /// Writes out settings propvided ina given DetectionSettings object to the UI.
        /// </summary>
        /// <param name="settings"></param>
        private void ReadSettingsToControls(DetectionSettings settings)
        {
            chkSamplingConserveMemory.IsChecked = settings.ConserveMemory;
            chkPreviewShowLive.IsChecked = settings.ShowPreview;

            if (settings.DisableFormDrawing == false)
           {
                chkPreviewDisableDrawing.IsChecked = false;
                chkPreviewShowLive.IsEnabled = true;
                
            } else
            {
                chkPreviewDisableDrawing.IsChecked = true;
                chkPreviewShowLive.IsEnabled = false;
                chkPreviewShowLive.IsChecked = false;
            };

            slidSamplingSampleWidth.Value = settings.SampleWidth;
            slidSamplingSampleAccuracy.Value= settings.SampleAccuracy;
            slidSamplingSampleInterval.Value = settings.SampleInterval;

        }

        /// <summary>
        /// A generated list of Regions for use during detection.
        /// </summary>
        /// <returns></returns>
        private List<Input> SampleRegionList()
        {
            List<Input> _regions = new List<Input>();
            _regions.Add(SampleInput(   0,  25,  75,  50,  50));
            _regions.Add(SampleInput(   1,   0,  50,   0,   0));
            _regions.Add(SampleInput(   2,  51, 100,   0,   0));
            _regions.Add(SampleInput(   3,   0,  50, 100, 100));
            _regions.Add(SampleInput(   4,  51, 100, 100, 100));
            _regions.Add(SampleInput(   5,   0,   0,   0,  50));
            _regions.Add(SampleInput(   6,   0,   0,  51, 100));
            _regions.Add(SampleInput(   7, 100, 100,   0,  50));
            _regions.Add(SampleInput(   8, 100, 100,  51, 100));
            return _regions;
        }

        /// <summary>
        /// Creates a sample Input region from hard coded values. Should only be used for testing. 
        /// </summary>
        /// <returns></returns>
        private Input SampleInput(int ID, int startX, int endX, int startY, int endY)
        {
            Input region = new Input();
            region.StartX = startX;
            region.EndX = endX;
            region.StartY = startY;
            region.EndY = endY;
            region.Description = "Sample input";
            region.ID = ID;
            return region;
        }

        private void butLoadConfig_Click(object sender, RoutedEventArgs e)
        {
            LoadConfig();
        }

        private void butCreateDefultHue_Click(object sender, RoutedEventArgs e)
        {
            Tools tools = new Tools();
            tools.CreateDefaultHueFile();
        }
    }
}
