using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using NuGet.Common;
using dotenv.net;
using Task_Management_App.Entities;
using Task_Management_App.Repository;

namespace Task_Management_App.Service;

public class MailingService
{

    private string verificationCode;

    private void GenerateVerificationCode()
    {
        var random = new Random();
        verificationCode = random.Next(100000, 999999).ToString();
    }

    public string MailToUser(string userMail)
    {
        Console.WriteLine($"Mailing to {userMail} s-a trimis mail");
        DotEnv.Load(options: new DotEnvOptions(
            envFilePaths: new[]
            {
                Path.Combine(AppContext.BaseDirectory,
                    "D:\\Licenta\\Task Management App\\Task Management App\\Service\\password.env")
            },
            overwriteExistingVars: true));
        var email = Environment.GetEnvironmentVariable("EMAIL");
        var password = Environment.GetEnvironmentVariable("PASSWORD");
     
        GenerateVerificationCode();

        
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Task Management", email));
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
                    <p>Best regards,<br/>Task Management Team</p>
                ",
            TextBody = $"Your verification code is: {verificationCode}"
        }.ToMessageBody();

      
        using (var client = new SmtpClient())
        {
            try
            {
                
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                
                client.Authenticate(email, password);

               
                client.Send(message);
                client.Disconnect(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Mail Error] {ex.Message}");
                throw;
            }
        }

        return verificationCode;
    }
}