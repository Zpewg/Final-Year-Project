namespace Task_Management_App.Entities;

public class NewPasswordFromUser
{
    public string UserEmail { get; set; }
    
    public string NewPassword { get; set; }

    public NewPasswordFromUser(string userEmail, string newPassword)
    {
        UserEmail = userEmail;
        NewPassword = newPassword;
    }
    
}