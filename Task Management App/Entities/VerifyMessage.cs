namespace Task_Management_App.Entities;

public class VerifyMessage
{
    public string verifyMessage {get; set;}


    public VerifyMessage(string verifyMessage)
    {
        this.verifyMessage = verifyMessage;
    }

    public VerifyMessage()
    {
        
    }

    public override string ToString()
    {
        return this.verifyMessage;
    }
    
}