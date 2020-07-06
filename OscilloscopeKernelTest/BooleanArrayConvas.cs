using OscilloscopeKernel.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OscilloscopeKernelTest
{
    public class BooleanArrayConvas : Canvas<bool[,]>
    {
        public override Color this[int x, int y] 
        { 
            get
            {
                if (InsideRange(x, y))
                {
                    return canvas[x - subX, y - subY] ? Color.White : Color.Black;
                }
                return default(Color);
            }
            set
            {
                if (InsideRange(x, y))
                {
                    if (value.Equals(Color.Black))
                    {
                        canvas[x - subX, y - subY] = false;
                    }
                    else
                    {
                        canvas[x - subX, y - subY] = true;
                    }
                }
            }
        }

        public override bool IsReady => true;

        private bool[,] canvas;

        public BooleanArrayConvas(int length, int width)
            : base(length, width)
        {
            canvas = new bool[length, width];
        }

        public override bool[,] Output()
        {
            bool[,] output = canvas;
            canvas = new bool[GraphSize.Length, GraphSize.Width];
            return output;
        }
    }
}
