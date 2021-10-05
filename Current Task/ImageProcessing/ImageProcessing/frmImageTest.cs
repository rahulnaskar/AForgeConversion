using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using AforgeClassLibrary;
using System.Drawing.Drawing2D;

namespace ImageProcessing
{
    public partial class frmImageTest : Form
    {
        Image TmpImg;
        AforgeImage afImg = new AforgeImage();
        int cropX;
        int cropY;
        Pen cropPen;
        int operation;

        public frmImageTest()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            listBox1.Items.Clear();
            FolderBrowserDialog fdlg = new FolderBrowserDialog();
            DialogResult result = fdlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (Directory.Exists(fdlg.SelectedPath + "\\Backup"))
                {
                    Directory.Delete(fdlg.SelectedPath + "\\Backup", true);
                    Directory.CreateDirectory(fdlg.SelectedPath + "\\Backup");

                }
                else
                {
                    Directory.CreateDirectory(fdlg.SelectedPath + "\\Backup");
                }
                textBox1.Text = fdlg.SelectedPath + "\\Backup";
                if (Directory.GetFiles(fdlg.SelectedPath).Length > 0)
                {
                    foreach (string file in Directory.GetFiles(fdlg.SelectedPath))
                    {
                        if (Path.GetExtension(file).ToLower() == ".jpg" || Path.GetExtension(file).ToLower() == ".tif" || Path.GetExtension(file).ToLower() == ".png")
                        {
                            File.Copy(file, fdlg.SelectedPath + "\\Backup" + "\\" + Path.GetFileName(file));
                        }
                    }
                }
                else
                {
                    return;
                }
                foreach (string item in Directory.GetFiles(textBox1.Text))
                {
                    listBox1.Items.Add(Path.GetFileName(item));
                }
                listBox1.SelectedIndex = 0;
                pictureBox1.ImageLocation = textBox1.Text + "\\" + listBox1.Items[0].ToString();
                TmpImg = new Bitmap(textBox1.Text + "\\" + listBox1.Items[0].ToString());
            }
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            pictureBox1.ImageLocation = textBox1.Text + "\\" + listBox1.SelectedItem.ToString();
            TmpImg = new Bitmap(textBox1.Text + "\\" + listBox1.SelectedItem.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                double angle;
                if (TmpImg.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
                {
                    angle = afImg.GetSkewAngle(TmpImg);
                    if (angle != 0)
                    {
                        TmpImg = afImg.Deskew(TmpImg, angle);
                        pictureBox1.Image = TmpImg;
                        //TmpImg.Save(textBox1.Text + "\\" + listBox1.SelectedItem.ToString(), System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    else
                        return;

                }
                else
                {
                    //afImg.ConvertToGreyScale(TmpImg);
                    System.Drawing.Image gImage = afImg.ConvertToGreyScale(TmpImg);
                    //string pixelFormat = gImage.PixelFormat.ToString();
                    if (gImage.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
                    {
                        angle = afImg.GetSkewAngle(gImage);
                    }
                    else
                    {
                        gImage = afImg.ConvertTo8bpp(gImage);
                        angle = afImg.GetSkewAngle(gImage);
                        gImage.Dispose();
                    }

                    //textBox1.Text = System.Drawing.Image.GetPixelFormatSize(image1.PixelFormat).ToString();
                    if (angle != 0)
                    {
                        TmpImg = afImg.Deskew(TmpImg, angle);
                        pictureBox1.Image = TmpImg;
                        //TmpImg.Save("Res.jpg");
                        
                        //TmpImg.Save(textBox1.Text + "\\" + listBox1.SelectedItem.ToString(), System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    else
                        return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TmpImg = afImg.Despeckle(TmpImg);
            pictureBox1.Image = TmpImg;
            string path = textBox1.Text + "\\" + listBox1.SelectedItem.ToString();
            //File.Delete(textBox1.Text + "\\" + listBox1.SelectedItem.ToString());
            //TmpImg.Save()
            //TmpImg.Save(textBox1.Text + "\\" + listBox1.SelectedItem.ToString());

        }

        private void button4_Click(object sender, EventArgs e)
        {
            TmpImg = afImg.RotateLeft90(TmpImg);
            pictureBox1.Image = TmpImg;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TmpImg = afImg.RotateRight90(TmpImg);
            pictureBox1.Image = TmpImg;
        }
        public void cropInit()
        {
            operation = 1;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            cropInit();
        }
        

        private void button8_Click(object sender, EventArgs e)
        {
            //Rectangle rect = new Rectangle(5,5,5,5);
            //double wdratio = Convert.ToDouble(TmpImg.Size.Width) / Convert.ToDouble(pictureBox1.Width);
            //rect.Width = Convert.ToInt32(Convert.ToDouble(rect.Width) * wdratio);
            //double htratio = Convert.ToDouble(TmpImg.Size.Height) / Convert.ToDouble(pictureBox1.Height);
            //rect.Height = Convert.ToInt32(Convert.ToDouble(rect.Height) * htratio);
            //rect.X = Convert.ToInt32(rect.X * wdratio);
            //rect.Y = Convert.ToInt32(rect.Y * htratio);
            TmpImg = afImg.AutoCrop(TmpImg);
            pictureBox1.Image = TmpImg;
        }

        private void pictureBox1_MouseUp_1(object sender, MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Cursor = Cursors.Default;
            if(operation == 1)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left && pictureBox1.Image != null)
                {
                    // Rectangle selectedRect = rect;
                    int rectW = Math.Abs(e.X - cropX);
                    int rectH = Math.Abs(e.Y - cropY);
                    if (rectW > 1)
                    {
                        Rectangle rect = new Rectangle(((cropX > e.X) ? e.X : cropX), ((cropY > e.Y) ? e.Y : cropY), rectW, rectH);
                        // rect = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
                        double wdratio = Convert.ToDouble(TmpImg.Size.Width) / Convert.ToDouble(pictureBox1.Width);
                        rect.Width = Convert.ToInt32(Convert.ToDouble(rect.Width) * wdratio);
                        double htratio = Convert.ToDouble(TmpImg.Size.Height) / Convert.ToDouble(pictureBox1.Height);
                        rect.Height = Convert.ToInt32(Convert.ToDouble(rect.Height) * htratio);
                        rect.X = Convert.ToInt32(rect.X * wdratio);
                        rect.Y = Convert.ToInt32(rect.Y * htratio);
                        //textBox1.Text = System.Drawing.Image.GetPixelFormatSize(TmpImg.PixelFormat).ToString();
                        TmpImg = afImg.Areacrop(TmpImg, rect);
                        // pictureBox1.Image = null;
                        pictureBox1.Image = TmpImg;
                    }
                    else
                    {
                        Cursor = Cursors.Default;
                    }
                }
            }
        }

        private void pictureBox1_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (operation == 1)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    if ((cropPen != null) && (pictureBox1.Image != null))
                    {
                        Cursor = Cursors.Cross;
                        pictureBox1.Refresh();
                        int rectW = Math.Abs(e.X - cropX);
                        int rectH = Math.Abs(e.Y - cropY);
                        pictureBox1.CreateGraphics().DrawRectangle(cropPen, ((cropX > e.X) ? e.X : cropX), ((cropY > e.Y) ? e.Y : cropY), rectW, rectH);
                    }
                }
            }
            else
                Cursor = Cursors.Default;
        }

        private void pictureBox1_MouseDown_1(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                cropX = e.X;
                cropY = e.Y;
                cropPen = new Pen(System.Drawing.Color.Blue, 1);
                cropPen.DashStyle = DashStyle.Dash;
                Cursor = Cursors.Cross;
            }
        }

        private void frmImageTest_Load(object sender, EventArgs e)
        {

        }
    }
}
