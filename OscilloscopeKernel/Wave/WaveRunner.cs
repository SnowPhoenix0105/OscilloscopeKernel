using System;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeCore.Wave
{
    public class WaveRunner
    {
        public double VoltageTimes
        {
            get => voltage_times;
            set => voltage_times = value;
        }

        public double PeriodTimes
        {
            get => period_times;
            set
            {
                period_times = value;
                output_period = value * wave_period;
            }
        }

        public double FrequenceTimes
        {
            get => 1.0 / period_times;
            set => period_times = 1.0 / value;
        }

        public int Time
        {
            get => (int)(phase * wave_period * period_times);
            set => phase = ((double)value / output_period) % 1.0;
        }

        public double Voltage => voltage_times * wave.Voltage((int)(phase * wave_period));

        public IWave Wave
        {
            get => wave;
            set
            {
                wave = value;
                wave_period = value.Period;
                output_period = period_times * value.Period;
            }
        }

        private double voltage_times = 1;
        private double period_times = 1;
        private double phase = 0;
        private IWave wave;
        private double output_period;
        private int wave_period;

        public WaveRunner()
        {
            this.wave = Waves.NONE;
            wave_period = 1;
            output_period = 1;
        }

        public WaveRunner(IWave wave)
        {
            this.wave = wave;
            wave_period = wave.Period;
            output_period = wave.Period;
        }

        public void TimeAhead(double delta_time)
        {
            phase += delta_time / output_period;
            phase = phase - (int)phase;         // faster than "phase %= 1"
        }
    }
}
