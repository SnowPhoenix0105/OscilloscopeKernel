using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using OscilloscopeKernel.Tools;
using System.Drawing;

namespace OscilloscopeKernel.Drawing
{
    public abstract class ConcurrentDrawer : IPointDrawer
    {
        public bool IsConcurrent => true;

        public ref SizeStruct GraphSize => ref graph_size;

        private SizeStruct graph_size;
        private SizeStruct old_point_size = new SizeStruct(-1, -1);
        private readonly int max_x;
        private readonly int max_y;
        private ConcurrentCachedPositonPool position_pool;
        private ConcurrentDictionary<IPosition, byte> positions = new ConcurrentDictionary<IPosition, byte>();
        private readonly LinkedList<Position> offsets = new LinkedList<Position>();

        public ConcurrentDrawer(int length, int width)
        {
            graph_size = new SizeStruct(length, width);
            position_pool = new ConcurrentCachedPositonPool(Math.Max(length, width));
            max_x = (int)(0.6 * length);
            max_y = (int)(0.6 * length);
        }

        public void SetPoint(in PositionStruct position)
        {
            if (Math.Abs(position.X) <= max_x && Math.Abs(position.Y) <= max_y)
            {
                if (!positions.ContainsKey(position))
                {
                    positions.TryAdd(position_pool.AllocPosition(position.X, position.Y), 0);
                }
            }
        }

        protected abstract void FreshOffsets(LinkedList<Position> offsets, in SizeStruct point_size);

        public void DrawAllPoint<T>(ICanvas<T> canvas, Color color, in SizeStruct point_size)
        {
            if (!(this.old_point_size.Equals(point_size)))
            {
                offsets.Clear();
                this.old_point_size = point_size;
                this.FreshOffsets(offsets, point_size);
            }
            foreach (Position center in positions.Keys)
            {
                foreach (Position offset in offsets)
                {
                    canvas[center.X + offset.X, center.Y + offset.Y] = color;
                    canvas[center.X - offset.X, center.Y + offset.Y] = color;
                    canvas[center.X + offset.X, center.Y - offset.Y] = color;
                    canvas[center.X - offset.X, center.Y - offset.Y] = color;
                }
            }
            positions.Clear();
            position_pool.FreeAllPosition();
        }
    }

    public class ConcurrentOvalPointDrawer : ConcurrentDrawer
    {
        public ConcurrentOvalPointDrawer(int length, int width) : base(length, width) { }

        protected override void FreshOffsets(LinkedList<Position> offsets, in SizeStruct point_size)
        {
            int length = point_size.Length;
            int width = point_size.Width;
            int square_radius = length * length + width * width;
            for (int x = 0; x <= length; x++)
            {
                for (int y = 0; y <= width; y++)
                {
                    if (x * x + y * y <= square_radius)
                    {
                        offsets.AddLast(new Position(x, y));        //  cannot alloc from pool here
                    }
                }
            }
        }
    }

    public class ConcurrentCrossPointDrawer : ConcurrentDrawer
    {
        public ConcurrentCrossPointDrawer(int length, int width) : base(length, width) { }

        protected override void FreshOffsets(LinkedList<Position> offsets, in SizeStruct point_size)
        {
            for (int i = 0; i < point_size.Length; i++)
            {
                offsets.AddLast(new Position(i, 0));
            }
            for (int i = 0; i < point_size.Width; i++)
            {
                offsets.AddLast(new Position(0, i));
            }
        }
    }
}
