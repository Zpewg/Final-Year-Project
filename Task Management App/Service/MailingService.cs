using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Task_Management_App.Entities;


namespace Task_Management_App.Service;

public class MailingService
{
    private string verificationCode;

    private readonly ILogger<MailingService> _logger;
    
    
    public MailingService(){}
    private void GenerateVerificationCode()
    {
        var random = new Random();
        verificationCode = random.Next(100000, 999999).ToString();
    }

    // Your existing verification method
    public string MailToUser(string userMail)
    {
        var email = Environment.GetEnvironmentVariable("EMAIL");
        var password = Environment.GetEnvironmentVariable("PASSWORD");
     
       
        GenerateVerificationCode();
        
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("TaskManagementApp", email));
        message.To.Add(new MailboxAddress("", userMail));
        message.Subject = "Your Task Management Verification Code";

        message.Body = new BodyBuilder
        {
            HtmlBody = $@"
                    <h2>Task Management Verification</h2>
                    <p>Hello,</p>
                    <p>Your verification code is:</p>
                    <h1 style='color:#4CAF50;'>{verificationCode}</h1>
                    <p>Enter this code in the app to complete your verification.</p>
                    <br/>
                    <p>Best regards,<br/>Task Management Team</p>",
            TextBody = $"Your verification code is: {verificationCode}"
        }.ToMessageBody();

        SendEmail(message, email, password);
        return verificationCode;
    }

    // NEW METHOD: Send Task Reminder
    public async Task SendTaskReminderEmail(string userMail, string taskName, string description)
    {
        
        var email = Environment.GetEnvironmentVariable("EMAIL");
        var password = Environment.GetEnvironmentVariable("PASSWORD");

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Task Management App", email));
        message.To.Add(new MailboxAddress("", userMail));
        message.Subject = $"Reminder: {taskName}";

        message.Body = new BodyBuilder
        {
            HtmlBody = $@"
                <div style='font-family: Arial, sans-serif; border: 1px solid #ddd; padding: 20px; border-radius: 10px;'>
                    <h2 style='color: #2196F3;'>Task Reminder</h2>
                    <p>Hello,</p>
                    <p>This is a reminder for your upcoming task:</p>
                    <div style='background-color: #f9f9f9; padding: 15px; border-left: 5px solid #2196F3;'>
                        <strong style='font-size: 1.2em;'>{taskName}</strong>
                        <p style='color: #555;'>{description ?? "No description provided."}</p>
                    </div>
                    <p>Check your app for more details.</p>
                    <br/>
                    <p>Best regards,<br/><strong>Task Management Team</strong></p>
                </div>",
            TextBody = $"Reminder: {taskName}. Description: {description ?? "No description provided."}"
        }.ToMessageBody();

        await SendEmailAsync(message, email, password);
    }

    // Helper method to avoid code duplication
    private async Task SendEmailAsync(MimeMessage message, string email, string password)
    {
        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(email, password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                Console.WriteLine($"[Email Sent] Reminder sent to {message.To}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Mail Error] {ex.Message}");
            }
        }
    }

    // Overload for the synchronous existing method
    private void SendEmail(MimeMessage message, string email, string password)
    {
        using (var client = new SmtpClient())
        {
            client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            client.Authenticate(email, password);
            client.Send(message);
            client.Disconnect(true);
        }
    }
}