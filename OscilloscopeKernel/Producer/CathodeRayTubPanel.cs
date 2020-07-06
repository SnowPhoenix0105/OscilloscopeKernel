using OscilloscopeKernel.Tools;
using OscilloscopeKernel.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OscilloscopeKernel.Producer
{
    public class CathodeRayTubPanel : IControlPanel
    {
        public double AccelVoltage => accel_voltage;

        public double WheelLength => wheel_length;

        public double WheelWidth => wheel_width;

        public double SlideLength => slide_length;

        public WaveFixer XWave => x_fixer;

        public WaveFixer YWave => y_fixer;

        public int OffsetX
        {
            get => offset_x;
            set => offset_x = value;
        }

        public int OffsetY
        {
            get => offset_y;
            set => offset_y = value;
        }

        public int PointLength
        {
            get => point_length;
            set => point_length = value;
        }

        public int PointWidth
        {
            get => point_width;
            set => point_width = value;
        }

        public bool IsAC
        {
            get => ac_mode;
            set => ac_mode = value;
        }

        private double accel_voltage;
        private double wheel_length;
        private double wheel_width;
        private double slide_length;
        private WaveFixer x_fixer = new WaveFixer();
        private WaveFixer y_fixer = new WaveFixer();
        private int offset_x;
        private int offset_y;
        private int point_length;
        private int point_width;
        private bool ac_mode;

        public CathodeRayTubPanel(
            double accel_voltage = 30,
            double wheel_length = 400,
            double wheel_width = 4,
            double slide_length = 4,
            int offset_x = 0,
            int offset_y = 0,
            int point_length = 0,
            int point_width = 0,
            bool ac_mode = false)
        {
            this.accel_voltage = accel_voltage;
            this.wheel_length = wheel_length;
            this.wheel_width = wheel_width;
            this.slide_length = slide_length;
            this.offset_x = offset_x;
            this.offset_y = offset_y;
            this.point_length = point_length;
            this.point_width = point_width;
            this.ac_mode = ac_mode;
        }

        public IControlInformation GetStateShot()
        {
            return new Information(this);
        }

        private class Information : IControlInformation
        {
            public object this[object key] => throw new NotImplementedException();

            ref SizeStruct IControlInformation.PointSize => ref point_size;

            public double XPeriod => x_wave.Period;

            public double YPeriod => y_wave.Period;

            private double accel_votage;
            private double wheel_length;
            private double wheel_width;
            private double slide_length;
            private IWave x_wave;
            private IWave y_wave;
            private PositionStruct offset;
            private SizeStruct point_size;
            private bool ac_mode;

            public Information(CathodeRayTubPanel origin)
            {
                this.accel_votage = origin.accel_voltage;
                this.wheel_length = origin.wheel_length;
                this.wheel_width = origin.wheel_width;
                this.slide_length = origin.slide_length;
                this.x_wave = origin.x_fixer.GetStateShot();
                this.y_wave = origin.y_fixer.GetStateShot();
                this.offset = new PositionStruct(origin.offset_x, origin.offset_y);
                this.point_size = new SizeStruct(origin.point_length, origin.point_width);
                this.ac_mode = origin.ac_mode;
            }

            public void Position(double x_phase, double y_phase, out PositionStruct position)
            {
                double x_voltage = x_wave.Voltage(x_phase);
                double y_voltage = y_wave.Voltage(y_phase);
                if (ac_mode)
                {
                    x_voltage -= x_wave.MeanVoltage;
                    y_voltage -= y_wave.MeanVoltage;
                }
                int x = offset.X + Calculate(x_voltage);
                int y = offset.Y + Calculate(y_voltage);
                position = new PositionStruct(x, y);
            }

            private int Calculate(double wheel_votage)
            {
                double up = wheel_votage
                    * this.wheel_length
                    * (this.wheel_length + this.slide_length);
                double down = 4
                    * this.accel_votage
                    * this.wheel_width;
                double point = up / down;
                return (int)point;
            }
        }
    }
}
