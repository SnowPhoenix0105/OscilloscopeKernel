using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OscilloscopeFramework;
using OscilloscopeKernel;
using OscilloscopeKernel.Drawing;
using OscilloscopeKernel.Producer;
using OscilloscopeKernel.Tools;
using OscilloscopeKernel.Wave;

namespace OscilloscopeFrameworkTest
{
    [TestClass]
    public class TotalTest
    {
        [TestMethod]
        public void TestAll()
        {
            CathodeRayTubPanel panel = new CathodeRayTubPanel();
            panel.XWave.Wave = new SinWave(Waves.UNIT_NUMBER_PRO_SECOND / 50, 0.4);
            panel.YWave.Wave = new SawToothWave(Waves.UNIT_NUMBER_PRO_SECOND / 50, 0.4);
            ConstructorTuple<ICanvas<Bitmap>> canvas_constructor 
                = new ConstructorTuple<ICanvas<Bitmap>>(typeof(BitmapCanvas), 360, 360);
            ConstructorTuple<IPointDrawer> point_drawer_constructor
                = new ConstructorTuple<IPointDrawer>(typeof(ConcurrentOvalPointDrawer), 360, 360);
            IRulerDrawer ruler_drawer = new CrossRulerDrawer(360, 360, Color.FromArgb(0xff, 0x10, 0x00, 0xff), 1);
            IGraphProducer graph_producer = new TotalConcurrentProducer(1234, Color.FromArgb(0xff, 0x10, 0x20, 0xff));
            DrivedOscilloscope<Bitmap> oscilloscope
                = new DrivedOscilloscope<Bitmap>(
                    canvas_constructor: canvas_constructor,
                    point_drawer_constructor: point_drawer_constructor,
                    ruler_drawer: ruler_drawer,
                    graph_producer: graph_producer,
                    control_panel: panel);
            ConcurrentQueue<Bitmap> buffer = oscilloscope.Buffer;
            Timer timer = new Timer(o => DequeueAndSave(buffer), null, 20, 20);
            oscilloscope.Start(20_000);
            Thread.Sleep(2_000);
            oscilloscope.End();
            Thread.Sleep(100);
            timer.Dispose();
        }

        private readonly string save_path = "..\\..\\TestResource\\";
        private int save_count = 0;

        private void DequeueAndSave(ConcurrentQueue<Bitmap> queue)
        {
            Bitmap new_graph;
            if (queue.TryDequeue(out new_graph))
            {
                string path;
                lock(save_path)
                {
                    path = save_path + (save_count++).ToString() + ".png";
                }
                FileStream file = File.OpenWrite(path);
                new_graph.Save(file, ImageFormat.Png);
                file.Close();
                new_graph.Dispose();
            }
        }
    }
}
