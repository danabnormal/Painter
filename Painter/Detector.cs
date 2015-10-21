using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace Painter
{
    /// <summary>
    /// The Detector object provides functions for obtaining and analyzing imagery to be used for the purpose of controlling a hue system.
    /// </summary>
    class Detector
    {
        /// <summary>
        /// A Region defines a line with which to begin obtaining sample data for a given image.
        /// </summary>
        public class Region
        {
            int _start;
            int _end;
            int _id;
            string _name;
            Axis _axis;

            /// <summary>
            /// Dictates whether the coordinates given apply to the X or Y axis.
            /// </summary>
            public Axis Axis
            {
                get
                {
                    return _axis;
                }
                set
                {
                    _axis = value;
                }
            }

            /// <summary>
            /// An Integer value representation of a whole number percentage (10 = 10%, 100 = 100%) detailing at what point the analysis line begins.
            /// </summary>
            public int Start
            {
                get {
                    return _start;
                }

                set
                {
                    _start = value;
                }
            }

            /// <summary>
            /// An Integer value representation of a whole number percentage (10 = 10%, 100 = 100%) detailing at what point the analysis line ends.
            /// </summary>
            public int End
            {
                get
                {
                    return _end;
                }

                set
                {
                    _end = value;
                }
            }

            /// <summary>
            /// An integer that represents the unioque ID of the Region.
            /// </summary>
            public int ID
            {
                get
                {
                    return _id;
                }

                set
                {
                    _id = value;
                }
            }

            /// <summary>
            /// A 'friendly', 'human' value describing the Region.
            /// </summary>
            public string Name
            {
                get
                {
                    return _name;
                }

                set
                {
                    _name = value;
                }
            }

        }

        /// <summary>
        /// Represents the axis a detection line should apply to
        /// </summary>
        public enum Axis
        {
            x=0,
            y=1
        }

        /// <summary>
        /// A collection of Region objects that define the areas that need to be analyzed.
        /// </summary>
        public List<Region> Regions { get; set; }

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
        public Color GetAverageColour(Image img, int accuracy, int startx, int starty, int endx, int endy)
        {
            using (var bmp = new Bitmap(img))
            {
                int width = bmp.Width;
                int height = bmp.Height;
                int red = 0;
                int green = 0;
                int blue = 0;
                int alpha = 0;
                int xcounter = 0;
                var ycounter = 0;
                for (int x = startx; x < endx; x = x + accuracy)
                {
                    ycounter = 0;
                    xcounter++;
                    for (int y = starty; y < endy; y = y + accuracy)
                    {
                        ycounter++;
                        var pixel = bmp.GetPixel(x, y);
                        red += pixel.R;
                        green += pixel.G;
                        blue += pixel.B;
                        alpha += pixel.A;
                    }
                }
                //Func<int, int> avg = c => c / (((endx - startx)/xcounter) * ((endy - starty)/ycounter));
                Func<int, int> avg = c => c / (xcounter * ycounter);

                red = avg(red);
                green = avg(green);
                blue = avg(blue);
                alpha = avg(alpha);

                var color = Color.FromArgb(alpha, red, green, blue);
                bmp.Dispose();

                return color;
            }
        }

        /// <summary>
        /// Obtains a screenshot of the primary window.
        /// </summary>
        /// <returns>An Image object containing a screenshot of the primary screen.</returns>
        public Image GetScreenShot()
        {
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            
            return bmpScreenshot;
        }
    }
}
