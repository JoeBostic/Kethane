﻿using System;
using System.Collections.Generic;

namespace Kethane.EnumerableExtensions
{
    public static class EnumerableExtensions
    {
        public struct Pair<T>
        {
            public T First { get; private set; }
            public T Second { get; private set; }

            public Pair(T first, T second)
                : this()
            {
                First = first;
                Second = second;
            }
        }

        public static IEnumerable<Pair<T>> AdjacentPairs<T>(this IEnumerable<T> sequence)
        {
            var enumerator = sequence.GetEnumerator();
            if (!enumerator.MoveNext()) { yield break; }

            T first, previous;
            first = previous = enumerator.Current;

            while (enumerator.MoveNext())
            {
                yield return new Pair<T>(previous, enumerator.Current);
                previous = enumerator.Current;
            }

            yield return new Pair<T>(previous, first);
        }
    }
}
