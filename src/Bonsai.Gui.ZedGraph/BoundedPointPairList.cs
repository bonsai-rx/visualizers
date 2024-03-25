using System;
using ZedGraph;

namespace Bonsai.Gui.ZedGraph
{
    internal class BoundedPointPairList : IPointListEdit
    {
        int maxCapacity = int.MaxValue;
        double minValue = double.MinValue;
        double maxValue = double.MaxValue;
        readonly PointPairDeque points = new();

        public BoundedPointPairList()
            : this(0)
        {
        }

        public BoundedPointPairList(int capacity)
        {
            SetCapacity(capacity);
        }

        public BoundedPointPairList(IPointList points, int capacity)
            : this(capacity)
        {
            if (points != null)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    Add(points[i]);
                }
            }
        }

        private void EnsureCapacity()
        {
            while (points.Count > maxCapacity)
            {
                points.TryDequeue(out _);
            }
        }

        public void SetBounds(double min, double max)
        {
            if (max < min) ThrowHelper.ThrowArgumentOutOfRange(nameof(max));

            minValue = min;
            maxValue = max;
            while (points.Count > 0 && points[0].Z < min)
            {
                points.TryDequeue(out _);
            }

            while (points.Count > 0 && points[points.Count - 1].Z > max)
            {
                points.TryDequeueLast(out _);
            }
        }

        public void SetCapacity(int capacity)
        {
            if (capacity < 0) ThrowHelper.ThrowArgumentOutOfRange(nameof(capacity));
            maxCapacity = capacity > 0 ? capacity : int.MaxValue;
            EnsureCapacity();
        }

        public PointPair this[int index]
        {
            get => points[index];
            set => points[index] = value;
        }

        PointPair IPointList.this[int index] => this[index];

        public int Count => points.Count;

        public void Add(PointPair point)
        {
            if (point == null)
            {
                ThrowHelper.ThrowArgumentNull(nameof(point));
            }

            Add(point.X, point.Y, point.Z, point.Tag);
        }

        public void Add(double x, double y)
        {
            Add(x, y, 0, null);
        }

        public void Add(double x, double y, string label)
        {
            Add(x, y, 0, label);
        }

        public void Add(double x, double y, double z, object tag)
        {
            if (z >= minValue && z <= maxValue)
            {
                points.Enqueue(x, y, z, tag);
            }

            EnsureCapacity();
        }

        public void Clear()
        {
            points.Clear();
        }

        public object Clone()
        {
            return new BoundedPointPairList(this, maxCapacity);
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }
    }
}
