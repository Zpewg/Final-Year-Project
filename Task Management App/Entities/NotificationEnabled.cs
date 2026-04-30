using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_Management_App.Entities;

public class NotificationEnabled
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int NotificationId { get; set; }
    
    [ForeignKey("UserId")]
    public int  UserId{ get; set; }
    
    [Column(TypeName = "bit")] 
    public bool IsEnabled { get; set; }
    
    public NotificationEnabled(){}

    public NotificationEnabled(int userId, bool isEnabled)
    {
        UserId = userId;
        IsEnabled = isEnabled;
    }

    public override string ToString()
    {
        return IsEnabled ? "Enabled" : "Disabled";
    }
}