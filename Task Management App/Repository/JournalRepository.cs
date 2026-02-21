using Microsoft.EntityFrameworkCore;
using Task_Management_App.DB;
using Task_Management_App.Entities;

namespace Task_Management_App.Repository;

public class JournalRepository
{
    private readonly MyDBContext _context;

    public JournalRepository(MyDBContext context)
    {
        _context = context;
    }

    public JournalRepository(){}


    public async Task AddJournal(Journal journal)
    {
        //verify that journal reached repo
        journal.ToString();
        Console.WriteLine("Journal in repo");
        
        await _context.Journals.AddAsync(journal);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Journal>> GetJournals(int userId)
    {
       return await _context.Journals.Where(x => x.UserId == userId).ToListAsync();
    }

    public async Task UpdateJournal(Journal journal)
    {
        _context.Entry(journal).State = EntityState.Modified;
        
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteJournal(Journal journal)
    {
        var journalToDelete = await _context.Journals.FindAsync(journal.IdJournal);
    
        // ✅ ADĂUGAT: Verificare pentru null
        if (journalToDelete == null)
        {
            return false; // Nu l-a găsit (ID invalid sau a fost deja șters)
        }

        _context.Journals.Remove(journalToDelete);
        await _context.SaveChangesAsync();
        return true;
    }

}