using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace OscilloscopeKernel.Tools
{

    public class ConcurrentCachedPositonPool
    {
        private ConcurrentQueue<VariablePosition> free_pool = new ConcurrentQueue<VariablePosition>();
        private ConcurrentQueue<VariablePosition> used_pool = new ConcurrentQueue<VariablePosition>();

        public ConcurrentCachedPositonPool(int init_number = 200)
        {
            if (init_number <= 0)
            {
                init_number = 200;
            }
            for (int i = 0; i < init_number; i++)
            {
                free_pool.Enqueue(new VariablePosition());
            }
        }

        public Position AllocPosition(int x, int y)
        {
            VariablePosition output;
            if (free_pool.TryDequeue(out output))
            {
                output.X = x;
                output.Y = y;
                return output;
            }
            output = new VariablePosition(x, y);
            used_pool.Enqueue(output);
            return output;
        }

        public void FreeAllPosition()
        {
            ConcurrentQueue<VariablePosition> temp = free_pool;
            free_pool = used_pool;
            used_pool = temp;
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
