using System;
using System.Drawing;
using System.Windows.Forms;

namespace Painter
{
    
    /// <summary>
    /// The Detector object provides functions for obtaining and analyzing imagery to be used for the purpose of controlling a hue system.
    /// </summary>
    class Detector
    {
        private Tools.Log _log = new Tools.Log();
        private Image _image;
        
        /// <summary>
        /// Returns a color object that represents the average colour of an image based on the settings provided.
        /// </summary>
        /// <param name="img">The image object to analyse</param>
        /// <param name="accuracy">Accuracy is determined by how many x pixels are evaluated - the lower the number, the higher the accuracy.</param>
        /// <param name="startx">X coordinate to start from.</param>
        /// <param name="starty">Y coordinate to start from.</param>
        /// <param name="endx">X coordinate to end at.</param>
        /// <param name="endy">Y coordinate to end at.</param>
        /// <returns>A color object that represents the average colour of an image based on the settings provided.</returns>
        public System.Windows.Media.Color GetAverageColour(int accuracy, int startx, int starty, int endx, int endy)
        {
            using (var bmp = new Bitmap(_image))
            {
                
                int width = _image.Width;
                int height = _image.Height;

                int xpc = width / 100;
                int ypc = height / 100;

                int red = 0;
                int green = 0;
                int blue = 0;
                int alpha = 0;
                int xcounter = 0;
                var ycounter = 0;

                for (int x = (startx * xpc); x < (endx * xpc); x = x + accuracy)
                {
                    ycounter=0;
                    xcounter++;
                    for (int y = (starty * ypc); y < ((endy * ypc)+1); y = y + accuracy)
                    {
                        _log.Write("x=" + x + ", y=" + y);
                        ycounter++;
                        var pixel = bmp.GetPixel(x, y);
                        red += pixel.R;
                        green += pixel.G;
                        blue += pixel.B;
                        alpha += pixel.A;
                    }
                }
                Func<int, int> avg = c => c / (xcounter * ycounter);

                red = avg(red);
                green = avg(green);
                blue = avg(blue);
                alpha = avg(alpha);
                
                System.Windows.Media.Color color = new System.Windows.Media.Color();
                color.A = Convert.ToByte(255);
                color.R = Convert.ToByte(red);
                color.G = Convert.ToByte(green);
                color.B = Convert.ToByte(blue);
                bmp.Dispose();

                _log.Write("Detector, color: " + color.ToString());
                return color;
            }
        }

        /// <summary>
        /// Obtains a screenshot of the primary window.
        /// </summary>
        /// <returns>An Image object containing a screenshot of the primary screen.</returns>
        public Image GetScreenShot()
        {
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            _image = bmpScreenshot;
            return _image;
        }

        /// <summary>
        /// Contains a copy of the most recent screenshot taken.
        /// </summary>
        public Image Image
        {
            get
            {
                return _image;
            }

            set
            {
                _image = value;
            }
        }      
    }
}
