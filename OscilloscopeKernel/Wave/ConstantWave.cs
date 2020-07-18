using System;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeKernel.Wave
{
    public class ConstantWave : AbstractWave
    {
        public override double MeanVoltage => value;

        public override int Period => 1;

        private double value;

        public ConstantWave(double voltage)
        {
            this.value = voltage;
        }

        public override double Voltage(double phase) => value;

        public override string ToString()
        {
            return string.Format("ConstantWave({0})", value);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (this.GetType() == obj.GetType())
            {
                return this.value == ((ConstantWave)obj).value;
            }
            return false;
        }
    }
}
