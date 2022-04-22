using System;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace DemoProgram
{
    public static class PolicyHelper
    {
        public static IAsyncPolicy CreateRetryOn503Policy(int retryCount, int waitInMilliSecs) =>
            Policy
                .Handle<HttpRequestException>(ex => ex.Message.Contains("503"))
                .WaitAndRetryAsync(
                    retryCount,
                    _ => TimeSpan.FromMilliseconds(waitInMilliSecs),
                    (ex, timespan, retryNo, context) =>
                    {
                        Console.WriteLine($"{context.OperationKey}: Retry number {retryNo} after " +
                            $"{timespan.TotalMilliseconds}ms. Original status code: 503");
                    }
                );

        public static IAsyncPolicy<HttpResponseMessage> CreateRetryOnErrorPolicy(int retryCount, int waitInMilliSecs) =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    retryCount,
                    _ => TimeSpan.FromMilliseconds(waitInMilliSecs),
                    (result, timespan, retryNo, context) =>
                    {
                        Console.WriteLine($"{context.OperationKey}: Retry number {retryNo} after " +
                            $"{timespan.TotalMilliseconds}ms. Original status code: {result.Result.StatusCode}");
                    }
                );

        public static IAsyncPolicy CreateRetryOnTimeoutPolicy(int retryCount, int waitInMilliSecs) =>
            Policy
                .Handle<TimeoutRejectedException>()
                .WaitAndRetryAsync(
                    retryCount,
                    _ => TimeSpan.FromMilliseconds(waitInMilliSecs),
                    (ex, timespan, retryNo, context) =>
                    {
                        Console.WriteLine($"{context.OperationKey}: Retry number {retryNo} after " +
                            $"{timespan.TotalMilliseconds}ms because {ex.Message}");
                    }
                );

        public static IAsyncPolicy CreateRequestTimeoutPolicy(int timeoutInMilliSecs) =>
            Policy.TimeoutAsync(TimeSpan.FromMilliseconds(timeoutInMilliSecs), TimeoutStrategy.Pessimistic);
    }
}
