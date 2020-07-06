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

        public override bool IsReady
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

        private Color[,] colors;
        private Color init_color;
        private Task reset_task = null;

        public ColorArrayCanvas(int length, int width, Color init_color)
            :base(length, width)
        {
            this.init_color = init_color;
            InternalReset();
        }

        private void InternalReset()
        {
            reset_task = new Task(() =>
            {
                colors = new Color[GraphSize.Length, GraphSize.Width];
                for (int x = 0; x < GraphSize.Length; x++)
                {
                    for (int y = 0; y < GraphSize.Width; y++)
                    {
                        colors[x, y] = init_color;
                    }
                }
            });
            reset_task.Start();
        }

        public override Color[,] Output()
        {
            Color[,] output = colors;
            InternalReset();
            return output;
        }
    }
}
