using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration conf)
        {
            _httpClient = httpClient;
            _configuration = conf;
        }

        public async Task SendPlatformToCommand(PlatformReadDto plat)
        {
            var httpContent = new StringContent(
                    JsonSerializer.Serialize(plat),
                    Encoding.UTF8,
                    "application/json");
            var response = await _httpClient.PostAsync($"{_configuration["CommandService"]}",httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Response Succeeded. Sync POST to CommandService was OK!");
            }
            else
            {
                Console.WriteLine("-->Response Failed. Sync POST to CommandService was NOT OK!");
            }
        }
    }
}
