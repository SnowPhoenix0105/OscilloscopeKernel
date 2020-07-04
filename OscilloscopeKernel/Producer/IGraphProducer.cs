using OscilloscopeCore.Drawing;
using OscilloscopeCore.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeCore.Producer
{
    public interface IGraphProducer
    {
        bool RequireThreadSafeDrawer { get; }
        int GraphLength { get; }
        int GraphWidth { get; }
        T Produce<T>(
            double delta_time, 
            ICanvas<T> canvas, 
            IPointDrawer point_drawer, 
            IRulerDrawer ruler_drawer, 
            IWave x_wave, 
            IWave y_wave);
    }
}
