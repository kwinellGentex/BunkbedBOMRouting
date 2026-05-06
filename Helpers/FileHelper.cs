namespace BunkbedBOMRouting.Helpers
{
    public static class FileHelper
    {
        public static void WriteToCSV(string filePath, 
                                      List<object> objList)
        {
            if (objList == null || objList.Count == 0)
            {
                Console.WriteLine("No data to write to CSV.");
                return;
            }

            var properties = objList[0].GetType().GetProperties();
            var lines = new List<string>();

            for (int i = 0; i < properties.Length; i++)
            {
                lines.Add(properties[i].Name);
            }

            foreach (var obj in objList)
            {
                var values = properties.Select(p => p.GetValue(obj, null)?.ToString() ?? "");
                lines.Add(string.Join(",", values));
            }

            System.IO.File.WriteAllLines(filePath, lines);
        }
        
        public static void WriteToCSV(Dictionary<string, int> dict, string keyName, string valueName)
        {
            var lines = new List<string>
            {
                $"{keyName},{valueName}"
            };

            foreach (var kvp in dict)
            {
                lines.Add($"{kvp.Key},{kvp.Value}");
            }

            // Get the project directory (3 levels up from bin/Debug/net8.0)
            string projectDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
            string outputPath = Path.Combine(projectDirectory, "output.csv");
            
            System.IO.File.WriteAllLines(outputPath, lines);
        }
    }
}
