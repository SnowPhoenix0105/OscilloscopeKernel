using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text;
using System.Drawing;
using OscilloscopeKernel.Tools;

namespace OscilloscopeKernel.Drawing
{ 
    public interface ICanvas<out T>
    {
        ref SizeStruct GraphSize { get; }

        bool IsReady { get; }

        /// <summary>
        /// get or set a pixel with index
        /// </summary>
        /// <param name="x">index x</param>
        /// <param name="y">index y</param>
        /// <returns></returns>
        Color this[int x, int y] { get; set; }

        /// <summary>
        /// output the graph directly and reset the canvas
        /// </summary>
        /// <returns></returns>
        T Output();
    }

    public abstract class Canvas<T> : ICanvas<T>
    {
        public abstract Color this[int x, int y] { get; set; }

        public ref SizeStruct GraphSize => ref graph_size;

        public abstract bool IsReady { get; }

        protected int supX => max_x;
        protected int subX => min_x;
        protected int supY => max_y;
        protected int subY => min_y;

        private SizeStruct graph_size;
        private readonly int max_x;
        private readonly int min_x;
        private readonly int max_y;
        private readonly int min_y;

        public Canvas(int length, int width)
        {
            graph_size = new SizeStruct(length, width);
            max_x = (length + 1) >> 1;
            max_y = (width + 1) >> 1;
            min_x = max_x - length;
            min_y = max_y - width;
        }

        protected bool InsideRange(int x, int y)
        {
            bool x_inside = min_x <= x && x < max_x;
            bool y_inside = min_y <= y && y < max_y;
            return x_inside && y_inside;
        }

        public abstract T Output();
    }
}
