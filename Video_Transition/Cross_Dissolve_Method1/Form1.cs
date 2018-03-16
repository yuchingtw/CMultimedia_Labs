using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cross_Dissolve_Method1 {
    public partial class Form1 : Form {
        Graphics g;

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            g = this.CreateGraphics();
        }

        private void button1_Click(object sender, EventArgs e) {
            Bitmap srcBitmap1 = new Bitmap("..\\..\\..\\source1.jpg");
            Bitmap srcBitmap2 = new Bitmap("..\\..\\..\\source2.jpg");

            int wide = srcBitmap1.Width;
            int height = srcBitmap1.Height;

            Bitmap tempBitmap = new Bitmap(wide, height);

            Color srcColor1, srcColor2, tempColor;

            DateTime time_start = DateTime.Now;
            for (double i = 0.0; i < 1; i = i + 0.1) {
                for (int y = 0; y < height; y++) {
                    for (int x = 0; x < wide; x++) {
                        srcColor1 = srcBitmap1.GetPixel(x, y);
                        srcColor2 = srcBitmap2.GetPixel(x, y);

                        tempColor = Color.FromArgb(
                            (int)(srcColor1.A * (1 - i) + srcColor2.A * i),
                            (int)(srcColor1.R * (1 - i) + srcColor2.R * i),
                            (int)(srcColor1.G * (1 - i) + srcColor2.G * i),
                            (int)(srcColor1.B * (1 - i) + srcColor2.B * i)
                        );

                        tempBitmap.SetPixel(x, y, tempColor);
                    }
                }
                g.DrawImage(tempBitmap, 50, 50);
            }
            DateTime time_end = DateTime.Now;
            textBox1.Text = ((TimeSpan)(time_end - time_start)).TotalMilliseconds.ToString();
        }

    }
}