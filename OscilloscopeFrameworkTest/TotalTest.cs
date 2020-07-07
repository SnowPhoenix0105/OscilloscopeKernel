//#define NEED_TIME_COMPARE

#if NEED_TIME_COMPARE


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
        public void PngOutputTest()
        {
            Color background_color = Color.FromArgb(0xff, 0x00, 0x00, 0x00);
            Color ruler_color = Color.FromArgb(0xff, 0x10, 0x00, 0xff);
            Color graph_color = Color.FromArgb(0xff, 0x10, 0xff, 0x10);
            CathodeRayTubPanel panel = new CathodeRayTubPanel();
            panel.XWave.Wave = new SinWave(Waves.UNIT_NUMBER_PRO_SECOND / 200, 0.4);
            panel.YWave.Wave = new SinWave(Waves.UNIT_NUMBER_PRO_SECOND / 100, 0.4);
            panel.PointLength = 1;
            panel.PointWidth = 1;
            ConstructorTuple<ICanvas<Bitmap>> canvas_constructor 
                = new ConstructorTuple<ICanvas<Bitmap>>(typeof(BitmapCanvas), 360, 360, background_color);
            ConstructorTuple<IPointDrawer> multi_point_drawer_constructor
                = new ConstructorTuple<IPointDrawer>(typeof(ConcurrentOvalPointDrawer), 360, 360);
            IRulerDrawer ruler_drawer = new CrossRulerDrawer(360, 360, ruler_color, 1);
            IGraphProducer total_parallel_graph_producer = new TotalParallelProducer(4321, graph_color);
            DrivedOscilloscope<Bitmap> drived_total_parallel_oscilloscope
                = new DrivedOscilloscope<Bitmap>(
                    canvas_constructor: canvas_constructor,
                    point_drawer_constructor: multi_point_drawer_constructor,
                    ruler_drawer: ruler_drawer,
                    graph_producer: total_parallel_graph_producer,
                    control_panel: panel);
            ConcurrentQueue<Bitmap> buffer = drived_total_parallel_oscilloscope.Buffer;
            save_count = 0;
            Timer timer = new Timer(o => DequeueAndSave(buffer, base_path + "Drived_TotalParallel\\"), null, 20, 10);
            drived_total_parallel_oscilloscope.Start(10_000);
            Thread.Sleep(2_000);
            drived_total_parallel_oscilloscope.End();
            Thread.Sleep(2_000);
            timer.Dispose();
            Console.WriteLine("drived-oscilloscope + total_parallel_producer -> time:\t" + save_count.ToString());

            ConstructorTuple<IPointDrawer> single_point_drawer_constructor
                = new ConstructorTuple<IPointDrawer>(typeof(OvalPointDrawer), 360, 360);
            IGraphProducer serial_graph_producer = new SerialProducer(4321, graph_color);
            DrivedOscilloscope<Bitmap> drived_serial_oscilloscope
                = new DrivedOscilloscope<Bitmap>(
                    canvas_constructor: canvas_constructor,
                    point_drawer_constructor: single_point_drawer_constructor,
                    ruler_drawer: ruler_drawer,
                    graph_producer: serial_graph_producer,
                    control_panel: panel);
            buffer = drived_total_parallel_oscilloscope.Buffer;
            save_count = 0;
            timer = new Timer(o => DequeueAndSave(buffer, base_path + "Drived_Serial\\"), null, 20, 10);
            drived_total_parallel_oscilloscope.Start(10_000);
            Thread.Sleep(2_000);
            drived_total_parallel_oscilloscope.End();
            Thread.Sleep(2_000);
            timer.Dispose();
            Console.WriteLine("drived-oscilloscope + serial_producer -> time:\t" + save_count.ToString());

            IGraphProducer simple_graph_producer = new SimpleProducer(4321, graph_color);
            ICanvas<Bitmap> canvas = canvas_constructor.NewInstance();
            IPointDrawer oval_single_drawer = new OvalPointDrawer(360, 360);
            TimeCountedOscilloscope<Bitmap> simple_oscilloscope
                = new TimeCountedOscilloscope<Bitmap>(
                    canvas: canvas,
                    point_drawer: oval_single_drawer,
                    ruler_drawer: ruler_drawer,
                    graph_producer: simple_graph_producer,
                    control_panel: panel);
            DateTime start_time = DateTime.Now;
            buffer = new ConcurrentQueue<Bitmap>();
            save_count = 0;
            timer = new Timer(o => DequeueAndSave(buffer, base_path + "Simple\\"), null, 20, 10);
            while (true)
            {
                DateTime now_time = DateTime.Now;
                TimeSpan delta_time = now_time - start_time;
                if (delta_time.TotalMilliseconds >= 2_000)
                {
                    break;
                }
                Bitmap output = simple_oscilloscope.Draw();
                buffer.Enqueue(output);
            }
            Thread.Sleep(2_000);
            timer.Dispose();
            Console.WriteLine("simple-oscilloscope -> time:\t" + save_count.ToString());
        }

        private readonly string base_path = "..\\..\\TestResource\\";
        private int save_count = 0;

        private void DequeueAndSave(ConcurrentQueue<Bitmap> queue, string save_path)
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
#endif


