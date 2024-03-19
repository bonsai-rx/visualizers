using System;
using System.Collections;
using System.Collections.Generic;
using ZedGraph;

namespace Bonsai.Gui.Visualizers
{
    internal class PointPairDeque : IReadOnlyList<PointPair>
    {
        int head;
        int tail;
        int count;
        PointPair[] buffer;

        public PointPairDeque() : this(capacity: 4)
        {
        }

        public PointPairDeque(int capacity)
        {
            if (capacity < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRange(nameof(capacity));
            }

            EnsureCapacity(capacity);
        }

        private void EnsureCapacity(int capacity)
        {
            var array = new PointPair[capacity];
            if (count > 0)
            {
                if (head < tail)
                {
                    Array.Copy(buffer, head, array, 0, count);
                }
                else
                {
                    Array.Copy(buffer, head, array, 0, buffer.Length - head);
                    Array.Copy(buffer, 0, array, buffer.Length - head, tail);
                }
            }

            for (int i = count; i < array.Length; i++)
            {
                array[i] = new PointPair();
            }

            buffer = array;
            head = 0;
            tail = count;
        }

        private int GetIndexInternal(int index)
        {
            return (head + index) % buffer.Length;
        }

        public PointPair this[int index]
        {
            get
            {
                if (index < 0 || index >= count)
                {
                    ThrowHelper.ThrowArgumentOutOfRange(nameof(index));
                }

                return buffer[GetIndexInternal(index)];
            }
            set
            {
                if (index < 0 || index >= count)
                {
                    ThrowHelper.ThrowArgumentOutOfRange(nameof(index));
                }

                buffer[GetIndexInternal(index)] = value;
            }
        }

        public int Count => count;

        public void Enqueue(PointPair point)
        {
            Enqueue(point.X, point.Y, 0, null);
        }

        public void Enqueue(double x, double y)
        {
            Enqueue(x, y, 0, null);
        }

        public void Enqueue(double x, double y, double z, object tag)
        {
            if (count >= buffer.Length)
            {
                EnsureCapacity(buffer.Length * 2);
            }

            buffer[tail].X = x;
            buffer[tail].Y = y;
            buffer[tail].Z = z;
            buffer[tail].Tag = tag;
            tail = (tail + 1) % buffer.Length;
            count++;
        }

        public bool TryDequeue(out PointPair result)
        {
            if (count == 0)
            {
                result = default;
                return false;
            }

            result = buffer[head];
            buffer[head].Tag = default;
            head = (head + 1) % buffer.Length;
            count--;
            return true;
        }

        public bool TryDequeueLast(out PointPair result)
        {
            if (count == 0)
            {
                result = default;
                return false;
            }

            var index = tail - 1;
            if (index < 0) index += buffer.Length;
            result = buffer[index];
            buffer[index].Tag = default;
            tail = index;
            count--;
            return true;
        }

        public void Clear()
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i].Tag = default;
            }
            head = 0;
            tail = 0;
            count = 0;
        }

        public IEnumerator<PointPair> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
            {
                yield return buffer[GetIndexInternal(i)];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
