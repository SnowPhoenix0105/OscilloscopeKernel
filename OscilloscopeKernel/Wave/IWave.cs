using System;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeCore.Wave
{
    public interface IWave
    {
        double MeanVoltage { get; }

        int Period { get; }

        double Voltage(int time);

        public static IWave operator +(IWave left, IWave right)
        {
            return new AddWave(left, right);
        }
    }

    public static class Waves
    {
        public static readonly ConstantWave NONE = new ConstantWave(0);

        public static readonly int UNIT_NUMBER_PRO_SECOND = 1000_000;

        public static double GetFrequence(IWave wave)
        {
            return 1.0 / (double)(wave.Period);
        }

        public static double CalculateMeanVoltage(IWave wave, int calculate_times = 1000)
        {
            WaveRunner runner = new WaveRunner(wave);
            double delta_time = (double)(wave.Period) / calculate_times;
            double little_sum = 0;
            double mean = 0;
            for (int i = 0; i < calculate_times; i++)
            {
                little_sum += runner.Voltage;
                runner.TimeAhead(delta_time);
                if ((i & 0xf) == 0)
                {
                    mean += little_sum / calculate_times;
                    little_sum = 0;
                }
            }
            mean += little_sum / calculate_times;
            return mean;
        }
    }

    public class ConstantWave : IWave
    {
        public double MeanVoltage => value;

        public int Period => 1;

        private double value;

        public ConstantWave(double voltage)
        {
            this.value = voltage;
        }

        public double Voltage(int time) => value;

        public override string ToString()
        {
            return string.Format("ConstantWave({0})", value);
        }

        public override bool Equals(object obj)
        {
            if (obj is ConstantWave)
            {
                return this.value == ((ConstantWave)obj).value;
            }
            return false;
        }
    }

    class AddWave : IWave
    {
        public double MeanVoltage => mean_voltage;

        public int Period => period;

        private IWave wave1;
        private IWave wave2;
        private int period;
        private double mean_voltage;

        public AddWave(IWave wave1, IWave wave2)
        {
            this.wave1 = wave1;
            this.wave2 = wave2;
            this.mean_voltage = wave1.MeanVoltage + wave2.MeanVoltage;
            this.period = GetLeastCommonMultiple(wave1.Period, wave2.Period);
        }

        public double Voltage(int time)
        {
            return wave1.Voltage(time) + wave2.Voltage(time);
        }

        private static int GetLeastCommonMultiple(int m, int n)
        {
            int a = m;
            int b = n;
            while (b != 0)
            {
                int c = a % b;
                a = b;
                b = c;
            }
            return (m/a) * n;
        }
    }
}
