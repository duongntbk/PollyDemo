using System;
using System.Linq;

namespace DemoCustomPolicy.MyRandom
{
    public class NumberGenerator : INumberGenerator
    {
        private readonly IBitGetter _bitGetter;

        public NumberGenerator(IBitGetter bitGetter) => _bitGetter = bitGetter;

        public int Next(int bitCount)
        {
            var rs = 0;
            var bitIndex = 0;
            Enumerable.Range(0, bitCount).Select(_ => _bitGetter.Get()).ToList().ForEach(bit =>
            {
                rs += (int)Math.Pow(2, bitIndex) * bit;
                bitIndex++;
            });

            return rs;
        }
    }
}
