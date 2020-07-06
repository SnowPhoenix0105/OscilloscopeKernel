using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OscilloscopeKernel.Tools
{
    public readonly struct SizeStruct
    {
        public int Length { get; }
        public int Width { get; }

        public SizeStruct(int length, int width)
        {
            this.Length = length;
            this.Width = width;
        }

        public static bool operator ==(SizeStruct left, SizeStruct right)
        {
            return (left.Length == right.Length) && (left.Width == right.Width);
        }

        public static bool operator !=(SizeStruct left, SizeStruct right)
        {
            return (left.Length != right.Length) || (left.Width != right.Width);
        }

        public override bool Equals(object obj)
        {
            if (obj is SizeStruct)
            {
                return (this.Length == ((SizeStruct) obj).Length) && (this.Width == ((SizeStruct)obj).Width);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (Length << 4) ^ Width;
        }
    }
}
