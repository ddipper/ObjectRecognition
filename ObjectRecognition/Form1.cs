using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Alturos.Yolo;
using Alturos.Yolo.Model;
using System.IO;
using System.Drawing.Imaging;

namespace ObjectRecognition
{
    public partial class Form1 : Form    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "JPG|*.jpg|PNG|*.png|BMP|*.bmp";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
            label1.Text = "Image selected";
        }

        private void buttonRecognize_Click(object sender, EventArgs e)
        {
            label1.Text = "Processing in progress";

            YoloWrapper yolo = new YoloWrapper("yolov3.cfg", "yolov3.weights", "coco.names");

            MemoryStream ms = new MemoryStream();
            pictureBox1.Image.Save(ms, ImageFormat.Jpeg);

            List<YoloItem> yi = yolo.Detect(ms.ToArray().ToString()).ToList<YoloItem>();

            Image img = pictureBox1.Image;
            Graphics graphics = Graphics.FromImage(img);

            Font font = new Font("Consolas", 12, FontStyle.Bold);
            SolidBrush solidBrush = new SolidBrush(Color.Red);

            foreach(YoloItem i in yi)
            {
                Point point = new Point(i.X, i.Y);
                Size size = new Size(i.Width, i.Height);

                Rectangle rectangle = new Rectangle(point, size);

                Pen pen = new Pen(Color.Red, 3);

                graphics.DrawRectangle(pen, rectangle);
                graphics.DrawString(i.Type, font, solidBrush, point);
            }

            pictureBox1.Image = img;
            label1.Text = "Processing completed";
        }
    }
}
