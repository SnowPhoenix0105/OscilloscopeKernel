using System;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeKernel.Tools
{
    struct PositionTuple
	{
		public int x;
		public int y;

		public PositionTuple(int x = 0, int y = 0)
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
			return (this.x == ((PositionTuple)other).x) && (this.y == ((PositionTuple)other).y);
		}

		public override int GetHashCode()
		{
			return y << 10 | (x & 0x3ff);
		}

		static public bool operator ==(PositionTuple left, PositionTuple right)
		{
			return (left.x == right.x) && (left.y == right.y);
		}

		static public bool operator !=(PositionTuple left, PositionTuple right)
		{
			return (left.x != right.x) || (left.y != right.y);
		}
	}
}
