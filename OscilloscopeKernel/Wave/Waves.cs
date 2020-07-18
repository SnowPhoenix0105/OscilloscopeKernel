using System;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeKernel.Wave
{


    public static class Waves
    {
        public static readonly ConstantWave NONE = new ConstantWave(0);

        public static readonly int UNIT_NUMBER_PRO_SECOND = 1000_000;

        public static double GetFrequence(IWave wave)
        {
            return UNIT_NUMBER_PRO_SECOND / (double)(wave.Period);
        }

        public static double CalculateMeanVoltage(IWave wave, int calculate_times = 1000)
        {
            double delta_phase = 1.0 / (double)calculate_times;
            double little_sum = 0;
            double mean = 0;
            double phase = 0;
            for (int i = 0; i < calculate_times; i++)
            {
                little_sum += wave.Voltage(phase);
                phase += delta_phase;
                if ((i & 0xf) == 0)
                {
                    mean += little_sum / calculate_times;
                    little_sum = 0;
                }
            }
            mean += little_sum / calculate_times;
            return mean;
        }

        public static AbstractWave Add(IWave left, IWave right)
        {
            return new AddWave(left, right);
        }

        public static AbstractWave Negative(IWave origin)
        {
            return new NegativeWave(origin);
        }

        public static AbstractWave Reverse(IWave origin)
        {
            return new WaveReverser(origin);
        }

        private static WaveDecorator Decorate(IWave origin)
        {
            return new WaveDecorator(origin);
        }
    }

    public class WaveDecorator : AbstractWave
    {
        public override double MeanVoltage => origin.MeanVoltage;

        public override int Period => origin.Period;

        public IWave Origin => origin;

        private IWave origin;

        internal WaveDecorator(IWave origin)
        {
            this.origin = origin;
        }

        public override double Voltage(double phase)
        {
            return origin.Voltage(phase);
        }
    }

    class NegativeWave : AbstractWave
    {
        public override double MeanVoltage => -origin.MeanVoltage;

        public override int Period => origin.Period;

        private IWave origin;

        public NegativeWave(IWave origin)
        {
            this.origin = origin;
        }

        public override double Voltage(double phase)
        {
            return -origin.Voltage(phase);
        }
    }

    class AddWave : AbstractWave
    {
        public override double MeanVoltage => mean_voltage;

        public override int Period => period;

        private IWave wave1;
        private IWave wave2;
        private int period;
        private int period1;
        private int period2;
        private double mean_voltage;

        public AddWave(IWave wave1, IWave wave2)
        {
            this.wave1 = wave1;
            this.wave2 = wave2;
            this.period1 = wave1.Period;
            this.period2 = wave2.Period;
            this.mean_voltage = wave1.MeanVoltage + wave2.MeanVoltage;
            this.period = GetLeastCommonMultiple(wave1.Period, wave2.Period);
        }

        public override double Voltage(double phase)
        {
            double time = phase * period;
            double phase1 = time / period1;
            double phase2 = time / period2;
            phase1 -= (int)phase1;
            phase2 -= (int)phase2;
            return wave1.Voltage(phase1) + wave2.Voltage(phase2);
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
            return (m / a) * n;
        }
    }

    class WaveReverser : AbstractWave
    {
        public override double MeanVoltage => origin.MeanVoltage;

        public override int Period => origin.Period;

        private IWave origin;

        public WaveReverser(IWave origin)
        {
            this.origin = origin;
        }

        public override double Voltage(double phase)
        {
            return origin.Voltage(1 - phase);
        }
    }
}
