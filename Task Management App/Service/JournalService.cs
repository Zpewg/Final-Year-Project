using Microsoft.IdentityModel.Tokens;
using Task_Management_App.Entities;
using Task_Management_App.Repository;
using Task_Management_App.Validators;

namespace Task_Management_App.Service;

public class JournalService
{
    private readonly JournalRepository _journalRepository;
    private readonly JournalValidator _journalValidator;

    public JournalService(JournalRepository journalRepository, JournalValidator journalValidator)
    {
        _journalRepository = journalRepository;
        _journalValidator = journalValidator;
    }

    public async Task<string?> CreateJournal(Journal journal)
    {
        string errorMsg = await _journalValidator.ValidateJournal(journal);

        if (errorMsg.IsNullOrEmpty())
        {
            await _journalRepository.AddJournal(journal);
        }
        return errorMsg;
    }

    public async Task<string?> Updatejournal(Journal journal)
    {
        string errorMsg = await _journalValidator.ValidateJournal(journal);

        if (errorMsg.IsNullOrEmpty())
        {
            await _journalRepository.UpdateJournal(journal);
        }
        return errorMsg;
    }

    public async Task<List<Journal>> GetAllJournals(int userId)
    {
         return await _journalRepository.GetJournals(userId);
    }

    public async Task<bool> DeleteJournal(Journal journal)
    {
        
        return await _journalRepository.DeleteJournal(journal);
    }
    
}