using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;

namespace Painter
{
    /// <summary>
    /// A collection of tools and sundry 'misc' helping classes including logging.
    /// </summary>
    public class Tools
    {
        /// <summary>
        /// Allows logging of various types by the application.
        /// </summary>
        public class Log {
            /// <summary>
            /// Commits a configured log entry 
            /// </summary>
            /// <param name="LogMessage">A string text message to be written to the log.</param>
            public void Write(string LogMessage)
            {
                // Insert bools to enable/disable logging features here. 
                bool _writetotextbox = false;

                if (_writetotextbox == true)
                {
                    MainWindow _mw = (MainWindow)Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                    _mw.txtLog.Text = _mw.txtLog.Text + DateTime.Now + " " + LogMessage + Environment.NewLine;
                }
                
            }
        }

        /// <summary>
        /// Exports an object to file for reference later
        /// </summary>
        /// <param name="config">An object to be exported.</param>
        /// <param name="ExportLocation">Location (inc. filename) for the export to be saved.</param>
        public void ExportConfig(object config, string ExportLocation)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(ExportLocation, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, config);
            stream.Close();
        }

        /// <summary>
        /// Loads a given file, casting it to the given object type. Effectively, this loads objkects that have previously been exported using ExportConfig.
        /// </summary>
        /// <typeparam name="T">An Object Type to be returned.</typeparam>
        /// <param name="filePath">Full path to the file to load.</param>
        /// <returns>An opbject of type T loaded from the given filename.</returns>
        public T ImportConfig<T>(string filePath)
        {

            try
            {
                using (Stream stream = File.Open(filePath, FileMode.Open))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    return (T)binaryFormatter.Deserialize(stream);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Creates a default hue file that can either be used for demo purposes or to replace a missing Default.hue (one should always be present).
        /// </summary>
        /// <param name="FileName">The name and path of the file to write to. If ommitted this will default to %APPDATA%\Default.hue .</param>
        public void CreateDefaultHueFile(string FileName = "c:\test.hue")
        {
            Tools tools = new Tools();
            tools.ExportConfig(CreateDefaultObject(), FileName);

        }

        private IOConfig.InputConfig CreateDefaultObject()
        {
            IOConfig.InputConfig cfg = new IOConfig.InputConfig();
            cfg.Settings = DefaultSettings();
            cfg.Inputs = DefaultRegionList();
            cfg.Name = "Default Configuration";
            return cfg;
        }

        /// <summary>
        /// Creates a Settings object that mimics the configuration set by the user through the UI.
        /// </summary>
        /// <returns></returns>
        private DetectionSettings DefaultSettings()
        {
            DetectionSettings settings = new DetectionSettings();
            settings.ConserveMemory = false;
            settings.ShowPreview = true;
            settings.DisableFormDrawing = false;

            settings.SampleWidth = 1;
            settings.SampleAccuracy = 1;
            settings.SampleInterval = 1;

            return settings;

        }

        /// <summary>
        /// A generated list of Regions for use during detection.
        /// </summary>
        /// <returns></returns>
        private List<Input> DefaultRegionList()
        {
            List<Input> _regions = new List<Input>();
            _regions.Add(SampleInput(0, 25, 75, 50, 50));
            _regions.Add(SampleInput(1, 0, 50, 0, 0));
            _regions.Add(SampleInput(2, 51, 100, 0, 0));
            _regions.Add(SampleInput(3, 0, 50, 100, 100));
            _regions.Add(SampleInput(4, 51, 100, 100, 100));
            _regions.Add(SampleInput(5, 0, 0, 0, 50));
            _regions.Add(SampleInput(6, 0, 0, 51, 100));
            _regions.Add(SampleInput(7, 100, 100, 0, 50));
            _regions.Add(SampleInput(8, 100, 100, 51, 100));
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
        
    }
}
