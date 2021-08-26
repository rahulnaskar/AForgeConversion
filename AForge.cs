using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Zen.Barcode;

namespace Utils
{
    public class AForge : Imagery
    {
        //Implementation of Barcode Generation
        public void GenerateBarcode(string BarText)
        {
            Zen.Barcode.Code128BarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
            var image = barcode.Draw(BarText, 50);
            Bitmap bitmap = new Bitmap(image);
            using (var graphics = Graphics.FromImage(image))
            using (var font = new Font("Consolas", 12)) // Any font you want
            using (var brush = new SolidBrush(Color.White))
            using (var format = new StringFormat() { LineAlignment = StringAlignment.Far }) // To align text above the specified point
            {
                
                graphics.DrawString(BarText, font, brush, 0, image.Height, format);
            }
            bitmap.Save("barcode.tif", System.Drawing.Imaging.ImageFormat.Tiff);
        }
    }
}
