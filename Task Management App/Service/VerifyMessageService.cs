using Task_Management_App.Entities;
using Task_Management_App.Repository;

namespace Task_Management_App.Service;

public class VerifyMessageService
{
    private readonly VerifyMessageRepository _verifyMessageRepository;

    public VerifyMessageService(VerifyMessageRepository verifyMessageRepository)
    {
        _verifyMessageRepository = verifyMessageRepository;
    }

    public async Task SendVerifyMessageAsync(VerifyMessage verifyMessage)
    {
        await _verifyMessageRepository.AddVerifyMessage(verifyMessage);
    }
}