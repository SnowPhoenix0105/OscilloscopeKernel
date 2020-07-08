using OscilloscopeKernel.Drawing;
using OscilloscopeKernel.Exceptions;
using OscilloscopeKernel.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OscilloscopeKernel.Producer
{
    public class TotalParallelProducer : IGraphProducer
    {
        public bool RequireConcurrentDrawer => true;

        private double saved_x_phase = 0;
        private double saved_y_phase = 0;
        private int calculate_times;
        private readonly object locker = new Object();
        private readonly Color graph_color;

        public TotalParallelProducer(int calculate_times, Color graph_color)
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

            Parallel.For(0, calculate_times, i =>
            {
                double x_phase = old_x_phase + i * x_phase_step;
                double y_phase = old_y_phase + i * y_phase_step;
                x_phase -= (int)x_phase;
                y_phase -= (int)y_phase;
                information.Position(x_phase, y_phase, out PositionStruct position);
                point_drawer.SetPoint(position);
            });

            point_drawer.DrawAllPoint(canvas, graph_color, information.PointSize);
        }
    }

    public class PartlyParallelProducer : IGraphProducer
    {
        public bool RequireConcurrentDrawer => true;

        private double saved_x_phase = 0;
        private double saved_y_phase = 0;
        private int calculate_unit_number;
        private int calculate_unit_times;
        private readonly object locker = new Object();
        private readonly Color graph_color;

        public PartlyParallelProducer(int calculate_unit_number, int calculate_unit_times, Color graph_color)
        {
            this.calculate_unit_number = calculate_unit_number;
            this.graph_color = graph_color;
            this.calculate_unit_times = calculate_unit_times;
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
                saved_x_phase -= (int)saved_x_phase;
                saved_y_phase += y_delta_phase;
                saved_y_phase -= (int)saved_y_phase;
            }
            double x_phase_step = x_delta_phase / calculate_unit_number;
            double y_phase_step = y_delta_phase / calculate_unit_number;

            Parallel.For(0, calculate_unit_number, i =>
            {
                int start_count = i * calculate_unit_times;
                for (int j = start_count; j < start_count + calculate_unit_times; j++)
                {
                    double x_phase = old_x_phase + j * x_phase_step;
                    double y_phase = old_y_phase + j * y_phase_step;
                    x_phase -= (int)x_phase;
                    y_phase -= (int)y_phase;
                    information.Position(x_phase, y_phase, out PositionStruct position);
                    point_drawer.SetPoint(position);
                }
            });

            point_drawer.DrawAllPoint(canvas, graph_color, information.PointSize);
        }
    }

    public class GhostParallelProducer : IGraphProducer
    {
        public bool RequireConcurrentDrawer => true;

        private double saved_x_phase = 0;
        private double saved_y_phase = 0;
        private int calculate_times;
        private int ghost_number;
        private PositionStruct[] ghosts;
        private readonly object locker = new Object();
        private readonly Color graph_color;
        private readonly Color ghost_color;

        public GhostParallelProducer(int calculate_times, int ghost_number, Color graph_color)
        {
            if (ghost_number > calculate_times)
            {
                throw new NegativeGhostTimeException();
            }
            this.calculate_times = calculate_times;
            this.ghost_number = ghost_number;
            this.ghosts = new PositionStruct[ghost_number];
            this.graph_color = graph_color;
            this.ghost_color = Color.FromArgb(graph_color.A, graph_color.R >> 1, graph_color.G >> 1, graph_color.B >> 1);
        }

        public async void Produce<T>(double delta_time, ICanvas<T> canvas, IPointDrawer point_drawer, IControlInformation information)
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

            for (int i = 0; i < ghost_number; i++)
            {
                point_drawer.SetPoint(ghosts[i]);
            }
            point_drawer.DrawAllPoint(canvas, ghost_color, information.PointSize);

            Task ghost_fresh = new Task(() =>
            {
                int base_count = calculate_times - ghost_number;
                for (int i = 0; i < ghost_number; i++)
                {
                    int total_count = i + base_count;
                    double x_phase = old_x_phase + total_count * x_phase_step;
                    double y_phase = old_y_phase + total_count * y_phase_step;
                    x_phase -= (int)x_phase;
                    y_phase -= (int)y_phase;
                    information.Position(x_phase, y_phase, out ghosts[i]);
                    point_drawer.SetPoint(ghosts[i]);
                }
            });
            ghost_fresh.Start();

            Parallel.For(0, calculate_times, i =>
            {
                double x_phase = old_x_phase + i * x_phase_step;
                double y_phase = old_y_phase + i * y_phase_step;
                x_phase -= (int)x_phase;
                y_phase -= (int)y_phase;
                information.Position(x_phase, y_phase, out PositionStruct position);
                point_drawer.SetPoint(position);
            });

            await ghost_fresh;

            point_drawer.DrawAllPoint(canvas, graph_color, information.PointSize);
        }
    }
}
