using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_Management_App.Entities;


public class UserTasks
{
    [Key]
    public int UserTaskId { get; set; }
    
    [ForeignKey("UserId")]
    public int UserId { get; set; }
    
    public string? Description {get; set;}
    
    [Required]
    public DateOnly Date { get; set; }
    
    [Required]
    public TimeOnly Time { get; set; }
    
    [Column(TypeName = "varchar(40)")]
    [Required]
    public string NameOfTask { get; set; }

    [NotMapped]
    public string DueDateFormat
    {
        get => Date.ToString("yyyy/MM/dd");
        set
        {
            if (DateOnly.TryParse(value, out DateOnly parsedDate))
            {
                Date = parsedDate;
            }
        }
    }

    [NotMapped]
    public string DueTimeFormat
    {
        get => Time.ToString("HH:mm");
        set
        {
            if (TimeOnly.TryParse(value, out TimeOnly parsedTime))
            {
                Time  = parsedTime;
            }
        }
    }

    public UserTasks(int UserTaskId, int UserId, string Description, DateOnly Date, TimeOnly Time, string NameOfTask)
    {
        this.UserTaskId = UserTaskId;
        this.UserId = UserId;
        this.Description = Description;
        this.Date = Date;
        this.Time  = Time;
        this.NameOfTask = NameOfTask;
    }
    
    public UserTasks(int UserTaskId, int UserId, DateOnly Date, TimeOnly Time, string NameOfTask)
    {
        this.UserTaskId = UserTaskId;
        this.UserId = UserId;
        this.Date = Date;
        this.Time  = Time;
        this.NameOfTask = NameOfTask;
    }

    public override string ToString()
    {
        return $"Task Id {UserTaskId}\n Task Date {Date}\n Task Time {Time} \n Task Name {NameOfTask}";
    }
}