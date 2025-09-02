using Task_Management_App.Entities;
using Task_Management_App.Validators;

namespace Task_Management_App.Service;
using Task_Management_App.Repository;

public class UserService
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository ??  throw new ArgumentNullException(nameof(userRepository));
    }
    public async Task<List<string>> CreateUser(UserDTO user)
    {
        Console.WriteLine("Am ajuns in service");
        List<string> errors = new List<string>();
        
        UserValidator userValidator = new UserValidator();
        
        errors = await userValidator.ValidateUser(user);

        if (errors.Any())
        {
            return errors;
        }
        
        User createdUser = new User(user.Name, user.Mail, user.Password, user.PhoneNumber );
      
        await _userRepository.AddUser(createdUser);
        
        return errors;
        
    }
    
    
}