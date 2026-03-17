using Task_Management_App.Entities;
using Task_Management_App.Validators;
using BCrypt.Net;
using System;
using NetTopologySuite.Geometries;

namespace Task_Management_App.Service;
using Task_Management_App.Repository;
using MaxMind.GeoIP2;
public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly VerifyMessageService _verifyMessageService;
    
    public record IpApiResponse(string status, string country, string city, double lat, double lon);

    public UserService(UserRepository userRepository, VerifyMessageService verifyMessageService)
    {
        _userRepository = userRepository ??  throw new ArgumentNullException(nameof(userRepository));
        _verifyMessageService = verifyMessageService ??  throw new ArgumentNullException(nameof(verifyMessageService));
     
    }
    public async Task<List<string>> CreateUser(UserDTO userDto)
    {
        Console.WriteLine("Am ajuns in service");
        List<string> errors = new List<string>();
        
        UserValidator userValidator = new UserValidator(_userRepository);
        
        errors = await userValidator.ValidateUser(userDto);

        foreach (var error in errors)
        {
            Console.WriteLine(error);
        }
        if (errors.Any())
        {
            return errors;
        }

        User user = new User(userDto.UserDTOName, userDto.UserDTOEmail, userDto.UserDTOPassword, userDto.UserDTOPhoneNumber );
        
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        
        await _userRepository.AddUser(user);
        string generatedCode = await MailUser(user);
        int userId = await _userRepository.GetUserIdByEmail(user.Email);
        VerifyMessage verifyMessage = new VerifyMessage(generatedCode, userId, CodeType.ActivateAccount);
        Console.WriteLine(verifyMessage.CurrentTime);
        await _verifyMessageService.SendVerifyMessageAsync(verifyMessage);
        return errors;
        
    }

    public async Task<bool> CheckPassword(User user, string password)
    {
        if (user == null)
        {
            Console.WriteLine("User is null asdafasdfasdf");
        }
        return BCrypt.Net.BCrypt.Verify(password, user.Password);
    }

    public async Task UpdateUserPassword(int userId, string password)
    {
        string hashPassword = BCrypt.Net.BCrypt.HashPassword(password);
        
       await _userRepository.UpdateUserPassword(userId, hashPassword);
    }
    public async Task<string> MailUser(User user)
    {
        MailingService mailingService = new MailingService();
        string message = mailingService.MailToUser(user.Email);
        return message;
    }

    public async Task<string> UpdateUserLocation(User user, string ip, int km)
    {
        // Aici folosim un API extern gratuit (ex: ip-api.com) pentru a evita baze de date locale
        using var client = new HttpClient();
        var response = await client.GetFromJsonAsync<IpApiResponse>($"http://ip-api.com/json/{ip}");

        if (response != null && response.status == "success")
        {
            var point = new Point(response.lat, response.lon);
        
            // Apelezi metoda ta existentă de salvare
            await _userRepository.UpdateUserLocation(user, point, km);
        
            return $"User located in {response.city}, {response.country}.";
        }

        return "Could not determine location from IP.";
    }
    
    
}