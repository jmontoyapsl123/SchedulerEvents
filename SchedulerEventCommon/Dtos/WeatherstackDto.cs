namespace SchedulerEventCommon.Dtos;
public class WeatherstackDto
{
    public Request Request { get; set; }
    public Location Location { get; set; }
}

public class Location
{
    public string Name { get; set; }
    public string Country { get; set; }
    public string Lat { get; set; }
    public string Lon { get; set; }
}

public class Request
{
    public string Language { get; set; }
}