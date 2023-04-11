
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using SchedulerEventCommon.Dtos;
using SchedulerEventDomain.Services.Interfaces;
using System.Text;

namespace SchedulerEventDomain.Services.Implementations;

public class WeatherstackService : IWeatherstackService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _client;

    public WeatherstackService(IConfiguration configuration)
    {
        _configuration = configuration;
        _client = new HttpClient();
    }

    public async Task<WeatherstackDto> GetCityInfo(string city)
    {
        var url = $"http://api.weatherstack.com/current?access_key={_configuration.GetSection("Weatherstack:ApiKey").Value}&query={city}";
        var result = await _client.GetAsync(url);
        if (!result.IsSuccessStatusCode)
        {
            throw new Exception("Error calling weatherstack api.");
        }

        var jsonResult = await result.Content.ReadAsStringAsync();
        var memorystream = new MemoryStream(Encoding.UTF8.GetBytes(jsonResult));
        var data = await JsonSerializer.DeserializeAsync<WeatherstackDto>(memorystream, new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});


        return data;
    }
}
