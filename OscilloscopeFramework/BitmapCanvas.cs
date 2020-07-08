using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OscilloscopeKernel.Drawing;
using OscilloscopeKernel.Tools;

namespace OscilloscopeFramework
{
    public class BitmapCanvas : Canvas<Bitmap>
    {
        public override Color this[int x, int y] {
            get
            {
                if (InsideRange(x, y))
                {
                    return bitmap.GetPixel(x - subX, y - subY);
                }
                return default(Color);
            }
            set
            {
                if (InsideRange(x, y))
                {
                    bitmap.SetPixel(x - subX, y - subY, value);
                }
            }
        }

        private Bitmap bitmap;

        public BitmapCanvas(int length, int width, BackgroundDrawer background_drawer = null)
            : base(length, width, background_drawer, true) { }

        protected override void ResetActionsBeforeDrawerWork()
        {
            bitmap = new Bitmap(GraphSize.Length, GraphSize.Width, PixelFormat.Format32bppPArgb);
        }

        public override Bitmap Output()
        {
            Bitmap output = bitmap;
            InternalReset();
            return output;
        }
    }
}
