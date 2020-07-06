using OscilloscopeKernel.Drawing;
using OscilloscopeKernel.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeKernel.Drawing
{
    public class CrossRulerDrawer : IRulerDrawer
    {
        public ref SizeStruct GraphSize => ref size_struct;

        protected int XMax => max_x;

        protected int YMax => max_y;

        private SizeStruct size_struct;
        private ColorStruct color_struct;
        private int max_x;
        private int max_y;
        private int extend_width;
        
        public CrossRulerDrawer(int length, int width, ColorStruct color, int extend_width = 1)
        {
            this.max_x = (length >> 1) + 1;
            this.max_y = (width >> 1) + 1;
            this.extend_width = extend_width >= 0 ? extend_width : 1 ;
            this.color_struct = color;
            size_struct = new SizeStruct(length, width);
        }

        public void Draw<T>(ICanvas<T> canvas)
        {
            for (int i = 0; i < max_x; i++)
            {
                for (int j = 0; j < extend_width; j++)
                {
                    canvas[i, j] = color_struct;
                    canvas[-i, j] = color_struct;
                    canvas[i, -j] = color_struct;
                    canvas[-i, -j] = color_struct;
                }
            }
            for (int i = 0; i < max_y; i++)
            {
                for (int j = 0; j < extend_width; j++)
                {
                    canvas[j, i] = color_struct;
                    canvas[-j, i] = color_struct;
                    canvas[j, -i] = color_struct;
                    canvas[-j, -i] = color_struct;
                }
            }
        }
    }
}
