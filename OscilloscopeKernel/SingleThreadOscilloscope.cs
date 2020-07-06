using OscilloscopeKernel.Drawing;
using OscilloscopeKernel.Producer;
using OscilloscopeKernel.Wave;
using OscilloscopeKernel.Exceptions;
using OscilloscopeKernel.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OscilloscopeKernel
{
    public abstract class Oscilloscope
    {
        public WaveFixer XFixer => x_fixer;
        public WaveFixer YFixer => y_fixer;
        public WaveFixer InputFixer => x_fixer;
        public WaveFixer SweepFixer => y_fixer;

        private WaveFixer x_fixer = new WaveFixer();
        private WaveFixer y_fixer = new WaveFixer();
    }

    public abstract class SingleThreadOscilloscope<T> : Oscilloscope
    {
        private ICanvas<T> canvas;
        private IPointDrawer point_drawer;
        private IRulerDrawer ruler_drawer;
        private IGraphProducer graph_producer;
        private IControlPanel control_panel;

        protected SingleThreadOscilloscope(
            ICanvas<T> canvas, 
            IPointDrawer point_drawer, 
            IRulerDrawer ruler_drawer,
            IGraphProducer graph_producer,
            IControlPanel control_panel)
        {
            if (canvas.GraphSize != point_drawer.GraphSize || canvas.GraphSize != ruler_drawer.GraphSize)
            {
                throw new OscillocopeBuildException("canvas, point_drawer, ruler_drawer have different GraphSize");
            }
            if (graph_producer.RequireMultiThreadDrawer && !point_drawer.IsMultiThreadSafe)
            {
                throw new OscillocopeBuildException("graph_producer require multi-thread-safe PointDrawer but point_drawer is not");
            }
            this.canvas = canvas;
            this.point_drawer = point_drawer;
            this.ruler_drawer = ruler_drawer;
            this.graph_producer = graph_producer;
            this.control_panel = control_panel;
        }

        protected T Draw(double delta_time)
        {
            ruler_drawer.Draw(canvas);
            graph_producer.Produce(
                delta_time: delta_time,
                canvas: canvas,
                point_drawer: point_drawer,
                information: control_panel.GetStateShot());
            return canvas.Output();
        }
    }

    public class SimpleOscilloscope<T> : SingleThreadOscilloscope<T>
    {
        public SimpleOscilloscope(
            ICanvas<T> canvas,
            IPointDrawer point_drawer,
            IRulerDrawer ruler_drawer,
            IGraphProducer graph_producer,
            IControlPanel control_panel)
            : base(canvas, point_drawer, ruler_drawer, graph_producer, control_panel) { }

        public new T Draw(double delta_time)
        {
            return base.Draw(delta_time);
        }
    }

    public class TimeCountedOscilloscope<T> : SingleThreadOscilloscope<T>
    {
        public DateTime last_time = DateTime.Now;

        public TimeCountedOscilloscope(
            ICanvas<T> canvas,
            IPointDrawer point_drawer,
            IRulerDrawer ruler_drawer,
            IGraphProducer graph_producer,
            IControlPanel control_panel)
            : base(canvas, point_drawer, ruler_drawer, graph_producer, control_panel) { }

        public T Draw()
        {
            DateTime now_time = DateTime.Now;
            TimeSpan ts = now_time - last_time;
            double delta_time = ts.TotalMilliseconds * 1000;
            last_time = now_time;
            return base.Draw(delta_time);
        }
    }
}
