using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DemoProgram
{
    public class ApiClient
    {
        private readonly HttpClient _client;
        private readonly Uri _baseUri;

        public ApiClient(HttpClient client, Uri baseUri)
        {
            _client = client;
            _baseUri = baseUri;
        }

        public async Task<HttpResponseMessage> SendToDelayEndpoint(Guid requestId, int delayInMilliSecs)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, new Uri(_baseUri, $"delay/{requestId}/{delayInMilliSecs}"));
            return await _client.SendAsync(request);
        }

        public async Task<HttpResponseMessage> SendToErrorEndpoint(Guid requestId, int errorCode)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, new Uri(_baseUri, $"error/{requestId}/{errorCode}"));
            return await _client.SendAsync(request);
        }

        public async Task ResetDelay(Guid requestId) => await Reset("reset_delay", requestId);

        public async Task ResetError(Guid requestId) => await Reset("reset_error", requestId);

        private async Task Reset(string path, Guid requestId)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Post, new Uri(_baseUri, $"{path}/{requestId}"));

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}
