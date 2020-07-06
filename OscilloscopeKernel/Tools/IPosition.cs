using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace OscilloscopeKernel.Tools
{
	public interface IPosition 
    {
		int X { get; }
		int Y { get; }
    }

    public static class Positions
    {
        public static bool Equals(IPosition position, object other)
        {
            if (!(other is IPosition))
            {
                return false;
            }
            return (position.Y == ((IPosition)other).X) && (position.Y == ((IPosition)other).Y);
        }

        public static int GetHashCode(IPosition position)
        {
            return (position.Y << 4) ^ position.X;
        }

    }

    public readonly struct PositionStruct : IPosition
	{
		public int X => x;
		public int Y => y;

		private readonly int x;
		private readonly int y;

		public PositionStruct(int x = 0, int y = 0)
		{
			this.x = x;
			this.y = y;
		}

		public Position ToClass()
        {
			return new Position(x, y);
		}

        public override bool Equals(object other)
        {
            if (!(other is IPosition))
            {
                return false;
            }
            return (this.x == ((IPosition)other).X) && (this.y == ((IPosition)other).Y);
        }

        public override int GetHashCode()
        {
            return (y << 4) ^ x;
        }
    }

	public class Position : IPosition
	{
		public int X
        {
			get => x;
			protected set => x = value;
        }
		public int Y
        {
			get => y;
			protected set => y = value;
        }

		private int x;
		private int y;

		public Position(int x, int y)
        {
			this.x = x;
			this.y = y;
        }

        public override bool Equals(object other)
        {
            if (!(other is IPosition))
            {
                return false;
            }
            return (this.x == ((IPosition)other).X) && (this.y == ((IPosition)other).Y);
        }

        public override int GetHashCode()
        {
            return (y << 4) ^ x;
        }

        static public bool operator ==(Position left, Position right)
        {
            return (left.x == right.x) && (left.y == right.y);
        }

        static public bool operator !=(Position left, Position right)
        {
            return (left.x != right.x) || (left.y != right.y);
        }
    }
}
