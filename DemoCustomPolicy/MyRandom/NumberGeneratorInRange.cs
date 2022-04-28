using System;
using DemoCustomPolicy.MyPolicies;
using Polly;
using Polly.Wrap;

namespace DemoCustomPolicy.MyRandom
{
    public class NumberGeneratorInRange : INumberGeneratorInRange
    {
        private readonly int _bitCount;
        private readonly INumberGenerator _innerGenerator;
        private readonly PolicyWrap _policies;

        public NumberGeneratorInRange(INumberGenerator innerGenerator, int min, int max)
        {
            _innerGenerator = innerGenerator;
            _bitCount = (int)Math.Floor(Math.Log(max, 2)) + 1;

            var numberOutOfRangePolicy = NumberOutOfRangePolicy.Create(min, max);
            var retryPolicy = Policy
                .Handle<NumberOutOfRangeException>()
                .RetryForever();
            _policies = Policy.Wrap(retryPolicy, numberOutOfRangePolicy);
        }

        public int Next() => _policies.Execute(() => _innerGenerator.Next(_bitCount));
    }
}
