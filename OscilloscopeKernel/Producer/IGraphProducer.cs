using OscilloscopeKernel.Drawing;
using OscilloscopeKernel.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeKernel.Producer
{
    public interface IGraphProducer
    {
        bool RequireMultiThreadDrawer { get; }

        void Produce<T>(
            double delta_time, 
            ICanvas<T> canvas, 
            IPointDrawer point_drawer, 
            IControlInformation information);
    }
}
