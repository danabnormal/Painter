﻿using Microsoft.Win32;
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
        private IOConfig.InputConfig _currentioconfig = new IOConfig.InputConfig();

        public MainWindow()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru-RU");
            InitializeComponent();
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
            ReadColour();
        }

        /// <summary>
        /// Initiates functionality for reading a color value and outputting that cvalue to form controls.
        /// </summary>
        private void ReadColour()
        {
            System.Windows.Media.SolidColorBrush _bg = new System.Windows.Media.SolidColorBrush();
            System.Windows.Media.SolidColorBrush _bg2 = new System.Windows.Media.SolidColorBrush();

            Input _input = _currentioconfig.Inputs.Find(x => x.ID == 0);
            _bg.Color = _detector.GetAverageColour(_currentioconfig.Settings.SampleAccuracy, _input.StartX, _input.StartY, _input.EndX, _input.EndY);
            labPreviewBox1.Background = _bg;

            Input _input1 = _currentioconfig.Inputs.Find(x => x.ID == 1);
            _bg2.Color = _detector.GetAverageColour(_currentioconfig.Settings.SampleAccuracy, _input1.StartX, _input1.StartY, _input1.EndX, _input1.EndY);
            labPreviewBox2.Background = _bg2;
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
            dlg.Filter = "Hue File|*.hue";
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
            dlg.Filter = "Hue File|*.hue";
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
            _regions.Add(SampleInput(0,0,50,0,0));
            _regions.Add(SampleInput(1, 51, 100, 0, 0));
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
    }
}
