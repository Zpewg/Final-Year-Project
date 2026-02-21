using Microsoft.EntityFrameworkCore;
using Task_Management_App.DB;
using Task_Management_App.Entities;

namespace Task_Management_App.Repository;

public class VerifyMessageRepository
{
    private readonly MyDBContext _context;

    public VerifyMessageRepository(MyDBContext context)
    {
        _context = context;
    }
    
    public VerifyMessageRepository(){}

    public async Task AddVerifyMessage(VerifyMessage verifyMessage)
    {
        Console.WriteLine("Mesajul este in repository");
        await _context.VerifyMessages.AddAsync(verifyMessage);
        await _context.SaveChangesAsync();
    }

    public async Task<List<VerifyMessage>> GetVerifyMessages(int userId)
    {
        // Returns a list of all messages associated with this user
        return await _context.VerifyMessages
            .Where(v => v.UsersId == userId)
            .ToListAsync();
    }

    public async Task RemoveExpiredMessagesAsync(CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Pasul 3");
        var threshold = DateTime.UtcNow.AddMinutes(-15);

        await _context.VerifyMessages
            .Where(v => v.CurrentTime < threshold)
            .ExecuteDeleteAsync(cancellationToken);
    }
    
    
}