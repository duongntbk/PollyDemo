using System;

namespace DemoCustomPolicy.MyRandom
{
    public class BitGetter : IBitGetter
    {
        private readonly Random _random;

        public BitGetter() => _random = new Random();

        public int Get() => _random.Next(0, 2);
    }
}
