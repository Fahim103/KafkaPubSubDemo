using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace MessageSendAgent
{
    public class SendNotification : ISendNotification
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SendNotification(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task SendMessageAsync(SubscriptionDto subscriptionDto)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, subscriptionDto.URI);
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(JsonSerializer.Serialize(subscriptionDto));

            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                using (var response = await httpClient.SendAsync(request))
                {
                    Console.WriteLine("Successful...");
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unsuccessful {ex.Message}");
            }
        }
    }
}