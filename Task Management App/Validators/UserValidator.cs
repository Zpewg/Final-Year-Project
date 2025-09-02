using System.Text.RegularExpressions;
using Task_Management_App.Entities;

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
    
    public async Task<List<string>> ValidateUser(UserDTO user)
    {   
        returnMailError(user);
        returnPhoneError(user);
        returnUserNameError(user);
        returnPasswordError(user);

        return errors;
    }


    private void returnMailError(UserDTO user)
    {
        if (!regexMail.IsMatch(user.Mail))
        {
            errors.Add("Email address is not correct.");
        }
    }

    private void returnPhoneError(UserDTO user)
    {
        if (!regexPhone.IsMatch(user.PhoneNumber))
        {
            errors.Add("Phone number is not correct.");
        }
    }

    private void returnUserNameError(UserDTO user)
    {
        if (!regexUserName.IsMatch(user.Name))
        {
            errors.Add("Name is not correct.");
        }
    }

    private void returnPasswordError(UserDTO user)
    {
        if (!regexPassword.IsMatch(user.Password))
        {
            errors.Add("Password is not correct.");
        }
    }

}