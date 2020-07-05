using OscilloscopeKernel.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeKernel.Drawing
{
    public interface IPointDrawer
    {
        int GraphLength { get; }
        int GraphWidth { get; }
        int PointLength { get; set; }
        int PointWidth { get; set; }
        void SetPoint(int x, int y);
        void DrawAllPoint<T>(ICanvas<T> canvas, ColorTuple color);
    }

    public interface IRulerDrawer
    {
        int GraphLength { get; }
        int GraphWidth { get; }
        void Draw<T>(ICanvas<T> canvas);
    }
}
