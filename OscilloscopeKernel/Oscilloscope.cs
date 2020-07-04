using OscilloscopeCore.Drawing;
using OscilloscopeCore.Producer;
using OscilloscopeCore.Wave;
using OscilloscopeKernel.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OscilloscopeCore
{
    public abstract class Oscilloscope<T>
    {
        public WaveFixer XFixer => x_fixer;
        public WaveFixer TFixer => y_fixer;
        public WaveFixer InputFixer => x_fixer;
        public WaveFixer SweepFixer => y_fixer;

        private ICanvas<T> canvas;
        private IPointDrawer point_drawer;
        private IRulerDrawer ruler_drawer;
        private IGraphProducer graph_producer;
        private WaveFixer x_fixer = new WaveFixer();
        private WaveFixer y_fixer = new WaveFixer();

        protected Oscilloscope(
            ICanvas<T> canvas, 
            IPointDrawer point_drawer, 
            IRulerDrawer ruler_drawer,
            IGraphProducer graph_producer)
        {
            if (graph_producer.RequireThreadSafeDrawer && !point_drawer.IsThreadSafe)
            {
                throw new OscillocopeBuildException("graph_producer requires thread-safe PointDrawer but point_drawer is not");
            }
            if (canvas.Length != point_drawer.GraphLength || canvas.Length != ruler_drawer.GraphLength)
            {
                throw new OscillocopeBuildException("canvas.Length, point_drawer.GraphLength, ruler_drawer.GraphLenth are not the same");
            }
            if (canvas.Width != point_drawer.GraphWidth || canvas.Width != ruler_drawer.GraphWidth)
            {
                throw new OscillocopeBuildException("canvas.Width, point_drawer.GraphWidth, ruler_drawer.GraphWidth are not the same");
            }
            this.canvas = canvas;
            this.point_drawer = point_drawer;
            this.ruler_drawer = ruler_drawer;
            this.graph_producer = graph_producer;
        }

        protected T Draw(double delta_time)
        {
            return graph_producer.Produce(
                delta_time: delta_time,
                canvas: canvas,
                point_drawer: point_drawer,
                ruler_drawer: ruler_drawer,
                x_wave: x_fixer.GetCut(),
                y_wave: y_fixer.GetCut());
        }
    }

    public class SimpleOscilloscope<T> : Oscilloscope<T>
    {
        public SimpleOscilloscope(
            ICanvas<T> canvas,
            IPointDrawer point_drawer,
            IRulerDrawer ruler_drawer,
            IGraphProducer graph_producer)
            : base(canvas, point_drawer, ruler_drawer, graph_producer) { }

        public new T Draw(double delta_time)
        {
            return base.Draw(delta_time);
        }
    }

    public class TimeCountedOscilloscope<T> : Oscilloscope<T>
    {
        public DateTime last_time = DateTime.Now;

        public TimeCountedOscilloscope(
            ICanvas<T> canvas,
            IPointDrawer point_drawer,
            IRulerDrawer ruler_drawer,
            IGraphProducer graph_producer)
            : base(canvas, point_drawer, ruler_drawer, graph_producer) { }

        public T Draw()
        {
            DateTime now_time = DateTime.Now;
            TimeSpan ts = now_time - last_time;
            double delta_time = ts.TotalMilliseconds * 1000;
            last_time = now_time;
            return base.Draw(delta_time);
        }
    }

    public class DriveredOscilloscope<T> : Oscilloscope<T>
    {
        public ConcurrentQueue<T> Buffer => buffer;

        private double delta_time;
        private Timer timer;
        private ConcurrentQueue<T> buffer;

        public DriveredOscilloscope(
            ICanvas<T> canvas,
            IPointDrawer point_drawer,
            IRulerDrawer ruler_drawer,
            IGraphProducer graph_producer,
            ConcurrentQueue<T> buffer = null)
            : base(canvas, point_drawer, ruler_drawer, graph_producer)
        {
            if (buffer == null)
            {
                this.buffer = new ConcurrentQueue<T>();
            }
            else
            {
                this.buffer = buffer;
            }
        }

        public void Start(int delta_time)
        {
            int timer_delta_time = delta_time * 1000 / Waves.UNIT_NUMBER_PRO_SECOND;
            this.delta_time = timer_delta_time;
            timer = new Timer(o => buffer.Enqueue(base.Draw(this.delta_time)), null, 0, timer_delta_time);
        }

        public void End()
        {
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }
        }
    }
}
