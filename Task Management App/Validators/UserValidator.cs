using System.Text.RegularExpressions;
using Task_Management_App.Entities;
using Task_Management_App.Repository;

namespace Task_Management_App.Validators;

public class UserValidator
{
    public  Regex regexMail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                        + "@"
                                        + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
    //Only phonenumbers from romania are accepted
    public Regex regexPhone = new Regex(@"^07\d{8}$");
    
    //All characters from beggining to end must be letters or numbers
    public Regex regexUserName = new Regex("^[a-zA-Z0-9]{1,20}$");
    
    //Must contain atleast one digit, one lower case character, one upper case chcd  and one special character
    //The min length is 8 the max length is 15
    public Regex regexPassword = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,15}$");

    
    private List<string> errors = new List<string>();
    
    private readonly UserRepository _userRepository;

    public UserValidator(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public List<User> allUsers = new List<User>();
    
    public async Task<List<string>> ValidateUser(UserDTO user)
    {
        allUsers = await _userRepository.GetAllUsers();
        ReturnMailError(user);
        ReturnPhoneError(user);
        ReturnUserNameError(user);
        ReturnPasswordError(user);

        return errors;
    }


    private void ReturnMailError(UserDTO user)
    {
        if (!regexMail.IsMatch(user.UserDTOEmail) || allUsers.Where(u => u.Email.Equals(user.UserDTOEmail)).Count() > 0)
        {
            errors.Add("Email address is not correct or is already in use.");
        }
    }

    private void ReturnPhoneError(UserDTO user)
    {
        if (!regexPhone.IsMatch(user.UserDTOPhoneNumber) || allUsers.Where(u => u.PhoneNumber.Equals(user.UserDTOPhoneNumber)).Count() > 0)
        {
            errors.Add("Phone number is not correct or  is already in use.");
        }
    }

    private void ReturnUserNameError(UserDTO user)
    {
        if (!regexUserName.IsMatch(user.UserDTOName))
        {
            errors.Add("Name is not correct.");
        }
    }

    private void ReturnPasswordError(UserDTO user)
    {
        if (!regexPassword.IsMatch(user.UserDTOPassword))
        {
            errors.Add("Password is not correct.");
        }
    }

    public string PasswordIsNotCorrect(String password)
    {
      
        if (!regexPassword.IsMatch(password))
        {
            return "Password is not correct.";
        }
            
        return null;
    }
    
  
    
}