using System;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeKernel.Wave
{
    public abstract class AbstractWave : IWave
    {
        public abstract double MeanVoltage { get; }

        public abstract int Period { get; }

        public abstract double Voltage(double phase);

        public AbstractWave Reverse()
        {
            return new WaveReverser(this);
        }

        public static AbstractWave operator -(AbstractWave origin)
        {
            return new NegativeWave(origin);
        }

        public static AbstractWave operator +(AbstractWave left, IWave right)
        {
            return new AddWave(left, right);
        }

        public static AbstractWave operator +(IWave left, AbstractWave right)
        {
            return new AddWave(left, right);
        }
    }
}
