using System;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeKernel.Tools
{
    public readonly struct PositionStruct
	{
		public readonly int X { get; }
		public readonly int Y { get; }

		public PositionStruct(int x = 0, int y = 0)
		{
			this.X = x;
			this.Y = y;
		}

		public Position ToClass()
        {
			return new Position(X, Y);
        }
	}

	public class Position
    {
		public int X => x;
		public int Y => y;

		private int x;
		private int y;

		public Position(int x, int y)
        {
			this.x = x;
			this.y = y;
        }


		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			if (this.GetType() != other.GetType())
			{
				return false;
			}
			return (this.x == ((Position)other).x) && (this.y == ((Position)other).y);
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
