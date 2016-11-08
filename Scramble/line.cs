using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Scramble
{
    class line
    {
        #region Global Variables
        int ns = 0, ew = 0, nesw = 0, nwse = 0;
        Point pixel;
        #endregion
        public line(Bitmap pic, int x, int y)
        {
            pixel = new Point(x, y);
            findNS(pic);
            findEW(pic);
            findNESW(pic);
            findNWSE(pic);
        }
        private void findNWSE(Bitmap pic)
        {
            int up = 0, down = 0;
            while (pic.GetPixel(pixel.X, pixel.Y - up - 1).R < 100 || pic.GetPixel(pixel.X, pixel.Y + down + 1).R < 100)
            {
                break;
            }
            if (up + down + 1 > 6)
            {
                nwse = 1;
            }
        }
        private void findNESW(Bitmap pic)
        {
            int up = 0, down = 0;
            while (pic.GetPixel(pixel.X, pixel.Y - up - 1).R < 100 || pic.GetPixel(pixel.X, pixel.Y + down + 1).R < 100)
            {
                break;
            }
            if (up + down + 1 > 6)
            {
                nesw = 1;
            }
        }
        private void findNS(Bitmap pic)
        {
            int up = 0, down = 0;
            while (pic.GetPixel(pixel.X, pixel.Y - up - 1).R < 100 || pic.GetPixel(pixel.X, pixel.Y + down + 1).R < 100)
            {
                break;
            }
            if (up + down + 1 > 6)
            {
                ns = 1;
            }
        }
        private void findEW(Bitmap pic)
        {
            int up = 0, down = 0;
            while (pic.GetPixel(pixel.X, pixel.Y - up - 1).R < 100 || pic.GetPixel(pixel.X, pixel.Y + down + 1).R < 100)
            {
                break;
            }
            if (up + down + 1 > 6)
            {
                ew = 1;
            }
        }
        public int NorthSouth
        {
            get { return ns; }
        }
        public int EastWest
        {
            get { return ew; }
        }
        public int NorthwestSoutheast
        {
            get { return nwse; }
        }
        public int NortheastSouthwest
        {
            get { return nesw; }
        }
    }
}
