using System;
using System.Collections.Generic;

namespace Bonsai.Gui.ZedGraph
{
    static class ThrowHelper
    {
        public static void ThrowArgumentOutOfRange(string paramName)
        {
            throw new ArgumentOutOfRangeException(paramName);
        }

        public static void ThrowArgumentNull(string paramName)
        {
            throw new ArgumentNullException(paramName);
        }

        public static void ThrowIfNotEquals<T>(T left, T right, string message)
        {
            if (!EqualityComparer<T>.Default.Equals(left, right))
            {
                throw new InvalidOperationException(message);
            }
        }
    }
}
