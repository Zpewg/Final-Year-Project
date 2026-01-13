using Task_Management_App.Repository;
using Task_Management_App.Entities;

namespace Task_Management_App.Service;


public class CodeFromUserService
{
    private readonly VerifyMessageRepository _verifyMessageRepository;

    public CodeFromUserService(VerifyMessageRepository verifyMessageRepository)
    {
        _verifyMessageRepository = verifyMessageRepository;
    }

    public async Task<bool> CheckValidCode(int userId, string code)
    {
        Console.WriteLine("Am ajuns la verificarea codului");

      
        List<VerifyMessage> messages = await _verifyMessageRepository.GetVerifyMessages(userId);

      
        foreach (var msg in messages)
        {
            bool isCodeMatch = code.Equals(msg.VerifysMessage);
        
           
            bool isNotExpired = (DateTime.UtcNow - msg.CurrentTime).TotalMinutes < 15;

            if (isCodeMatch && isNotExpired)
            {
                return true; 
            }
        }

        return false; 
    }
}