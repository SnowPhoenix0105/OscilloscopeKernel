using OscilloscopeKernel.Drawing;
using OscilloscopeKernel.Exceptions;
using OscilloscopeKernel.Producer;
using OscilloscopeKernel.Tools;
using OscilloscopeKernel.Wave;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;

namespace OscilloscopeKernel
{
    public class MultiThreadOscilloscope<T> : Oscilloscope
    {
        public ConcurrentQueue<T> Buffer => buffer;

        private IRulerDrawer ruler_drawer;
        private IGraphProducer graph_producer;
        private IControlPanel control_panel;
        private ConcurrentQueue<T> buffer;

        private ConstructorTuple<ICanvas<T>> canvas_constructor;
        private ConstructorTuple<IPointDrawer> point_drawer_constructor;

        private readonly ConcurrentQueue<ICanvas<T>> free_canvas;
        private readonly ConcurrentQueue<IPointDrawer> free_point_drawer;

        public MultiThreadOscilloscope(
            ConstructorTuple<ICanvas<T>> canvas_constructor,
            ConstructorTuple<IPointDrawer> point_drawer_constructor,
            IRulerDrawer ruler_drawer,
            IGraphProducer graph_producer,
            IControlPanel control_panel,
            ConcurrentQueue<T> buffer = null)
        {
            ICanvas<T> canvas = canvas_constructor.NewInstance();
            IPointDrawer point_drawer = point_drawer_constructor.NewInstance();
            if (canvas.GraphSize != point_drawer.GraphSize || canvas.GraphSize != ruler_drawer.GraphSize)
            {
                throw new OscillocopeBuildException("canvas, point_drawer, ruler_drawer have different GraphSize");
            }
            this.canvas_constructor = canvas_constructor;
            this.point_drawer_constructor = point_drawer_constructor;
            this.ruler_drawer = ruler_drawer;
            this.graph_producer = graph_producer;
            this.control_panel = control_panel;
            if (buffer == null)
            {
                this.buffer = new ConcurrentQueue<T>();
            }
            else
            {
                this.buffer = buffer;
            }
            this.free_canvas.Enqueue(canvas);
            this.free_point_drawer.Enqueue(point_drawer);
        }

        protected void Draw(double delta_time)
        {
            ICanvas<T> canvas;
            if (!free_canvas.TryDequeue(out canvas))
            {
                canvas = canvas_constructor.NewInstance();
            }
            IPointDrawer point_drawer;
            if (!free_point_drawer.TryDequeue(out point_drawer))
            {
                point_drawer = point_drawer_constructor.NewInstance();
            }
            T new_graph = graph_producer.Produce(
                delta_time: delta_time,
                canvas: canvas,
                point_drawer: point_drawer,
                ruler_drawer: ruler_drawer,
                information: control_panel.GetStateShot());
            buffer.Enqueue(new_graph);
            free_canvas.Enqueue(canvas);
            free_point_drawer.Enqueue(point_drawer);
        }
    }

    public class UndrivedOscilloscope<T> : MultiThreadOscilloscope<T>
    {
        public UndrivedOscilloscope(
            ConstructorTuple<ICanvas<T>> canvas_constructor,
            ConstructorTuple<IPointDrawer> point_drawer_constructor,
            IRulerDrawer ruler_drawer,
            IGraphProducer graph_producer,
            IControlPanel control_panel,
            ConcurrentQueue<T> buffer = null)
            : base(
                  canvas_constructor: canvas_constructor,
                  point_drawer_constructor: point_drawer_constructor,
                  ruler_drawer: ruler_drawer, 
                  graph_producer: graph_producer,
                  control_panel: control_panel,
                  buffer: buffer)
        { }

        public new void Draw(double delta_time)
        {
            base.Draw(delta_time);
        }
    }

    public class DrivedOscilloscope<T> : MultiThreadOscilloscope<T>
    {
        private Timer timer = null;

        public DrivedOscilloscope(
            ConstructorTuple<ICanvas<T>> canvas_constructor,
            ConstructorTuple<IPointDrawer> point_drawer_constructor,
            IRulerDrawer ruler_drawer,
            IGraphProducer graph_producer,
            IControlPanel control_panel,
            ConcurrentQueue<T> buffer = null)
            : base(
                  canvas_constructor: canvas_constructor,
                  point_drawer_constructor: point_drawer_constructor,
                  ruler_drawer: ruler_drawer,
                  graph_producer: graph_producer,
                  control_panel: control_panel,
                  buffer: buffer)
        { }

        public void Start(int delta_time)
        {
            int timer_delta_time = delta_time * 1000 / Waves.UNIT_NUMBER_PRO_SECOND;
            delta_time = timer_delta_time * Waves.UNIT_NUMBER_PRO_SECOND / 1000;
            timer = new Timer(o => base.Draw(delta_time), null, 500, timer_delta_time);
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
