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
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    return (T)binaryFormatter.Deserialize(stream);
            }
        }
        
    }
}
