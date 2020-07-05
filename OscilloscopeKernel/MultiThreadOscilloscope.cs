using OscilloscopeKernel.Drawing;
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
        private ConcurrentQueue<T> buffer;

        private ConstructorTuple<ICanvas<T>> canvas_constructor;
        private ConstructorTuple<IPointDrawer> point_drawer_constructor;
        private ConstructorTuple<IGraphProducer> graph_producer_constructor;

        private readonly Dictionary<ICanvas<T>, bool> canvas_using = new Dictionary<ICanvas<T>, bool>();
        private readonly Dictionary<IPointDrawer, bool> point_drawer_using = new Dictionary<IPointDrawer, bool>();
        private readonly Dictionary<IGraphProducer, bool> graph_producer_using = new Dictionary<IGraphProducer, bool>();

        public MultiThreadOscilloscope(
            ConstructorTuple<ICanvas<T>> canvas_constructor,
            ConstructorTuple<IPointDrawer> point_drawer_constructor,
            IRulerDrawer ruler_drawer,
            ConstructorTuple<IGraphProducer> graph_producer_constructor,
            ConcurrentQueue<T> buffer = null,
            int buffer_max = 20)
        {
            this.canvas_constructor = canvas_constructor;
            this.point_drawer_constructor = point_drawer_constructor;
            this.ruler_drawer = ruler_drawer;
            this.graph_producer_constructor = graph_producer_constructor;
            if (buffer == null)
            {
                this.buffer = new ConcurrentQueue<T>();
            }
            else
            {
                this.buffer = buffer;
            }
        }

        private U GetFreeInstance<U>(ConstructorTuple<U> constructor, Dictionary<U, bool> u_using)
        {
            foreach(KeyValuePair<U, bool> entry in u_using)
            {
                if (entry.Value == false)
                {
                    return entry.Key;
                }
            }
            U new_instance = constructor.NewInstance();
            u_using.Add(new_instance, false);
            return new_instance;
        }

        protected void Draw(double delta_time)
        {
            ICanvas<T> canvas;
            lock (canvas_using)
            {
                canvas = GetFreeInstance(canvas_constructor, canvas_using);
                canvas_using[canvas] = false;
            }
            IPointDrawer point_drawer;
            lock (point_drawer_using)
            {
                point_drawer = GetFreeInstance(point_drawer_constructor, point_drawer_using);
                point_drawer_using[point_drawer] = false;
            }
            IGraphProducer graph_producer;
            lock (graph_producer_using)
            {
                graph_producer = GetFreeInstance(graph_producer_constructor, graph_producer_using);
                graph_producer_using[graph_producer] = false;
            }
            T new_graph = graph_producer.Produce(
                delta_time: delta_time,
                canvas: canvas,
                point_drawer: point_drawer,
                ruler_drawer: ruler_drawer,
                x_wave: XFixer.GetCut(),
                y_wave: YFixer.GetCut());
            buffer.Enqueue(new_graph);
        }
    }

    public class UndrivedOscilloscope<T> : MultiThreadOscilloscope<T>
    {
        public UndrivedOscilloscope(
            ConstructorTuple<ICanvas<T>> canvas_constructor,
            ConstructorTuple<IPointDrawer> point_drawer_constructor,
            IRulerDrawer ruler_drawer,
            ConstructorTuple<IGraphProducer> graph_producer_constructor,
            ConcurrentQueue<T> buffer = null,
            int buffer_max = 20)
            : base(canvas_constructor, 
                  point_drawer_constructor, 
                  ruler_drawer, 
                  graph_producer_constructor,
                  buffer,
                  buffer_max)
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
            ConstructorTuple<IGraphProducer> graph_producer_constructor,
            ConcurrentQueue<T> buffer = null,
            int buffer_max = 20)
            : base(canvas_constructor, 
                  point_drawer_constructor, 
                  ruler_drawer, graph_producer_constructor, 
                  buffer,
                  buffer_max)
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
