using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OscilloscopeCore.Tools
{
    public class ColorTuple
    {
        public byte R => r;
        public byte G => g;
        public byte B => b;

        private byte r;
        private byte g;
        private byte b;

        public ColorTuple(byte R = 0, byte G = 0, byte B = 0)
        {
            this.r = R;
            this.g = G;
            this.b = B;
        }

        /// <summary>
        /// post-condition: 
        ///     ensure that the channel is set to 0xFF if it will overflow 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        static public ColorTuple operator + (ColorTuple A, ColorTuple B)
        {
            int new_R = A.r + B.r;
            int new_G = A.g + B.g;
            int new_B = A.b + B.b;
            return new ColorTuple(
                R: new_R > Byte.MaxValue ? Byte.MaxValue : (byte)new_R,
                G: new_G > Byte.MaxValue ? Byte.MaxValue : (byte)new_G,
                B: new_B > Byte.MaxValue ? Byte.MaxValue : (byte)new_B
            );
        }

        /// <summary>
        /// post-condition: 
        ///     ensure that the channel is set to 0 if it will overflow 
        /// </summary>
        /// <param name="A">minuend</param>
        /// <param name="B">subtrahend</param>
        /// <returns></returns>
        static public ColorTuple operator - (ColorTuple A, ColorTuple B)
        {
            int new_R = A.r - B.r;
            int new_G = A.g - B.g;
            int new_B = A.b - B.b;
            return new ColorTuple(
                R: new_R < Byte.MinValue ? Byte.MinValue : (byte)new_R,
                G: new_G < Byte.MinValue ? Byte.MinValue : (byte)new_G,
                B: new_B < Byte.MinValue ? Byte.MinValue : (byte)new_B
            );
        }

        static public ColorTuple operator * (ColorTuple color, double times)
        {
            double new_R = color.r * times;
            double new_G = color.g * times;
            double new_B = color.b * times;
            return new ColorTuple(
                R: new_R < Byte.MinValue ? Byte.MinValue : (byte)new_R,
                G: new_G < Byte.MinValue ? Byte.MinValue : (byte)new_G,
                B: new_B < Byte.MinValue ? Byte.MinValue : (byte)new_B
            );
        }

        static public ColorTuple operator * (double times, ColorTuple color)
        {
            return color * times;
        }
    }
}
