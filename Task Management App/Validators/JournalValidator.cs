using System.Text.RegularExpressions;
using Task_Management_App.Entities;
using Task_Management_App.Repository;

namespace Task_Management_App.Validators;

public class JournalValidator
{
    private readonly JournalRepository _journalRepository;

    public JournalValidator(JournalRepository journalRepository)
    {
        _journalRepository = journalRepository;
    }

    public async Task<string?> ValidateJournal(Journal journal)
    {
        
        //Any character max 40ch
        Regex regexJournalName = new Regex(@"^[a-zA-Z0-9!@#$%^&*()_+{}\[\]:;<>,.?\/~`=\s-]{1,40}$");

        if (!regexJournalName.IsMatch(journal.JournalName))
        {
            return  "Journal name is invalid";
        }
        
        return null;
    }
}