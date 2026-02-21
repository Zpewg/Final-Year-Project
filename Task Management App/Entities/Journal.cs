using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_Management_App.Entities;

public class Journal
{
    
    [Key]
    public int IdJournal { get; set; }
    
    [ForeignKey("UserId")]
    public int UserId { get; set; }
    
    [Column(TypeName = "varchar(40)")]
    public string JournalName { get; set; }
    
    public string JournalText { get; set; }

    public Journal(int userId, string journalName, string journalText)
    {
        UserId = userId;
        JournalName = journalName;
        JournalText = journalText;
    }

    public override string ToString()
    {
        return $"Journal Name: {JournalName}\n Journal Text: {JournalText}";
    }
    
}