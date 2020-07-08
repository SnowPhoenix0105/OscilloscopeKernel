using OscilloscopeKernel.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace OscilloscopeKernel.Drawing
{
    public interface IPointDrawer
    {
        bool IsConcurrent { get; }
        ref SizeStruct GraphSize { get; }
        void SetPoint(in PositionStruct position);
        void DrawAllPoint<T>(ICanvas<T> canvas, Color color, in SizeStruct point_size);
    }

    public interface IBackgroundDrawer
    {
        ref SizeStruct GraphSize { get; }
        void Draw<T>(ICanvas<T> canvas);
    }
}
