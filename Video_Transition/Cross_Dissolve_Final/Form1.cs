using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Cross_Dissolve_Final {
    public partial class Form1 : Form {
        const int number = 50;
        int times = 0;

        Graphics g;
        Bitmap[] data = new Bitmap[number+1];

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

            int cnt = 0;
            for (double i = 0.0; i < 1.0; i = i + (1.0 / number)) {
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
                        for (int x = 0; x < wide; x++, srcP1 += 4, srcP2 += 4, tempP += 4) {
                            *tempP = (byte)(srcP1[0] * (1 - i) + srcP2[0] * i);
                            *(tempP + 1) = (byte)(srcP1[1] * (1 - i) + srcP2[1] * i);
                            *(tempP + 2) = (byte)(srcP1[2] * (1 - i) + srcP2[2] * i);
                            *(tempP + 3) = (byte)(srcP1[3] * (1 - i) + srcP2[3] * i);
                        }
                    }
                }

                //解鎖位圖
                srcBitmap1.UnlockBits(srcBmData1);
                srcBitmap2.UnlockBits(srcBmData2);
                tempBitmap.UnlockBits(tempBmData);

                data[cnt] = tempBitmap.Clone() as Bitmap;
                cnt++;
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
