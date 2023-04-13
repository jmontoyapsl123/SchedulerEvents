namespace SchedulerEventDomain.Services.Interfaces;

public interface ISendEmailService
{
    Task SendEmailAsync(string email, string message);
}
