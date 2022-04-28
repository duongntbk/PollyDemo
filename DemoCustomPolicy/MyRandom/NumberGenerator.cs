using System;

namespace DemoCustomPolicy.MyRandom
{
    public class NumberGenerator : INumberGenerator
    {
        private readonly IBitGetter _bitGetter;

        public NumberGenerator(IBitGetter bitGetter) => _bitGetter = bitGetter;

        public int Next(int bitCount)
        {
            var rs = 0;
            for (var bitIndex = 0; bitIndex < bitCount; bitIndex++)
            {
                var bit = _bitGetter.Get();
                rs += (int)Math.Pow(2, bitIndex) * bit;
            }

            return rs;
        }
    }
}
