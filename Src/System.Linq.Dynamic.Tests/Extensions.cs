using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq.Dynamic.Tests
{
    /// <summary>
    /// Only for testing. Do not use these in production. They are wildly inefficient as they are not parallel and will cause lazy loaded lists to resolve.
    /// </summary>
    public static class Extensions
    {
        public static IEnumerable<dynamic> Select(this object source, Func<dynamic, dynamic> map)
        {
            foreach (dynamic item in source as dynamic)
            {
                yield return map(item);
            }
        }

        public static T First<T>(this IQueryable<T> source)
        {
            var result = source.FirstOrDefault();
            if (result == null)
                throw new InvalidOperationException();
            return result;
        }

        public static dynamic First(this object source)
        {
            var result = source.FirstOrDefault();
            if (result == null)
                throw new InvalidOperationException();
            return result;
        }

        public static dynamic First(this object source, Func<dynamic, dynamic> predicate)
        {
            var result = source.FirstOrDefault(predicate);
            if(result == null)
                throw new InvalidOperationException();
            return result;
        }

        public static T FirstOrDefault<T>(this IQueryable<T> source)
        {
            foreach (dynamic item in source as dynamic)
            {
                return item;
            }
            return default(T);
        }

        public static dynamic FirstOrDefault(this object source)
        {
            foreach (dynamic item in source as dynamic)
            {
                return item;
            }
            return null;
        }

        public static dynamic FirstOrDefault(this object source, Func<dynamic, dynamic> predicate)
        {
            foreach (dynamic item in source as dynamic)
            {
                if (predicate(item))
                    return item;
            }
            return null;
        }

        public static IEnumerable<dynamic> Where(this object source, Func<dynamic, dynamic> predicate)
        {
            foreach (dynamic item in source as dynamic)
            {
                if (predicate(item))
                    yield return item;
            }
        }
    }
}
