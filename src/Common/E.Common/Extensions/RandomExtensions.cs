using System.Collections.Generic;
using System.Linq;
using E.Common.Random;

namespace E.Common.Extensions
{
    public static class RandomExtensions
    {
        //[ThreadStatic]
        private static readonly PcgRandom RandomObj;

        static RandomExtensions()
        {
            RandomObj = new PcgRandom();
        }


        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            if (source != null && source.Any())
            {
                var length = (uint) source.Count();
                return source.OrderBy(i => RandomObj.GetUInt32(0, length));
            }

            return source;
        }
    }
}
