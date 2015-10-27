using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

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
            // TODO implement proper logic for finding the log control. 
            // private TextBox _logcontrol = Form.ActiveForm.Controls.Find("txtLog", true).GetValue(0);

            private TextBox _logcontrol = new TextBox();

            /// <summary>
            /// Commits a configured log entry 
            /// </summary>
            /// <param name="LogMessage">A string text message to be written to the log.</param>
            public void Write(string LogMessage)
            {
                _logcontrol.Text = DateTime.Now + " " + LogMessage;
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
        
    }
}
