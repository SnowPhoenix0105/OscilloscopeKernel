using OscilloscopeKernel.Drawing;
using OscilloscopeKernel.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace OscilloscopeKernel.Drawing
{
    public class NoneBackgroundDrawer : IBackgroundDrawer
    {
        public ref SizeStruct GraphSize => ref size_struct;

        private SizeStruct size_struct;

        public NoneBackgroundDrawer(int length, int width)
        {
            size_struct = new SizeStruct(length, width);
        }

        public void Draw<T>(ICanvas<T> canvas) { }
    }

    public class BackgroundDrawer : IBackgroundDrawer
    {
        public ref SizeStruct GraphSize => ref size_struct;

        protected int XMax => max_x;

        protected int YMax => max_y;

        private SizeStruct size_struct;
        private Color background_color;
        private int max_x;
        private int max_y;

        public BackgroundDrawer(int length, int width, Color background_color)
        {
            this.max_x = (length >> 1) + 1;
            this.max_y = (width >> 1) + 1;
            this.background_color = background_color;
            size_struct = new SizeStruct(length, width);
        }

        public void Draw<T>(ICanvas<T> canvas)
        {
            for (int x = 0; x < max_x; x++)
            {
                for (int y = 0; y < max_y; y++)
                {
                    canvas[x, y] = background_color;
                    canvas[-x, y] = background_color;
                    canvas[x, -y] = background_color;
                    canvas[-x, -y] = background_color;
                }
            }
        }
    }

    public class CrossBackgroundDrawer : BackgroundDrawer
    {

        private SizeStruct size_struct;
        private Color ruler_color;
        private int extend_width;

        public CrossBackgroundDrawer(int length, int width, Color background_color, Color ruler_color, int extend_width = 1)
            : base(length, width, background_color)
        {
            this.extend_width = extend_width >= 0 ? extend_width : 1 ;
            this.ruler_color = ruler_color;
            size_struct = new SizeStruct(length, width);
        }

        public new void Draw<T>(ICanvas<T> canvas)
        {
            base.Draw(canvas);
            for (int i = 0; i < XMax; i++)
            {
                for (int j = 0; j < extend_width; j++)
                {
                    canvas[i, j] = ruler_color;
                    canvas[-i, j] = ruler_color;
                    canvas[i, -j] = ruler_color;
                    canvas[-i, -j] = ruler_color;
                }
            }
            for (int i = 0; i < YMax; i++)
            {
                for (int j = 0; j < extend_width; j++)
                {
                    canvas[j, i] = ruler_color;
                    canvas[-j, i] = ruler_color;
                    canvas[j, -i] = ruler_color;
                    canvas[-j, -i] = ruler_color;
                }
            }
        }
    }
}
