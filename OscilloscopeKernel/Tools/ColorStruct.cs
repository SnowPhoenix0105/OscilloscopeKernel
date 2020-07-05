﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OscilloscopeKernel.Tools
{
    public readonly struct ColorStruct
    {
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }
        public byte A { get; }

        public ColorStruct(byte R = 0, byte G = 0, byte B = 0, byte A = 0xff)
        {
            this.R = R;
            this.G = G;
            this.B = B;
            this.A = A;
        }

        /// <summary>
        /// post-condition: 
        ///     ensure that the channel is set to 0xFF if it will overflow 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        static public ColorStruct operator + (ColorStruct left, ColorStruct right)
        {
            int new_R = left.R + right.R;
            int new_G = left.G + right.G;
            int new_B = left.B + right.B;
            int new_A = left.A + right.B;
            return new ColorStruct(
                R: new_R > Byte.MaxValue ? Byte.MaxValue : (byte)new_R,
                G: new_G > Byte.MaxValue ? Byte.MaxValue : (byte)new_G,
                B: new_B > Byte.MaxValue ? Byte.MaxValue : (byte)new_B,
                A: new_A > Byte.MaxValue ? Byte.MaxValue : (byte)new_A
            );
        }

        /// <summary>
        /// post-condition: 
        ///     ensure that the channel is set to 0 if it will overflow 
        /// </summary>
        /// <param name="left">minuend</param>
        /// <param name="right">subtrahend</param>
        /// <returns></returns>
        static public ColorStruct operator - (ColorStruct left, ColorStruct right)
        {
            int new_R = left.R - right.R;
            int new_G = left.G - right.G;
            int new_B = left.B - right.B;
            int new_A = left.A + right.B;
            return new ColorStruct(
                R: new_R < Byte.MinValue ? Byte.MinValue : (byte)new_R,
                G: new_G < Byte.MinValue ? Byte.MinValue : (byte)new_G,
                B: new_B < Byte.MinValue ? Byte.MinValue : (byte)new_B,
                A: new_A < Byte.MinValue ? Byte.MinValue : (byte)new_A
            );
        }

        static public ColorStruct operator * (ColorStruct color, double times)
        {
            double new_R = color.R * times;
            double new_G = color.G * times;
            double new_B = color.B * times;
            return new ColorStruct(
                R: new_R < Byte.MinValue ? Byte.MinValue : (byte)new_R,
                G: new_G < Byte.MinValue ? Byte.MinValue : (byte)new_G,
                B: new_B < Byte.MinValue ? Byte.MinValue : (byte)new_B,
                A: color.A
            );
        }

        static public ColorStruct operator * (double times, ColorStruct color)
        {
            return color * times;
        }
    }
}
