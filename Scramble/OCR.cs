using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Scramble
{
    class OCR
    {
        #region Global Variables
        string _st;
        char _ch;
        #endregion
        public OCR(Bitmap pic)
        {
            _ch = getChar(pic)[0];
            _st = getChar(pic);
        }
        public string String
        {
            get { return _st; }
        }
        public char Char
        {
            get { return _ch; }
        }
        private string getChar(Bitmap pic)
        {
            string st = "?";
            bool lines = true;
            //check how many horiz lines
            int horiz = 0;
            //vert lines
            int vert = 0;
            //ne sw lines
            int NeSw = 0;
            //nw se lines
            int NwSe = 0;
            if (lines)
            {
                horiz = CheckHoriz(pic);
                //vert lines
                vert = CheckVert(pic);
                //ne sw lines
                NeSw = CheckNESW(pic);
                //nw se lines
                NwSe = CheckNWSE(pic);
            }
            else
            {
                for (int r = 0; r < pic.Height; r++)
                {
                    for (int c = 0; c < pic.Width; c++)
                    {
                        if (pic.GetPixel(c, r).R < 100)
                        {
                            line l = new line(pic, c, r);
                            horiz += l.EastWest;
                            vert += l.NorthSouth;
                            NeSw += l.NortheastSouthwest;
                            NwSe += l.NorthwestSoutheast;

                        }
                    }
                }
            }
            st = "ew:" + horiz.ToString() + " ns:" + vert.ToString() + " nesw:" + NeSw.ToString() + " nwse:" + NwSe.ToString();
            /*if(horiz == 0)
            {
            }
            else if(horiz == 1)
            {
            }
            else if(horiz == 2)
            {
            }
            else
            {
            }*/
            return st;
        }
        private int CheckHoriz(Bitmap pic)
        {
            int lines = 0;
            for(int r = 0; r < pic.Height; r++)
            {
                int pixels = 0;
                int pix = 0;
                for(int c = 0; c < pic.Width; c++)
                {
                    if(pic.GetPixel(c, r).R < 100)
                    {
                        pix++;
                    }
                    else 
                    {
                        if(pix > pixels)
                        {
                            pixels = pix;
                        }
                        pix = 0;
                    }
                }
                if(pixels > 6)
                {
                    lines++;
                    r += 2;
                }
            }
            return lines;
        }
        private int CheckVert(Bitmap pic)
        {
            int lines = 0;
            for(int c = 0; c < pic.Width; c++)
            {
                int pixels = 0;
                int pix = 0;
                for(int r = 0; r < pic.Height; r++)
                {
                    if(pic.GetPixel(c, r).R < 100)
                    {
                        pix++;
                    }
                    else 
                    {
                        if(pix > pixels)
                        {
                            pixels = pix;
                        }
                        pix = 0;
                    }
                }
                if(pixels > 6)
                {
                    lines++;
                    c += 2;
                }
            }
            return lines;
        }
        private int CheckNWSE(Bitmap pic)
        {
            int lines = 0;
            for(int r = pic.Height - 1; r >= 0; r--)
            {
                int pixels = 0;
                int pix = 0;
                for(int c = 0; c < pic.Width && r < pic.Height; c++)
                {
                    if(pic.GetPixel(c, r).R < 100)
                    {
                        pix++;
                    }
                    else 
                    {
                        if(pix > pixels)
                        {
                            pixels = pix;
                        }
                        pix = 0;
                    }
                    r++;
                }
                if(pixels > 6)
                {
                    lines++;
                    r += 2;
                }
            }
            for(int c = 0; c < pic.Width; c++)
            {
                int pixels = 0;
                int pix = 0;
                for(int r = 0; c < pic.Width && r < pic.Height; r++)
                {
                    if(pic.GetPixel(c, r).R < 100)
                    {
                        pix++;
                    }
                    else 
                    {
                        if(pix > pixels)
                        {
                            pixels = pix;
                        }
                        pix = 0;
                    }
                    c++;
                }
                if(pixels > 6)
                {
                    lines++;
                    c += 2;
                }
            }
            return lines;
        }
        private int CheckNESW(Bitmap pic)
        {
            int lines = 0;
            for (int c = 0; c < pic.Width; c++)
            {
                int pixels = 0;
                int pix = 0;
                for (int r = 0; c >= 0 && r < pic.Height; r++)
                {
                    if (pic.GetPixel(c, r).R < 100)
                    {
                        pix++;
                    }
                    else
                    {
                        if (pix > pixels)
                        {
                            pixels = pix;
                        }
                        pix = 0;
                    }
                    c--;
                }
                if (pixels > 6)
                {
                    lines++;
                    c += 2;
                }
            }
            for (int r = 0; r < pic.Height; r++)
            {
                int pixels = 0;
                int pix = 0;
                for (int c = pic.Width - 1; c >= 0 && r < pic.Height; c--)
                {
                    if (pic.GetPixel(c, r).R < 100)
                    {
                        pix++;
                    }
                    else
                    {
                        if (pix > pixels)
                        {
                            pixels = pix;
                        }
                        pix = 0;
                    }
                    r++;
                }
                if (pixels > 6)
                {
                    lines++;
                    r += 2;
                }
            }
            return lines;
        }
    }
}
