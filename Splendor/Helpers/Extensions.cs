using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Splendor.Helpers
{
    public static class Extensions
    {
        private static Random random = new Random((int)DateTime.Now.Ticks);
        public static ICollection<T> TakeRandom<T>(this ICollection<T> collection, int n)
        {
            if (n > collection.Count)
            {
                throw new ArgumentOutOfRangeException("You cannot take more elements that are in collection.");
            }

            ICollection<T> result = new List<T>();
            // IEnumerable<int> sequence = Enumerable.Range(0, collection.Count).OrderBy(x => random.Next());
            IEnumerable<int> sequence = Enumerable.Range(0, collection.Count).ToList().Shuffle();

            for (int i = 0; i < n; ++i)
            {
                result.Add(collection.ElementAt(sequence.ElementAt(i)));
            }

            return result;
        }

        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            for (var i = 0; i < list.Count; i++)
                list.Swap(i, random.Next(i, list.Count));
            return list;
        }

        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        public static string CamelCase(this string str)
        {
            return str;
        }
    }
}