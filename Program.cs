using BunkbedBOMRouting.Classes;
using BunkbedBOMRouting.Helpers;
using Newtonsoft.Json;



var selectedTestCase = TestCase.Bunkbed; // Change this to select different test cases

string BOMFilePath = "";
string RoutingStepsFilePath = "";

// switch case for selected TestCase, just change target file path depending on case.
switch (selectedTestCase)
{
    case TestCase.Bunkbed:
        BOMFilePath = @"bunkbed-bom.json";
        RoutingStepsFilePath = @"bunkbed-routing.json";
        break;
    case TestCase.Desk:
        BOMFilePath = @"desk-bom.json";
        RoutingStepsFilePath = @"desk-routing.json";
        break;
    case TestCase.CoffeeTable:
        BOMFilePath = @"coffee-table-bom.json";
        RoutingStepsFilePath = @"coffee-table-routing.json";
        break;
    case TestCase.Bookshelf:
        BOMFilePath = @"bookshelf-bom.json";
        RoutingStepsFilePath = @"bookshelf-routing.json";
        break;
    default:
        Console.WriteLine("Invalid test case selected.");
        return;
}
Console.WriteLine($"Selected Test Case: {selectedTestCase}\n");


//Attempt to read and deserialize BOM JSON file into a Component object.
//Early return if file is not found or deserialization fails.
FileInfo bomFile = new FileInfo(BOMFilePath);
if (!bomFile.Exists)
{
    Console.WriteLine($"BOM file not found at path: {BOMFilePath}");
    return;
}

string json = File.ReadAllText(BOMFilePath);
Component? component = JsonConvert.DeserializeObject<Component>(json);
if (component == null)
{
    Console.WriteLine("Failed to deserialize BOM JSON.");
    return;
}


Console.WriteLine($"Component: {component.Description}\n");



// Get a dictionary of provided parts and their quantities for the component,
// then write this information to a CSV file.
Dictionary<string, int> providedPartsQuantity = component.GetComponentProvidedPartsQuantities();
FileHelper.WriteToCSV(providedPartsQuantity, "Description", "Quantity");

var providedParts = component.GetComponentProvidedParts();


// Attempt to read and deserialize Routing Steps JSON file into a list of RoutingStep objects. Early return if file is not found or deserialization fails.
FileInfo routingStepsFile = new FileInfo(RoutingStepsFilePath);

if (!routingStepsFile.Exists)
{
    Console.WriteLine($"Routing Steps file not found at path: {RoutingStepsFilePath}");
    return;
}

string routingStepsJson = File.ReadAllText(RoutingStepsFilePath);
var routingSteps = JsonConvert.DeserializeObject<List<RoutingStep>>(routingStepsJson);
if (routingSteps == null)
{
    Console.WriteLine("Failed to deserialize Routing Steps JSON.");
    return;
}


// Calculate and display the total Takt Time for the routing steps.
int totalTaktTime = routingSteps.GetTotalTaktTime();
Console.WriteLine($"Total Takt Time: {totalTaktTime}\n");

// Get and display the routing steps that do not require parts based on the provided routing steps and the component's associated steps.
var stepsThatDoNotRequireParts = component.GetStepsThatDoNotRequireParts(routingSteps);

foreach (var step in stepsThatDoNotRequireParts)
{
    Console.WriteLine($"Step {step.Step} '{step.Description}' has no provided components added.");
}

/*
Console.WriteLine();

var stepsThatRequireTools = component.GetRoutingStepsThatRequireTools(routingSteps);

foreach (var step in stepsThatRequireTools)
{
    Console.WriteLine($"Step {step.Step} '{step.Description}' requires tools");
}
*/
