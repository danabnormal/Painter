using System;
using System.Windows;
using System.Windows.Controls;

namespace Painter
{
    /// <summary>
    /// CVlass that deas with any logging needed by trhe pp, whether to file or form.
    /// </summary>
    class Logging
    {
        /// <summary>
        /// Writes a standard log entry as configured by the application.
        /// </summary>
        /// <param name="entry"></param>
        public void Write(string entry)
        {
            //TODO: This should be read from Application settings (including Form location).
            TextBox control = new TextBox();
            foreach (Window w in Application.Current.Windows){
                if (w.GetType() == typeof(MainWindow))
                {
                    control = (w as MainWindow).txtLog;
                }
            }
            control.Text += DateTime.Now + ":" + entry + System.Environment.NewLine;

            //TODO: Scrub that lot and write to Debugh window instead
        }
    }
}
