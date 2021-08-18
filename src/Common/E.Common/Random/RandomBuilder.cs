using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace E.Common.Random
{
    public static class RandomBuilder
    {
        private static readonly PcgRandom RandomObj;

        static RandomBuilder()
        {
            RandomObj = new PcgRandom();
        }
        /// <summary>
        /// 描 述:创建加密随机数生成器 生成强随机种子
        /// </summary>
        /// <returns></returns>
        private static int GetRandomSeed()
        {
            var bytes = new byte[4];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        private static PcgRandom GetRandom()
        {
            //return new Random(GetRandomSeed());
            return RandomObj;
        }

        /// <summary>
        /// 随机整数，包括最小值，不包括最大值
        /// </summary>
        public static Task<int> Random(int minValue, int maxValue)
        {
            var ranValue = GetRandom().GetInt32(minValue, maxValue);
            return Task.FromResult(ranValue);
        }

        /// <summary>
        /// 从给的数组中随机一个值
        /// </summary>
        public static Task<T> Random<T>(params T[] values)
        {
            var ranValue = GetRandom().GetInt32(0, values.Length);
            return Task.FromResult(values[ranValue]);
        }

        public static Task<int> Random(int maxValue)
        {
            var ranValue = GetRandom().GetInt32(0,maxValue);
            return Task.FromResult(ranValue);
        }

        public static Task<int> Random()
        {
            var ranValue = GetRandom().GetInt32();
            return Task.FromResult(ranValue);
        }

        /// <summary>
        /// 按权重随机
        /// </summary>
        public static List<RandomObject<TV>> GetRandomList<TV>(List<RandomObject<TV>> list, int count)
        {
            if (list == null || list.Count <= count || count <= 0)
            {
                return list;
            }

            //计算权重综合
            int totalWeights = 0;
            for (int i = 0; i < list.Count; i++)
            {
                totalWeights += list[i].Weight;
            }

            System.Random ran = new System.Random(GetRandomSeed());  
            List<KeyValuePair<int, int>> wlist = new List<KeyValuePair<int, int>>();    
            for (int i = 0; i < list.Count; i++)
            {
                int w = list[i].Weight + ran.Next(0, totalWeights);  
                wlist.Add(new KeyValuePair<int, int>(i, w));
            }

            //排序
            wlist.Sort(
                (kvp1, kvp2) => kvp2.Value - kvp1.Value);

            var newList = new List<RandomObject<TV>>();
            for (int i = 0; i < count; i++)
            {
                newList.Add(list[wlist[i].Key]);
            }

            return newList;
        }
    }

    public class RandomObject<T>
    {
        public int Weight { set; get; }

        public T Item { get; set; }
    }
}
