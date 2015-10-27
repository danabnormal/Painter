using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Painter
{
    /// <summary>
    /// A collection of tools and sundry 'misc' helping classes including logging.
    /// </summary>
    class Tools
    {
        /// <summary>
        /// Allows logging of various types by the application.
        /// </summary>
        class Log {
            // TODO implement proper logic for finding the log control. 
            // private TextBox _logcontrol = Form.ActiveForm.Controls.Find("txtLog", true).GetValue(0);

            private TextBox _logcontrol = new TextBox();

            /// <summary>
            /// Commits a configured log entry 
            /// </summary>
            /// <param name="LogMessage">A string text message to be written to the log.</param>
            void Write(string LogMessage)
            {
                _logcontrol.Text = DateTime.Now + " " + LogMessage;
            }
        }
    }
}
