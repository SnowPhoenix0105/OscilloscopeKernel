using OscilloscopeKernel.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

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

        public ColorArrayCanvas(int length, int width, BackgroundDrawer background_drawer = null)
            :base(length, width, background_drawer, true) { }

        protected override void ResetActionsBeforeDrawerWork()
        {
            colors = new Color[GraphSize.Length, GraphSize.Width];
        }

        public override Color[,] Output()
        {
            Color[,] output = colors;
            InternalReset();
            return output;
        }
    }
}
