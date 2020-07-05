using OscilloscopeKernel.Drawing;
using OscilloscopeKernel.Tools;
using OscilloscopeKernel.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeKernel.Producer
{
    public abstract class SimpleProducer : IGraphProducer
    {
        private double saved_x_phase = 0;
        private double saved_y_phase = 0;
        private int calculate_times;
        private readonly object locker = new Object();
        private readonly ColorStruct graph_color;

        public SimpleProducer(int calculate_times, ColorStruct graph_color)
        {
            this.calculate_times = calculate_times;
            this.graph_color = graph_color;
        }

        public T Produce<T>(double delta_time, ICanvas<T> canvas, IPointDrawer point_drawer, IRulerDrawer ruler_drawer, IControlInformation information)
        {
            double x_delta_phase = delta_time / information.XPeriod;
            double y_delta_phase = delta_time / information.YPeriod;
            double old_x_phase;
            double old_y_phase;
            lock(locker)
            {
                old_x_phase = saved_x_phase;
                old_y_phase = saved_y_phase;
                saved_x_phase += x_delta_phase;
                saved_x_phase -= (int)saved_x_phase;
                saved_y_phase += y_delta_phase;
                saved_y_phase -= (int)saved_y_phase;
            }
            double x_phase_step = x_delta_phase / calculate_times;
            double y_phase_step = y_delta_phase / calculate_times;
            
            for (int i = 0; i < calculate_times; i++)
            {
                double x_phase = old_x_phase + i * x_phase_step;
                double y_phase = old_y_phase + i * y_phase_step;
                if (x_phase > 1)
                {
                    x_phase -= (int)x_phase;
                }
                if (y_phase > 1)
                {
                    y_phase -= (int)y_phase;
                }
                information.Position(x_phase, y_phase, out PositionStruct position);
                point_drawer.SetPoint(position);
            }

            point_drawer.DrawAllPoint(canvas, graph_color, information.PointSize);
            ruler_drawer.Draw(canvas);
            return canvas.Output();
        }
    }
}
