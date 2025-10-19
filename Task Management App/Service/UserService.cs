using Task_Management_App.Entities;
using Task_Management_App.Validators;
using BCrypt.Net;
using System;

namespace Task_Management_App.Service;
using Task_Management_App.Repository;

public class UserService
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository ??  throw new ArgumentNullException(nameof(userRepository));
    }
    public async Task<List<string>> CreateUser(User user)
    {
        Console.WriteLine("Am ajuns in service");
        List<string> errors = new List<string>();
        
        UserValidator userValidator = new UserValidator(_userRepository);
        
        errors = await userValidator.ValidateUser(user);

        foreach (var error in errors)
        {
            Console.WriteLine(error);
        }
        if (errors.Any())
        {
            return errors;
        }
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        
        await _userRepository.AddUser(user);
        
        return errors;
        
    }
    
    
}