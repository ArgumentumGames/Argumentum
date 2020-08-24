using System;
using System.Drawing;

namespace NBrightCore.images
{
    public class ImgWaterMark
    {


        private Bitmap _bmp;
        private readonly Bitmap _wbmp;

        private readonly bool _isPng;

        public ImgWaterMark(string physicalPathToImage, string physicalPathToWatermarkImage) : this(physicalPathToImage, physicalPathToWatermarkImage, false)
        {
        }

        public ImgWaterMark(string physicalPathToImage, string physicalPathToWatermarkImage, bool makeTransparent)
        {
            _bmp = new Bitmap(physicalPathToImage);
            _wbmp = new Bitmap(physicalPathToWatermarkImage);
            if (makeTransparent)
            {
                var extension = System.IO.Path.GetExtension(physicalPathToWatermarkImage);
                if (extension != null && extension.ToLower() == ".png")
                {
                    _isPng = true;
                }
                else
                {
                    _isPng = false;
                }
            }
            else
            {
                //set png so no tranparent is done.
                _isPng = true;
            }
        }

        public void AddWaterMark()
        {
            //get the drawing canvas (graphics object) from the bitmap

// ReSharper disable PossibleLossOfFraction
            float x = (_bmp.Width - _wbmp.Width)/2;
// ReSharper restore PossibleLossOfFraction
// ReSharper disable PossibleLossOfFraction
            float y = (_bmp.Height - _wbmp.Height)/2;
// ReSharper restore PossibleLossOfFraction

            Graphics canvas;
            try
            {
                canvas = Graphics.FromImage(_bmp);
            }
            catch (Exception)
            {
                //You cannot create a Graphics object 
                //from an image with an indexed pixel format.
                //If you want to open this image and draw 
                //on it you need to do the following...
                //size the new bitmap to the source bitmaps dimensions
                var bmpNew = new Bitmap(_bmp.Width, _bmp.Height);
                canvas = Graphics.FromImage(bmpNew);
                //draw the old bitmaps contents to the new bitmap
                //paint the entire region of the old bitmap to the 
                //new bitmap..use the rectangle type to 
                //select area of the source image
                canvas.DrawImage(_bmp, new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), 0, 0, _bmp.Width, _bmp.Height,
                                 GraphicsUnit.Pixel);
                _bmp = bmpNew;
            }

            canvas.DrawImage(DrawWatermark(_wbmp), x, y, _wbmp.Width, _wbmp.Height);

            //release image
            _wbmp.Dispose();

        }

        public Bitmap Image
        {
            get { return _bmp; }
        }

        private Bitmap DrawWatermark(Bitmap watermarkBm)
        {
            if (!_isPng)
            {
                watermarkBm.MakeTransparent(watermarkBm.GetPixel(0, 0));
            }
            return watermarkBm;
        }

    }
}
