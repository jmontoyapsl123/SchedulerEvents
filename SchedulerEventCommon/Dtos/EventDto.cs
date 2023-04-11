using System.Text.Json.Serialization;

namespace SchedulerEventCommon.Dtos;
public class EventDto
{
    public string EventName { get; set; }
    public string Description { get; set; }
    public DateTime EventDate { get; set; }
    public int EventType { get; set; }
    public string City { get; set; }
    [JsonIgnore]
    public string Country { get; set; }
    [JsonIgnore]
    public string Longitude { get; set; }
    [JsonIgnore]
    public string Latitud { get; set; }
    [JsonIgnore]
    public string EventHour {get; set;} 
}