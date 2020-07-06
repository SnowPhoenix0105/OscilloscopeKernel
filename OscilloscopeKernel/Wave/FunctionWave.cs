using System;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeKernel.Wave
{
    public class FunctionWave : AdditiveWave
    {
        public delegate double WaveFunction(double phase);

        public override double MeanVoltage => mean_voltage;

        public override int Period => period;

        protected double VoltageTimes => voltage_times;

        private int period;
        private double mean_voltage;
        private WaveFunction function;
        private double voltage_times;

        public FunctionWave(WaveFunction function, int period, double voltage_times = 1)
        {
            this.function = function;
            this.period = period;
            this.voltage_times = voltage_times;
            this.mean_voltage = Waves.CalculateMeanVoltage(this);
        }

        public FunctionWave(WaveFunction function, int period, double voltage_times, double function_mean)
        {
            this.function = function;
            this.period = period;
            this.voltage_times = voltage_times;
            this.mean_voltage = voltage_times * function_mean;
        }

        public override double Voltage(double phase)
        {
            return voltage_times * function(phase);
        }

        public override string ToString()
        {
            return string.Format("FunctionWave(period={0}, mean={1})", period, mean_voltage);
        }
    }

    public class SinWave : FunctionWave
    {
        public SinWave(int period, double max_voltage)
            : base(phase => Math.Sin(2 * Math.PI * phase), period, max_voltage, 0) { }

        public override string ToString()
        {
            return string.Format("SinWave(period={0}, max={1})", Period, VoltageTimes);
        }
    }

    public class SawToothWave : FunctionWave
    {
        public SawToothWave(int period, double max_voltage)
            : base(phase => 2 * phase - 1, period, max_voltage, 0) { }

        public override string ToString()
        {
            return string.Format("SawToothWave(period={0}, max={1})", Period, VoltageTimes);
        }
    }

    public class SquareWave : FunctionWave
    {
        public SquareWave(int period, double max_voltage)
            : base(phase => phase < 0.5 ? -1 : 1, period, max_voltage, 0) { }

        public override string ToString()
        {
            return string.Format("SquareWave(period={0}, max={1})", Period, VoltageTimes);
        }
    }
}
