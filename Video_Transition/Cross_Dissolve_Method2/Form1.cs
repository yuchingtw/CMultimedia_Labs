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

namespace Cross_Dissolve_Method2 {
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

            Rectangle rect = new Rectangle(0, 0, 800, 600);

            DateTime time_start = DateTime.Now;
            for (double i = 0.0; i < 1; i = i + 0.1) {
                //將資料lock到系統內的記憶體的某個區塊
                BitmapData srcBmData1 = srcBitmap1.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                BitmapData srcBmData2 = srcBitmap2.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                BitmapData tempBmData = tempBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

                //取出位址
                System.IntPtr srcPtr1 = srcBmData1.Scan0;
                System.IntPtr srcPtr2 = srcBmData2.Scan0;
                System.IntPtr tempPtr = tempBmData.Scan0;

                //宣告陣列
                byte[] srcValues​​1 = new byte[srcBmData1.Stride * height];
                byte[] srcValues​​2 = new byte[srcBmData2.Stride * height];
                byte[] tempValues​​ = new byte[tempBmData.Stride * height];

                //複製ARGB資料複製進去陣列
                System.Runtime.InteropServices.Marshal.Copy(srcPtr1, srcValues1​​, 0, srcBmData1.Stride * height);
                System.Runtime.InteropServices.Marshal.Copy(srcPtr2, srcValues2, 0, srcBmData2.Stride * height);
                System.Runtime.InteropServices.Marshal.Copy(tempPtr, tempValues, 0, tempBmData.Stride * height);

                for (int y = 0; y < height; y++) {
                    for (int x = 0; x < wide; x++) {
                        int k = 4 * x;
                        tempValues​​[y * tempBmData.Stride + k + 3] = (byte)(
                            srcValues​​1[y * srcBmData1.Stride + k + 3] * (1 - i) + srcValues​​2[y * srcBmData2.Stride + k + 3] * i
                        );
                        tempValues​​[y * tempBmData.Stride + k + 2] = (byte)(
                            srcValues​​1[y * srcBmData1.Stride + k + 2] * (1 - i) + srcValues​​2[y * srcBmData2.Stride + k + 2] * i
                        );
                        tempValues​​[y * tempBmData.Stride + k + 1] = (byte)(
                            srcValues​​1[y * srcBmData1.Stride + k + 1] * (1 - i) + srcValues​​2[y * srcBmData2.Stride + k + 1] * i
                        );
                        tempValues​​[y * tempBmData.Stride + k] = (byte)(
                            srcValues​​1[y * srcBmData1.Stride + k] * (1 - i) + srcValues​​2[y * srcBmData2.Stride + k] * i
                        );
                    }
                }
                //回存
                System.Runtime.InteropServices.Marshal.Copy(tempValues, 0, tempPtr, tempBmData.Stride * height);

                //解鎖位圖
                srcBitmap1.UnlockBits(srcBmData1);
                srcBitmap2.UnlockBits(srcBmData2);
                tempBitmap.UnlockBits(tempBmData);

                g.DrawImage(tempBitmap, 50, 50);
            }
            DateTime time_end = DateTime.Now;
            textBox1.Text = ((TimeSpan)(time_end - time_start)).TotalMilliseconds.ToString();
        }
    }
}
