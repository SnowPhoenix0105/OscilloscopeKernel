using OscilloscopeKernel.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OscilloscopeKernel.Drawing
{
    public class ColorArrayCanvas : Canvas<Color[,]>
    {
        public override Color this[int x, int y]
        {
            get
            {
                if (InsideRange(x, y))
                {
                    return colors[x - subX, y - subY];
                }
                return default(Color);
            }
            set
            {
                if (InsideRange(x, y))
                {
                    colors[x - subX, y - subY] = value;
                }
            }
        }

        private Color[,] colors;

        public ColorArrayCanvas(int length, int width)
            :base(length, width)
        {
            this.colors = new Color[length, width];
        }

        public override Color[,] Output()
        {
            Color[,] output = colors;
            colors = new Color[GraphSize.Length, GraphSize.Width];
            return output;
        }
    }
}
