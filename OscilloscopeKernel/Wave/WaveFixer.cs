﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeKernel.Wave
{
    public class WaveFixer
    {
        public double VoltageTimes
        {
            get => voltage_times;
            set => voltage_times = value;
        }

        public double PeriodTimes
        {
            get => period_times;
            set => period_times = value;
        }

        public IWave Wave
        {
            get => wave;
            set => wave = value;
        }

        private double voltage_times = 1;
        private double period_times = 1;
        private IWave wave;

        public WaveFixer()
        {
            this.wave = Waves.NONE;
        }

        public WaveFixer(IWave wave)
        {
            this.wave = wave;
        }

        public IWave GetCut()
        {
            return new Cut(this);
        }

        private class Cut : IWave
        {
            public double MeanVoltage => voltage_times * wave.MeanVoltage;

            public int Period => total_period;

            private double voltage_times;
            private int total_period;
            private IWave wave;

            public Cut(WaveFixer fixer)
            {
                this.voltage_times = fixer.voltage_times;
                this.total_period = (int)(fixer.period_times * fixer.wave.Period);
                this.wave = fixer.wave;
            }

            public double Voltage(double phase)
            {
                return voltage_times * wave.Voltage(phase);
            }
        }
    }
}