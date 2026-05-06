namespace BunkbedBOMRouting.Classes
{
    /// <summary>
    /// Record to represent a routing step in the assembly process, containing the step number, description, and takt time.
    /// </summary>
    /// <param name="Step"></param>
    /// <param name="Description"></param>
    /// <param name="TaktTime"></param>
    public record RoutingStep(Step Step, string Description = "", int TaktTime = 0);
    
    public static class RoutingStepExtensions
    {
        /// <summary>
        /// Returns the RoutingStep that matches the provided step number, or null if no match is found.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="stepNumber"></param>
        /// <returns></returns>
        public static RoutingStep? FindByStepNumber(this IEnumerable<RoutingStep> steps, int stepNumber)
        {
            return steps.FirstOrDefault(s => s.Step == stepNumber);
        }

        /// <summary>
        /// Returns a list of RoutingSteps that match the provided step numbers.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="stepNumbers"></param>
        /// <returns></returns>
        public static List<RoutingStep> GetRoutingSteps(this IEnumerable<RoutingStep> steps, IEnumerable<Step> stepNumbers)
        {
            return steps.Where(s => stepNumbers.Contains(new Step(s.Step))).ToList();
        }

        /// <summary>
        /// Returns the total Takt Time for a collection of RoutingSteps by summing the TaktTime of each step.
        /// </summary>
        /// <param name="steps"></param>
        /// <returns></returns>
        public static int GetTotalTaktTime(this IEnumerable<RoutingStep> steps)
        {
            int totalTaktTime = steps.Sum(s => s.TaktTime);
            return totalTaktTime;
        }
    }
}
