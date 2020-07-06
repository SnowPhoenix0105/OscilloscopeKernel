using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OscilloscopeKernel.Drawing;
using OscilloscopeKernel.Tools;

namespace OscilloscopeFramwork
{
    public class BitmapCanvas : ICanvas<Bitmap>
    {
        //TODO
        public ColorStruct this[int x, int y] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ref SizeStruct GraphSize => throw new NotImplementedException();

        public Bitmap Output()
        {
            throw new NotImplementedException();
        }
    }
}
