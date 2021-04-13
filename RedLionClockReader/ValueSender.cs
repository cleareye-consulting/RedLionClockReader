using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ClearEye.RedLionClockReader
{
    public class ValueSender : IValueSender
    {

        private readonly string deviceId;
        private readonly string endpoint;
        private readonly HttpClient httpClient;

        public ValueSender(string deviceId, string endpoint)
        {
            this.deviceId = deviceId;
            this.endpoint = endpoint;
            httpClient = new HttpClient();
        }

        public async Task SendValue(decimal value)
        {
            var requestData = JsonConvert.SerializeObject(new { deviceId = deviceId, value = value });
            var response = await httpClient.PostAsync(endpoint, new StringContent(requestData, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }


        public void Dispose()
        {

            httpClient.Dispose();
        }
    }
}