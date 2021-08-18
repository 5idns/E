using System;

namespace E.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class MathExtensions
    {

        /// <summary>
        /// 排列计算P
        /// </summary>
        /// <param name="r">参与选择的元素个数</param>
        /// <param name="n">元素总个数</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="OverflowException"></exception>
        public static long Permutation(int r, int n)
        {
            if (r > n || r <= 0 || n <= 0) throw new ArgumentException("params invalid!");
            long t = 1;
            var i = n;

            while (i != n - r)
            {
                try
                {
                    checked
                    {
                        t *= i;
                    }
                }
                catch
                {
                    throw new OverflowException("overflow happens!");
                }
                --i;
            }
            return t;
        }

        /// <summary>
        /// 组合计算C
        /// </summary>
        /// <param name="n">组合结果长度</param>
        /// <param name="m">组合运算长度</param>
        /// <returns></returns>
        public static int Combination(int n, int m)
        {
            if (n > m) return 0;
            decimal i = 1;
            for (var j = m; j > n; j--)
            {
                i *= j;
            }

            for (var j = m; j > n; j--)
            {
                i /= m - j + 1;
            }
            return (int)i;
        }

    }
}
