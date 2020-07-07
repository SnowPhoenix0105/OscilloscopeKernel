using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OscilloscopeFramework;
using System.Drawing;
using System.Threading;

namespace OscilloscopeFrameworkTest
{
    [TestClass]
    public class BitmapCanvasTest
    {
        private int CHANGE_TIMES = 1000;
        private Color back_ground = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
        private Color point = Color.FromArgb(0xff, 0x0, 0x0, 0x0);
        private Random rnd = new Random();

        private int TestConvas(BitmapCanvas canvas, int min_x, int sup_x, int min_y, int sup_y)
        {
            int wait_count = 0;
            while (!canvas.IsReady)
            {
                wait_count++;
                Thread.Yield();
            }
            for (int i = min_x; i < sup_x; i++)
            {
                Assert.AreEqual(back_ground, canvas[i, min_y]);
                Assert.AreEqual(back_ground, canvas[i, sup_y - 1]);
                Assert.AreEqual(default(Color), canvas[i, min_y - 1]);
                Assert.AreEqual(default(Color), canvas[i, sup_y]);
            }
            for (int i = min_y; i < sup_y; i++)
            {
                Assert.AreEqual(back_ground, canvas[min_x, i]);
                Assert.AreEqual(back_ground, canvas[sup_x - 1, i]);
                Assert.AreEqual(default(Color), canvas[min_x - 1, i]);
                Assert.AreEqual(default(Color), canvas[sup_x, i]);
            }
            for (int i = 0; i < CHANGE_TIMES; i++)
            {
                int x = rnd.Next(min_x, sup_x);
                int y = rnd.Next(min_y, sup_y);
                Color old = canvas[x, y];
                Color new_color = old.Equals(back_ground) ? point : back_ground;
                canvas[x, y] = new_color;
                Assert.AreEqual(new_color, canvas[x, y]);
            }
            Bitmap out1 = canvas.Output();
            while (!canvas.IsReady)
            {
                wait_count++;
                Thread.Yield();
            }
            out1.Dispose();
            for (int x = min_x; x < sup_x; x++)
            {
                for (int y = min_y; y < sup_y; y++)
                {
                    Assert.AreEqual(canvas[x, y], back_ground);
                }
            }
            return wait_count;
        }


        [TestMethod]
        public void TestReset()
        {
            BitmapCanvas canvas = new BitmapCanvas(360, 360, back_ground);
            int wait_count = TestConvas(canvas, -180, 180, -180, 180);
            Console.WriteLine("360 * 360, wait_count:\t" + wait_count.ToString());


            canvas = new BitmapCanvas(361, 361, back_ground);
            wait_count = TestConvas(canvas, -180, 181, -180, 181);
            Console.WriteLine("361 * 361, wait_count:\t" + wait_count.ToString());


            canvas = new BitmapCanvas(360, 361, back_ground);
            wait_count = TestConvas(canvas, -180, 180, -180, 181);
            Console.WriteLine("360 * 361, wait_count:\t" + wait_count.ToString());


            canvas = new BitmapCanvas(720, 720, back_ground);
            wait_count = TestConvas(canvas, -360, 360, -360, 360);
            Console.WriteLine("720 * 720, wait_count:\t" + wait_count.ToString());


            canvas = new BitmapCanvas(360, 720, back_ground);
            wait_count = TestConvas(canvas, -180, 180, -360, 360);
            Console.WriteLine("360 * 720, wait_count:\t" + wait_count.ToString());
        }
    }
}
