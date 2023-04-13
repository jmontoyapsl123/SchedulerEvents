using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchedulerEventRepositories.Entities;
public class EventInvitation
{
    [Key]
    [Column(Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int State { get; set; }
    public int EventId { get; set; }
    public int DeveloperId { get; set; }
    public string HasInvitation {get; set;}
}