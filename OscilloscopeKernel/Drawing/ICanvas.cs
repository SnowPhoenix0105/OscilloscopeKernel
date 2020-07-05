using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text;
using OscilloscopeKernel.Tools;

namespace OscilloscopeKernel.Drawing
{ 
    public interface ICanvas<out T>
    {
        int Length { get; }
        int Width { get; }

        /// <summary>
        /// get or set a pixel with index
        /// </summary>
        /// <param name="x">index x</param>
        /// <param name="y">index y</param>
        /// <returns></returns>
        ColorTuple this[int x, int y] { get; set; }

        /// <summary>
        /// output the graph directly and reset the canvas
        /// </summary>
        /// <returns></returns>
        T Output();
    }
}
