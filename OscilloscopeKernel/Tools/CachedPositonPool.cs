using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace OscilloscopeKernel.Tools
{
    public class CachedPositonPool
    {
        private readonly LinkedList<VariablePosition> pool = new LinkedList<VariablePosition>();
        private IEnumerator<VariablePosition> enumerator;
        private bool overflow = false;

        public CachedPositonPool(int init_number = 100)
        {
            if (init_number <= 0)
            {
                init_number = 100;
            }
            for (int i = 0; i < init_number; i++)
            {
                pool.AddLast(new VariablePosition());
            }
            enumerator = pool.GetEnumerator();
        }
        
        public Position AllocPosition(int x, int y)
        {
            VariablePosition output;
            if (!overflow)
            {
                if (enumerator.MoveNext())
                {
                    output = enumerator.Current;
                    output.X = x;
                    output.Y = y;
                    return output;
                }
                overflow = true;
                enumerator.Dispose();
            }
            output = new VariablePosition(x, y);
            pool.AddLast(output);
            return output;
        }

        public void FreeAllPosition()
        {
            if (!overflow)
            {
                enumerator.Dispose();
            }
            enumerator = pool.GetEnumerator();
            overflow = false;
        }

        protected sealed class VariablePosition : Position
        {
            public new int X
            {
                get => base.X;
                set => base.X = value;
            }

            public new int Y
            {
                get => base.Y;
                set => base.Y = value;
            }

            public VariablePosition(int x = 0, int y = 0) : base(x, y) { }
		}
    }
}
