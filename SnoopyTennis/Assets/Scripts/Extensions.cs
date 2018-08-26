using System;
using System.Collections.Generic;

public static class Extensions
{
    public static T FindMinItem<T>(this IEnumerable<T> source, Func<T, float> action) where T : class
    {
        using (var enumerator = source.GetEnumerator())
        {
            if (!enumerator.MoveNext())
                return null;
            var smallest = enumerator.Current;
            var smallestValue = action(smallest);

            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                var newValue = action(item);
                if (newValue < smallestValue)
                {
                    smallest = item;
                    smallestValue = newValue;
                }
            }
            return smallest;
        }
    }
}
