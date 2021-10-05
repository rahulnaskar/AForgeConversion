using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
using AForge.Imaging.Filters;
using AForge;
using AForge.Imaging;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using AForge.Math;
namespace AforgeClassLibrary
{
   public class AforgeImage
    { 
       public System.Drawing.Image Deskew(System.Drawing.Image image, double angle)
        {
            //Bitmap bmp = (Bitmap)image;
            //RotateBilinear rotate = new RotateBilinear(-angle, true);
            RotateBicubic rotate = new RotateBicubic(-angle, true);
            Bitmap imageDeskew = rotate.Apply((Bitmap)image);
            return (System.Drawing.Image)imageDeskew;
            //}
        }    
        public System.Drawing.Image RotateLeft90(System.Drawing.Image image)
        {
            Bitmap bmp = (Bitmap)image;     
            RotateBilinear rotate = new RotateBilinear(-90);
            Bitmap rotate90 = rotate.Apply(bmp);
            return (System.Drawing.Image)rotate90;
        }
        public System.Drawing.Image RotateRight90(System.Drawing.Image image)
        {
            Bitmap bmp = (Bitmap)image;
            RotateBilinear rotate = new RotateBilinear(90);
            Bitmap rotate90 = rotate.Apply(bmp);
            return (System.Drawing.Image)rotate90;
        }
        public System.Drawing.Image AutoCrop(System.Drawing.Image image)
        {
            Bitmap bmp = (Bitmap)image;
            Rectangle rect = new Rectangle();
            //rect.X = image.Width - 5;
            //rect.Y = image.Height - 5;
            rect.Height = image.Height - 10;
            rect.Width = image.Width - 10;
            Crop crop = new Crop(rect);
            Bitmap CropImage = crop.Apply(bmp);
            return (System.Drawing.Image)CropImage;
        }
        public System.Drawing.Image Despeckle(System.Drawing.Image image)
        {
            Bitmap bmp = (Bitmap)image;
            Median filter = new Median();
            Bitmap DespeckleImage = filter.Apply(bmp);
            return (System.Drawing.Image)DespeckleImage;
        }
        public System.Drawing.Image Areacrop(System.Drawing.Image image, Rectangle rect)
        {
            Bitmap bmp = (Bitmap)image;
           // Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
            //Convert16bppTo8bpp
            Crop crop = new Crop(rect);
            Bitmap cropImage = crop.Apply(bmp);
            return (System.Drawing.Image)cropImage;
        }
        public System.Drawing.Image ConvertToGreyScale(System.Drawing.Image image)
        {
            Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
            Bitmap GreyImage = filter.Apply((Bitmap)image);
            return GreyImage;
        }
       public System.Drawing.Image ConvertTo8bpp(System.Drawing.Image image)
        {
            Bitmap img8bpp = AForge.Imaging.Image.Convert16bppTo8bpp((Bitmap)image);
            return (System.Drawing.Image)img8bpp;
        }
       public double GetSkewAngle(System.Drawing.Image image)
       {
           DocumentSkewChecker checkangle = new DocumentSkewChecker();
           double angle = checkangle.GetSkewAngle((Bitmap)image);
           return angle;
       }
    }
}
