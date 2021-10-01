using Newtonsoft.Json;
using System;

namespace BenchmarkChoco.App
{
    public static class Program
    {
        public static void Main()
        {
            var expectedEntries = new ChocolateyFactory().GetUninstallerEntries(null);
            var actualEntries = new ChocolateyFactoryWithLib().GetUninstallerEntries(null);

            for (var i = 0; i < expectedEntries.Count; i++)
            {
                var expectedJson = JsonConvert.SerializeObject(expectedEntries[i], Formatting.Indented);
                var actualJson = JsonConvert.SerializeObject(actualEntries[i], Formatting.Indented);
                var correct = expectedJson.Equals(actualJson, StringComparison.OrdinalIgnoreCase);
                Console.WriteLine($"{i}: {correct}");
                if (!correct)
                {
                    Console.WriteLine("Expected:");
                    Console.WriteLine(expectedJson);
                    Console.WriteLine("Actual:");
                    Console.WriteLine(actualJson);
                }
            }
        }
    }
}
