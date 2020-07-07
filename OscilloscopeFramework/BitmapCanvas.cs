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

        public override bool IsReady
        {
            get
            {
                if (reset_bitmap == null)
                {
                    return true;
                }
                if (reset_bitmap.IsCompleted)
                {
                    reset_bitmap = null;
                    return true;
                }
                return false;
            }
        }

        private Bitmap bitmap;
        private Color init_color;
        private Task reset_bitmap = null;

        public BitmapCanvas(int length, int width)
            : base(length, width)
        {
            this.init_color = Color.Black;
            InternalResetBitmap();
        }

        public BitmapCanvas(int length, int width, Color background_color)
            : base(length, width)
        {
            this.init_color = background_color;
            InternalResetBitmap();
        }

        private void InternalResetBitmap()
        {
            reset_bitmap = new Task(() =>
            {
                bitmap = new Bitmap(GraphSize.Length, GraphSize.Width, PixelFormat.Format32bppArgb);
                for (int x = 0; x < GraphSize.Length; x++)
                {
                    for (int y = 0; y < GraphSize.Width; y++)
                    {
                        bitmap.SetPixel(x, y, init_color);
                    }
                }
            });
            reset_bitmap.Start();
        }

        public override Bitmap Output()
        {
            Bitmap output = bitmap;
            InternalResetBitmap();
            return output;
        }
    }
}
