using Task_Management_App.Entities;
using Task_Management_App.Helpers;

namespace Task_Management_App.Service;

public class ScheduleOptimizerService
{
    private const double BurnoutThreshold = 45.0;
    private const double IdealDailyLoad = 25.0;

    public List<UserTasks> OptimizeWeek(List<UserTasks> currentTasks)
    {
        // 1. Grupăm task-urile pe zile
        var dailySchedule = currentTasks
            .GroupBy(t => t.Date)
            .ToDictionary(g => g.Key, g => g.ToList());

        var overloadedDays = dailySchedule
            .Where(kv => kv.Value.Sum(t => t.taskWeight ?? 0) > BurnoutThreshold)
            .OrderByDescending(kv => kv.Value.Sum(t => t.taskWeight ?? 0))
            .ToList();

        List<UserTasks> suggestions = new List<UserTasks>();

        foreach (var day in overloadedDays)
        {
            var tasksToMove = day.Value
                .Where(t => t.taskUrgency < 4) // Nu mutăm chestiile ultra-urgente
                .OrderBy(t => t.taskWeight)
                .ToList();

            foreach (var task in tasksToMove)
            {
                // Căutăm o zi "mai liberă" în viitorul apropiat (următoarele 3 zile)
                var betterDay = FindBetterDay(dailySchedule, task.Date);

                if (betterDay != null)
                {
                    // Creăm o "fantomă" a task-ului pe noua dată
                    var suggestedTask = CloneTask(task);
                    suggestedTask.Date = betterDay.Value;
                    // Opțional: ajustăm ora pentru a nu se suprapune
                    
                    suggestions.Add(suggestedTask);
                    
                    // Actualizăm modelul local pentru următoarea iterație
                    MoveTaskLocally(dailySchedule, task, betterDay.Value);
                }
            }
        }

        return suggestions;
    }

    private DateOnly? FindBetterDay(Dictionary<DateOnly, List<UserTasks>> schedule, DateOnly currentDate)
    {
        for (int i = 1; i <= 3; i++)
        {
            var nextDay = currentDate.AddDays(i);
            var load = schedule.ContainsKey(nextDay) ? schedule[nextDay].Sum(t => t.taskWeight ?? 0) : 0;
            
            if (load < IdealDailyLoad) return nextDay;
        }
        return null;
    }

    private UserTasks CloneTask(UserTasks t)
    {
        var newTask = new UserTasks {
            UserTaskId = t.UserTaskId, // ID-ul e vital pentru a face Update mai târziu
            UserId = t.UserId,
            NameOfTask = t.NameOfTask + " (Suggested)",
            Description = t.Description,
            Date = t.Date,
            Time = t.Time,
            taskDifficulty = t.taskDifficulty,
            taskUrgency = t.taskUrgency,
            taskLength = t.taskLength
        };
        
        newTask.taskWeight = TaskWeightCalculator.Calculate(
            newTask.taskDifficulty, 
            newTask.taskUrgency, 
            newTask.taskLength
        );

        return newTask;
    }
    private void MoveTaskLocally(Dictionary<DateOnly, List<UserTasks>> schedule, UserTasks task, DateOnly targetDate)
    {
       
        if (schedule.ContainsKey(task.Date))
        {
            schedule[task.Date].Remove(task);
        }

        
        if (!schedule.ContainsKey(targetDate))
        {
            schedule[targetDate] = new List<UserTasks>();
        }

       
        schedule[targetDate].Add(task);
        
        task.Date = targetDate;
    }
}