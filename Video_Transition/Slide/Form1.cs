using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Slide {
    public partial class Form1 : Form {
        const int number = 800;
        int times = 0;

        Graphics g;
        Bitmap[] data = new Bitmap[number];

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            g = this.CreateGraphics();

            Bitmap srcBitmap1 = new Bitmap("..\\..\\..\\source1.jpg");
            Bitmap srcBitmap2 = new Bitmap("..\\..\\..\\source2.jpg");

            int wide = srcBitmap1.Width;
            int height = srcBitmap1.Height;

            Bitmap tempBitmap = new Bitmap(wide, height);

            Rectangle rect = new Rectangle(0, 0, 800, 600);

            for (int i = 0; i < wide; i++) {
                //將資料lock到系統內的記憶體的某個區塊
                BitmapData srcBmData1 = srcBitmap1.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                BitmapData srcBmData2 = srcBitmap2.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                BitmapData tempBmData = tempBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

                //取出位址
                System.IntPtr srcPtr1 = srcBmData1.Scan0;
                System.IntPtr srcPtr2 = srcBmData2.Scan0;
                System.IntPtr tempPtr = tempBmData.Scan0;

                unsafe {
                    //宣告指標
                    byte* srcP1 = (byte*)(void*)srcPtr1;
                    byte* srcP2 = (byte*)(void*)srcPtr2;
                    byte* tempP = (byte*)(void*)tempPtr;

                    for (int y = 0; y < height; y++) {
                        for (int x = 0; x < wide; x++, tempP += 4) {
                            if (i > x) {
                                int offset = ((wide + x - i) * 4) + (y * wide * 4);
                                *tempP = *(srcP1 + offset);
                                *(tempP + 1) = *(srcP1 + offset + 1);
                                *(tempP + 2) = *(srcP1 + offset + 2);
                                *(tempP + 3) = *(srcP1 + offset + 3);
                            }
                            else {
                                int offset = ((wide - x + i) * 4) + (y * wide * 4);
                                *tempP = *(srcP2 + offset);
                                *(tempP + 1) = *(srcP2 + offset + 1);
                                *(tempP + 2) = *(srcP2 + offset + 2);
                                *(tempP + 3) = *(srcP2 + offset + 3);
                            }
                        }
                    }
                }

                //解鎖位圖
                srcBitmap1.UnlockBits(srcBmData1);
                srcBitmap2.UnlockBits(srcBmData2);
                tempBitmap.UnlockBits(tempBmData);

                data[i] = tempBitmap.Clone() as Bitmap;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            timer1.Interval = int.Parse(textBox1.Text);
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e) {
            if (times < number) {
                g.DrawImage(data[times], 50, 50);
                times++;
            }
            else {
                times = 0;
                timer1.Enabled = false;
            }
        }
    }
}
