namespace Task_Management_App.Entities;

public class UserDTO
{
    public string Mail {get; set;}
    
    public string Name {get; set;}
    
    public string Password {get; set;}
    
    public string PhoneNumber {get; set;}

    public override string ToString()
    {
        return $"{Mail} {Name} {Password} {PhoneNumber}";
    }
}