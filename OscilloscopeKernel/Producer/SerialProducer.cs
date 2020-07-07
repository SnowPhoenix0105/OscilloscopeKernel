using OscilloscopeKernel.Drawing;
using OscilloscopeKernel.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace OscilloscopeKernel.Producer
{
    public class SerialProducer : IGraphProducer
    {
        public bool RequireMultiThreadDrawer => false;

        private double saved_x_phase = 0;
        private double saved_y_phase = 0;
        private int calculate_times;
        private readonly object locker = new Object();
        private readonly Color graph_color;

        public SerialProducer(int calculate_times, Color graph_color)
        {
            this.calculate_times = calculate_times;
            this.graph_color = graph_color;
        }

        public void Produce<T>(double delta_time, ICanvas<T> canvas, IPointDrawer point_drawer, IControlInformation information)
        {
            double x_delta_phase = delta_time / information.XPeriod;
            double y_delta_phase = delta_time / information.YPeriod;
            double old_x_phase;
            double old_y_phase;
            lock (locker)
            {
                old_x_phase = saved_x_phase;
                old_y_phase = saved_y_phase;
                saved_x_phase += x_delta_phase;
                saved_y_phase += y_delta_phase;
                saved_x_phase -= (int)saved_x_phase;
                saved_y_phase -= (int)saved_y_phase;
            }
            double x_phase_step = x_delta_phase / calculate_times;
            double y_phase_step = y_delta_phase / calculate_times;
            x_phase_step -= (int)x_phase_step;
            y_phase_step -= (int)y_phase_step;
            double x_phase = old_x_phase;
            double y_phase = old_x_phase;

            for (int i = 0; i < calculate_times; i++)
            {
                x_phase += x_phase_step;
                y_phase += y_phase_step;
                if (x_phase >= 1) x_phase -= 1;
                if (y_phase >= 1) y_phase -= 1;
                information.Position(x_phase, y_phase, out PositionStruct position);
                point_drawer.SetPoint(position);
            }

            point_drawer.DrawAllPoint(canvas, graph_color, information.PointSize);
        }
    }
}
