namespace Task_Management_App.Entities;

public class CodeFromUser
{
    public string Code { get; set; }
    
    public string Mail { get; set; }
    
    public int? UserId { get; set; }

   public CodeFromUser(){}

    public CodeFromUser(string code, string mail)
    {
        Code = code;
        Mail = mail;
    }
}