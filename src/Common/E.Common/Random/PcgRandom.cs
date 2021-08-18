using System;
using System.Security.Cryptography;

namespace E.Common.Random
{
    public class PcgRandom
    {
        private const double Inverse32Bit = 2.32830643653869628906e-010d;
        private const double Inverse52Bit = 2.22044604925031308085e-016d;

        private static readonly RandomNumberGenerator Rng = new RNGCryptoServiceProvider();

        private ulong _state;
        private readonly ulong _stream;

        public PcgRandom(ulong state, ulong stream)
        {
            _state = state;
            _stream = stream | 1UL;
        }
        public PcgRandom(ulong state) : this(state, GetSeed()) { }

        public PcgRandom() : this(GetSeed(), GetSeed()) { }

        /// <summary>
        /// Generates a uniformly distributed double between the range (0, 1).
        /// </summary>
        public double GetDouble()
        {
            return CreateDouble(GetInt32(), GetInt32());
        }
        /// <summary>
        /// Generates a uniformly distributed 32-bit signed integer between the range of int.MaxValue and int.MinValue.
        /// </summary>
        public int GetInt32()
        {
            return (int)GetUInt32();
        }
        /// <summary>
        /// Generates a uniformly distributed 32-bit signed integer between the range [min, max].
        /// </summary>
        public int GetInt32(int minValue, int maxValue)
        {
            var min = minValue < maxValue ? minValue : maxValue;
            var max = minValue < maxValue ? maxValue : minValue;
            var range = max - min;

            return (int)(GetUInt32((uint)range) + min);

            //if (range < uint.MaxValue)
            //{
            //    return (int) (GetUInt32((uint) range) + min);
            //}

            //return GetInt32();
        }
        /// <summary>
        /// Generates a uniformly distributed 32-bit unsigned integer between the range of uint.MaxValue and uint.MinValue.
        /// </summary>
        public uint GetUInt32()
        {
            return Pcg32(ref _state, _stream);
        }
        /// <summary>
        /// Generates a uniformly distributed 32-bit unsigned integer between the range [min, max].
        /// </summary>
        public uint GetUInt32(uint minValue, uint maxValue)
        {
            var min = minValue < maxValue ? minValue : maxValue;
            var max = minValue < maxValue ? maxValue : minValue;
            var range = max - min;

            if (uint.MaxValue > range)
            {
                return GetUInt32(range) + min;
            }

            return GetUInt32();
        }

        private uint GetUInt32(uint exclusiveHigh)
        {
            var threshold = (uint)((0x100000000UL - exclusiveHigh) % exclusiveHigh);
            var sample = GetUInt32();

            while (sample < threshold)
            {
                sample = GetUInt32();
            }

            return sample % exclusiveHigh;
        }

        private static double CreateDouble(int minValue, int maxValue)
        {
            // reference: https://www.doornik.com/research/randomdouble.pdf
            return 0.5d + Inverse52Bit / 2 + minValue * Inverse32Bit + (maxValue & 0x000FFFFF) * Inverse52Bit;
        }
        private static ulong GetSeed()
        {
            var buffer = new byte[sizeof(ulong)];

            Rng.GetBytes(buffer);

            return BitConverter.ToUInt64(buffer, 0);
        }
        private static uint Pcg32(ref ulong state, ulong stream)
        {
            // reference: http://www.pcg-random.org/paper.html
            state = unchecked(state * 6364136223846793005UL + stream);

            return RotateRight((uint)(((state >> 18) ^ state) >> 27), (int)(state >> 59));
        }
        private static uint RotateRight(uint value, int count)
        {
            return (value >> count) | (value << (-count & 31));
        }
    }
}
