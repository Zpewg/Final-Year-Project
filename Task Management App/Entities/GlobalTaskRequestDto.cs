namespace Task_Management_App.Entities;

public class GlobalTaskRequestDto
{
    public UserTasksGlobal Task { get; set; }
    public User User { get; set; }
}