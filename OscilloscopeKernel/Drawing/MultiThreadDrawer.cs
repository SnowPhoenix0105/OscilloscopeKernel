﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using OscilloscopeKernel.Tools;

namespace OscilloscopeKernel.Drawing
{
    public abstract class ConcurrentDrawer : IPointDrawer
    {
        public bool IsMultiThreadSafe => true;

        public ref SizeStruct GraphSize => ref graph_size;

        private SizeStruct graph_size;
        private SizeStruct old_point_size;
        private readonly int max_x;
        private readonly int max_y;
        private CachedPositonPool position_pool;
        private ConcurrentDictionary<IPosition> positions = new ConcurrentDictionary<IPosition>();
        private readonly LinkedList<Position> offsets = new LinkedList<Position>();

        public ConcurrentDrawer(int length, int width)
        {
            graph_size = new SizeStruct(length, width);
            position_pool = new CachedPositonPool(Math.Max(length, width));
            max_x = (int)(0.6 * length);
            max_y = (int)(0.6 * length);
        }

        public void SetPoint(in PositionStruct position)
        {
            if (Math.Abs(position.X) <= max_x && Math.Abs(position.Y) <= max_y)
            {
                if (!positions.Contains(position))
                {
                    positions.Add(position_pool.AllocPosition(position.X, position.Y));
                }
            }
        }

        protected abstract void FreshOffsets(LinkedList<Position> offsets, in SizeStruct point_size);

        public void DrawAllPoint<T>(ICanvas<T> canvas, ColorStruct color, in SizeStruct point_size)
        {
            if (!(this.old_point_size.Equals(point_size)))
            {
                offsets.Clear();
                this.old_point_size = point_size;
                this.FreshOffsets(offsets, point_size);
            }
            foreach (Position center in positions)
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
            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < width; y++)
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
