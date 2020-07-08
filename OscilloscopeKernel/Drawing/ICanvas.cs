using System;
using System.Drawing;
using System.Threading.Tasks;
using OscilloscopeKernel.Exceptions;
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

        public bool IsReady
        {
            get
            {
                if (reset_task == null)
                {
                    return true;
                }
                if (reset_task.IsCompleted)
                {
                    reset_task = null;
                    return true;
                }
                return false;
            }
        }

        protected int supX => max_x;
        protected int subX => min_x;
        protected int supY => max_y;
        protected int subY => min_y;

        private SizeStruct graph_size;
        private readonly int max_x;
        private readonly int min_x;
        private readonly int max_y;
        private readonly int min_y;
        private IBackgroundDrawer background_drawer;
        private Task reset_task = null;

        public Canvas(int length, int width, IBackgroundDrawer background_drawer = null, bool need_reset = true)
        {
            graph_size = new SizeStruct(length, width);
            if (background_drawer != null)
            {
                if (background_drawer.GraphSize != graph_size)
                {
                    throw new DifferentGraphSizeException();
                }
                this.background_drawer = background_drawer;
            }
            else
            {
                this.background_drawer = new NoneBackgroundDrawer(length, width);
            }
            max_x = (length + 1) >> 1;
            max_y = (width + 1) >> 1;
            min_x = max_x - length;
            min_y = max_y - width;
            if (need_reset)
            {
                InternalReset();
            }
        }

        protected bool InsideRange(int x, int y)
        {
            bool x_inside = min_x <= x && x < max_x;
            bool y_inside = min_y <= y && y < max_y;
            return x_inside && y_inside;
        }

        protected virtual void ResetActionsBeforeDrawerWork() { }

        protected virtual void ResetActionsAfterDrawerWork() { }

        protected void InternalReset()
        {
            reset_task = new Task(() =>
            {
                ResetActionsBeforeDrawerWork();
                background_drawer.Draw(this);
                ResetActionsAfterDrawerWork();
            });
            reset_task.Start();
        }

        public abstract T Output();
    }
}
