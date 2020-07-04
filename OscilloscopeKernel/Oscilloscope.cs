using OscilloscopeCore.Drawing;
using System;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeCore
{
    public class Oscilloscope<T>
    {
        private ICanvas<T> canvas;
        private IPointDrawer point_drawer;
        private IRulerDrawer ruler_drawer;
        private IGraphProducer graph_calculator;

    }
}
