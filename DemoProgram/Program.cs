using System;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;

namespace DemoProgram
{
    class Program
    {
        private static ApiClient _client = SetupApiClient();

        static async Task Main(string[] args)
        {
            try
            {
                await Retry503();
                await RetryError();
                await RetryDelay();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unhandle exception occurred: {ex.Message}");
                Console.WriteLine(ex.GetType().FullName);
            }
            Console.WriteLine("Press any key...");
            Console.ReadLine();
        }

        private static async Task Retry503()
        {
            var requestId = new Guid("be21236d-6eb7-4a58-8208-56ac1dee4207");
            var errorCode = 503;
            await _client.ResetError(requestId);

            var policy = PolicyHelper.CreateRetryOn503Policy(2, 503);

            var pollyContext = new Context(nameof(Retry503));
            var response = await policy.ExecuteAsync(async ctx =>
            {
                var response = await _client.SendToErrorEndpoint(requestId, errorCode);
                response.EnsureSuccessStatusCode();
                return response;
            }, pollyContext);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        private static async Task RetryError()
        {
            var requestId = new Guid("be21236d-6eb7-4a58-8208-56ac1dee4207");
            var errorCode = 503;
            await _client.ResetError(requestId);

            var retryPolicy = PolicyHelper.CreateRetryOnErrorPolicy(2, 500);

            var pollyContext = new Context(nameof(RetryError));
            var response = await retryPolicy.ExecuteAsync(async ctx =>
                await _client.SendToErrorEndpoint(requestId, errorCode), pollyContext);

            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        private static async Task RetryDelay()
        {
            var requestId = new Guid("be21236d-6eb7-4a58-8208-56ac1dee4207");
            var delayInMilliSecs = 2000;
            await _client.ResetDelay(requestId);

            var timeoutPolicy = PolicyHelper.CreateRequestTimeoutPolicy(200);
            var retryPolicy = PolicyHelper.CreateRetryOnTimeoutPolicy(2, 500);
            var wrapper = Policy.WrapAsync(retryPolicy, timeoutPolicy);

            var pollyContext = new Context(nameof(RetryDelay));
            var response = await wrapper.ExecuteAsync(async ctx =>
                await _client.SendToDelayEndpoint(requestId, delayInMilliSecs), pollyContext);

            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        private static ApiClient SetupApiClient()
        {
            var baseUri = new Uri("https://localhost:5001/demo/");
            var httpClient = new HttpClient();
            return new ApiClient(httpClient, baseUri);
        }
    }
}
