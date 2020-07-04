using OscilloscopeCore.Drawing;
using OscilloscopeCore.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeCore
{
    public interface IGraphProducer
    {
        int GraphLength { get; }
        int GraphWidth { get; }
        void Produce<T>(ICanvas<T> canvas, IPointDrawer point_drawer, IRulerDrawer ruler_drawer, WaveRunner x_runner, WaveRunner y_runner);
    }
}
