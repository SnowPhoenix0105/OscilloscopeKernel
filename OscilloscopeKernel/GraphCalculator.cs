using OscilloscopeCore.Drawing;
using System;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeCore
{
    class GraphCalculator<T>
    {


        public Queue<T> Buffer { get { return this.buffer; } }    //输出缓冲
        private Drawer<T> Drawer { get; }   //存储每一帧数据的库
        public Setting Setting { get; }        //设置
        private Queue<T> buffer;
        private double _sweep_phase;
        private double _input_phase;



        public GraphCalculator(Setting new_setting, Queue<T> new_buffer, ICanvas<T> canvas, int graph_length = 360, int graph_width = 360)
        {
            this.Setting = new_setting;
            this.buffer = new_buffer;
            this.Drawer = new Drawer<T>(graph_length, graph_width, canvas);
        }

        public GraphCalculator(Setting new_setting, ICanvas<T> canvas, int graph_length = 360, int graph_width = 360)
        {
            this.Setting = new_setting;
            this.buffer = new Queue<T>();
            this.Drawer = new Drawer<T>(graph_length, graph_width, canvas);
        }

        public void FreshInit()
        {
            this._sweep_phase = this.Setting.Sweep.InitPhase;
            this._input_phase = this.Setting.Input.InitPhase;
        }

        public void DrawGraph(double delta_time)
        {
            double sweep_step = this.Setting.SamplePeriod / this.Setting.Sweep.Period;
            double input_step = this.Setting.SamplePeriod / this.Setting.Input.Period;
            double deltaTime = delta_time * 1000;

            for (int i = 0; i < deltaTime / this.Setting.SamplePeriod; i++)
            {
                this._sweep_phase += sweep_step;
                this._input_phase += input_step;
                this._sweep_phase %= 1;
                this._input_phase %= 1;

                double sweep_votage = this.Setting.Offset_x + this.Setting.Sweep.MaxVotage * this.Setting.Sweep.Function(this._sweep_phase);
                double input_votage = this.Setting.Offset_y + this.Setting.Input.MaxVotage * this.Setting.Input.Function(this._input_phase);

                if (Setting.ACMode)
                {
                    sweep_votage -= Setting.Sweep.AverageVotage;
                    input_votage -= Setting.Input.AverageVotage;
                }

                int x = (int)this.Calculate(sweep_votage);
                int y = (int)this.Calculate(input_votage);

                this.Drawer.SetPoint(x, y);
            }

            this.Drawer.DrawAll(this.Setting.PointHeight, this.Setting.PointWidth, this.Setting.Brightness);
            this.Drawer.DrawRuler();
            this.Drawer.Output(this.Buffer);
        }

        public T DrawEmptyGraph()
        {
            this.Drawer.Reset();
            this.Drawer.DrawRuler();
            return this.Drawer.OutputColors();
        }

        private double Calculate(double wheel_votage)
        {

            double up = wheel_votage
                * this.Setting.WheelLenth
                * (this.Setting.WheelLenth + this.Setting.SlideLenth);
            double down = 4
                * this.Setting.AccelVotage
                * this.Setting.WheelWidth;
            double point = up / down;

            return point;
        }
    }
}
