using System;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace DemoCustomPolicy.MyPolicies
{
    public class DuongTimeoutPolicy : AsyncPolicy
    {
        private readonly int _timeoutInMilliSecs;

        private DuongTimeoutPolicy(int timeoutInMilliSecs) => _timeoutInMilliSecs = timeoutInMilliSecs;

        // By convention, Polly configuration syntax uses static factory methods.
        public static DuongTimeoutPolicy Create(int timeoutInMilliSecs) =>
            new DuongTimeoutPolicy(timeoutInMilliSecs);

        protected override async Task<TResult> ImplementationAsync<TResult>(
            Func<Context, CancellationToken, Task<TResult>> action, Context context,
            CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            var delegateTask = action(context, cancellationToken);
            var timeoutTask = Task.Delay(_timeoutInMilliSecs);

            await Task.WhenAny(delegateTask, timeoutTask).ConfigureAwait(continueOnCapturedContext);

            if (timeoutTask.IsCompleted)
            {
                throw new DuongTimeoutException(
                    $"{context.OperationKey}: Task was cancelled after: {_timeoutInMilliSecs}ms");
            }

            return await delegateTask;
        }
    }
}
