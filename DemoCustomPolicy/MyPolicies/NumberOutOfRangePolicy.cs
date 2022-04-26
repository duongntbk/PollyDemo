using System;
using System.Threading;
using Polly;

namespace DemoCustomPolicy.MyPolicies
{
    public class NumberOutOfRangePolicy : Policy
    {
        private readonly int _min;
        private readonly int _max;

        private NumberOutOfRangePolicy(int min, int max)
        {
            _min = min;
            _max = max;
        }

        // By convention, Polly configuration syntax uses static factory methods.
        public static NumberOutOfRangePolicy Create(int min, int max) =>
            new NumberOutOfRangePolicy(min, max);

        protected override TResult Implementation<TResult>(Func<Context, CancellationToken, TResult> action,
            Context context, CancellationToken cancellationToken)
        {
            if (typeof(TResult) != typeof(int))
            {
                throw new NotSupportedException("This policy can only handle delegate that returns an integer.");
            }

            var num = action(context, cancellationToken);
            if (num as int? < _min || num as int? > _max)
            {
                throw new NumberOutOfRangeException();
            }

            return num;
        }
    }
}
