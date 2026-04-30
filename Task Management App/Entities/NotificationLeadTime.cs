using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Task_Management_App.Entities;

public class NotificationLeadTime
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int NotificationLeadTimeId { get; set; }
    
    [ForeignKey("NotificationId")]
    public int  NotificationId { get; set; }
    
    [Column(TypeName = "int")]
    public int NotificationTime { get; set; }
    
    public NotificationLeadTime(){}

    public NotificationLeadTime(int notificationId, int notificationTime)
    {
        NotificationTime = notificationTime;
        NotificationId = notificationId;
    }

    public override string ToString()
    {
        return "NotificationLeadTime: " + NotificationTime.ToString();
    }
}