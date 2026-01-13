namespace Task_Management_App.Entities;

public class UserDTO
{
    public int UserDTOId { get; set; }
    
    public string UserDTOName {get; set;}
    
    public string UserDTOEmail {get; set;}
    
    public string UserDTOPassword {get; set;}
    
    public string UserDTOPhoneNumber {get; set;}
    
  

    public UserDTO()
    {
        
    }

    public UserDTO(int userDtoId, string userDtoName, string userDtoEmail, string userDtoPassword, string userDtoPhoneNumber)
    {
      UserDTOId = userDtoId;
      UserDTOName = userDtoName;
      UserDTOEmail = userDtoEmail;
      UserDTOPassword = userDtoPassword;
      UserDTOPhoneNumber = userDtoPhoneNumber;
    
    }

    public override string ToString()
    {
        return $"The user dto is: {UserDTOName}";
    }
}