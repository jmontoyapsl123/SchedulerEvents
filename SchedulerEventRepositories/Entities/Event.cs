using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchedulerEventRepositories.Entities;
public class Event
{
    [Key]
    [Column(Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string EventName { get; set; }
    public string Description { get; set; }
    public DateTime EventDate { get; set; }
    public int EventType { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string Longitude { get; set; }
    public string Latitud { get; set; }
}