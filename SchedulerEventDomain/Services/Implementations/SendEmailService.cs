
using Microsoft.Extensions.Configuration;
using SchedulerEventDomain.Services.Interfaces;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SchedulerEventDomain.Services.Implementations;

public class SendEmailService : ISendEmailService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _client;
    private readonly ILogger<SendEmailService> _iLogger;
    public SendEmailService(IConfiguration configuration, ILogger<SendEmailService> iLogger)
    {
        _configuration = configuration;
        _client = new HttpClient();
        _iLogger = iLogger;
    }

    public async Task SendEmailAsync(string email, string message)
    {
        string sendGridApiKey = _configuration.GetSection("SendGrid:ApiKey").Value;
        if (string.IsNullOrEmpty(sendGridApiKey))
        {
            _iLogger.LogError("The 'SendGridApiKey' is not configured");
        }

        var client = new SendGridClient(sendGridApiKey);
        var toEmail = new EmailAddress(email);
        var fromEmail = new EmailAddress(_configuration.GetSection("SendGrid:From").Value);
        var subject = "Invitation to new Envent";
        var emailMessage = MailHelper.CreateSingleEmail(fromEmail, toEmail, subject, null, message);
        var response = await client.SendEmailAsync(emailMessage);
        if (!response.IsSuccessStatusCode)
        {
            _iLogger.LogError($"Error sending the email: {response.StatusCode}");
        }
    }
}
