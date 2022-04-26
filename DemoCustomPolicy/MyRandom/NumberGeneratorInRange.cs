using System;
using DemoCustomPolicy.MyPolicies;
using Polly;
using Polly.Wrap;

namespace DemoCustomPolicy.MyRandom
{
    public class NumberGeneratorInRange : INumberGenerator
    {
        private readonly int _min;
        private readonly int _max;
        private readonly INumberGenerator _innerGenerator;
        private readonly PolicyWrap _policies;

        public NumberGeneratorInRange(INumberGenerator innerGenerator, int min, int max)
        {
            _innerGenerator = innerGenerator;
            _min = min;
            _max = max;

            var pollyContext = new Context(nameof(NumberGeneratorInRange));
            var numberOutOfRangePolicy = NumberOutOfRangePolicy.Create(min, max);
            var retryPolicy = Policy
                .Handle<NumberOutOfRangeException>()
                .RetryForever();
            _policies = Policy.Wrap(retryPolicy, numberOutOfRangePolicy);
        }

        public int Next(int min, int max)
        {
            if (min != _min || max != _max)
            {
                throw new NotSupportedException($"Can only generate number in the range [{_min}, {_max}]");
            }

            return _policies.Execute(() => _innerGenerator.Next(min, max));
        }
    }
}
