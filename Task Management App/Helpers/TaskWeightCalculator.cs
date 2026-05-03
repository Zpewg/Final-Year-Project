namespace Task_Management_App.Helpers;

public static class TaskWeightCalculator
{
    public static double Calculate(int? difficulty, int? urgency, double? length)
    {
        int d = difficulty ?? 3;
        int u = urgency ?? 3;
        double l = length ?? 1.0;

        double weightDifficulty = 0.65;
        double weightUrgency = 0.35;
        double contextSwitchPenalty = 1.5;

        double baseIntensity = (weightDifficulty * d) + (weightUrgency * u);
        double totalWeight = (baseIntensity * l) + contextSwitchPenalty;

        return Math.Round(totalWeight, 1);
    }
}