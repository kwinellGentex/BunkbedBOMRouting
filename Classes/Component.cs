namespace BunkbedBOMRouting.Classes
{
    /// <summary>
    /// Component record represents a component in the Bill of Materials (BOM) with its description, quantity, associated step, source, and a list of sub-components (BOM).
    /// </summary>
    /// <param name="Description">Description of component</param>
    /// <param name="Quantity">Quantity of component</param>
    /// <param name="Step">Assembly step for component</param>
    /// <param name="Source">Source of component (e.g., Provided, Partial Item, Completed Item)</param>
    /// <param name="Bom">List of sub-components (BOM)</param>
    public record Component(string Description, int Quantity, int Step, string Source, List<Component> Bom);
    
    public static class ComponentExtensions
    {

        /// <summary>
        /// Returns a dictionary of provided parts and their total quantities for a given component, including all sub-components in the BOM. 
        /// Only components with Source "Provided" are included in the count.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> GetComponentProvidedPartsQuantities(this Component component)
        {
            if (component.Bom == null || component.Bom.Count == 0) //Assuming all "provided" components do not have a BOM associated
            {
                if (component.Source == "Provided")
                {
                    return new Dictionary<string, int> { [component.Description] = component.Quantity };
                }
                else
                {
                    return new Dictionary<string, int>();
                }
            }

            return component.Bom
                .SelectMany(sub => GetComponentProvidedPartsQuantities(sub))
                .GroupBy(kvp => kvp.Key)
                .ToDictionary(g => g.Key, g => g.Sum(kvp => kvp.Value));
        }
        
        /// <summary>
        /// Returns a dictionary of all parts and their total quantities for a given component, including all sub-components in the BOM.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public static Dictionary<string, int> GetComponentTotalPartsQuantity(this Component component)
        {
            if (component.Bom == null || component.Bom.Count == 0)
            {
                return new Dictionary<string, int> { [component.Description] = component.Quantity };
            }
            return component.Bom
                .SelectMany(sub => GetComponentTotalPartsQuantity(sub))
                .GroupBy(kvp => kvp.Key)
                .ToDictionary(g => g.Key, g => g.Sum(kvp => kvp.Value) * component.Quantity);
        }

        /// <summary>
        /// Returns a list of provided parts for a given component, including all sub-components in the BOM.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public static List<Part> GetComponentProvidedParts(this Component component)
        {
            if (component.Bom == null || component.Bom.Count == 0)
            {
                return new List<Part> { new Part { Description = component.Description, Quantity = component.Quantity } };
            }

            return component.Bom
                .SelectMany(sub => GetComponentProvidedParts(sub))
                .GroupBy(p => p.Description)
                .Select(g => new Part { Description = g.Key, Quantity = g.Sum(p => p.Quantity) })
                .ToList();
        }

        /// <summary>
        /// Returns steps associated with the given component and all its sub-components in the BOM.
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public static List<int> GetStepsAssociatedWithComponents(this Component component)
        {
            var steps = new List<int> { component.Step };
            if (component.Bom != null && component.Bom.Count > 0)
            {
                steps.AddRange(component.Bom
                        .SelectMany(sub => GetStepsAssociatedWithComponents(sub)));
            }
            return steps
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// Returns a list of routing steps that require parts for a given component, based on the provided routing steps and the parts associated with the component.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="routingSteps">List of routing steps to parse through.</param>
        /// <returns></returns>
        public static List<RoutingStep> GetStepsThatRequireParts(this Component component, IEnumerable<RoutingStep> routingSteps)
        {
            List<int> StepsAssociatedWithComponents = component.GetStepsAssociatedWithComponents();
            return routingSteps.GetRoutingSteps(StepsAssociatedWithComponents);
        }

        /// <summary>
        /// Returns a list of routing steps that do not require parts for a given component, based on the provided routing steps and the parts associated with the component.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="routingSteps">List of routing steps to parse through</param>
        /// <returns></returns>
        public static List<RoutingStep> GetStepsThatDoNotRequireParts(this Component component, IEnumerable<RoutingStep> routingSteps)
        {
            List<int> StepsAssociatedWithComponents = component.GetStepsAssociatedWithComponents();
            return routingSteps.Where(s => !StepsAssociatedWithComponents.Contains(s.Step)).ToList();
        }
    }
}
