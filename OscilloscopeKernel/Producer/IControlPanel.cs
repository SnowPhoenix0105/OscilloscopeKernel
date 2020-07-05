using OscilloscopeKernel.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeKernel.Producer
{
    public interface IControlInformation
    {
        double XPeriod { get; }
        double YPeriod { get; }
        ref SizeStruct PointSize { get; }
        object this[object key] { get; }
        void Position(double x_phase, double y_phase, out PositionStruct position);
    }

    public interface IControlPanel
    {
        IControlInformation GetStateShot();
    }
}
