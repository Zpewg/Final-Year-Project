using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_Management_App.Entities;

public class TaskSuggestion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TaskSuggestionId { get; set; }
    
    [ForeignKey("UserId")]
    public int  UserId{ get; set; }
    
    [Column(TypeName = "bit")] 
    public bool IsEnabled { get; set; }
    
    public TaskSuggestion(){}

    public TaskSuggestion(int userId, bool isEnabled)
    {
        UserId = userId;
        IsEnabled = isEnabled;
    }

    public override string ToString()
    {
        return IsEnabled ? "Enabled" : "Disabled";
    }
}