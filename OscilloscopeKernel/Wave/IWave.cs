using System;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeKernel.Wave
{
    public interface IWave
    {
        double MeanVoltage { get; }

        int Period { get; }

        double Voltage(double phase);
    }
}
