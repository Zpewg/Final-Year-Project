namespace Task_Management_App.Entities;

public class LogIn
{
    public string Mail {get; set;}
    public string Password {get; set;}
    
    
    public LogIn(){}

    public LogIn(string mail, string password)
    {
        Mail = mail;
        Password = password;
    }
}