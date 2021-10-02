using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BenchmarkChoco.App
{
    public static class Program
    {
        public static void Main()
        {
            var expectedEntries = new ChocolateyFactory().GetUninstallerEntries(null);
            CompareWithLib(expectedEntries);
            CompareSingleParsing(expectedEntries);
        }

        private static void CompareSingleParsing(IList<ApplicationUninstallerEntry> expectedEntries)
        {
            var actualEntriesParsingSingle = new ChocolateyFactoryParsingSingle().GetUninstallerEntries(null);
            CompareResults(expectedEntries, actualEntriesParsingSingle);
        }

        private static void CompareWithLib(IList<ApplicationUninstallerEntry> expectedEntries)
        {
            var actualEntriesWithLib = new ChocolateyFactoryWithLib().GetUninstallerEntries(null);
            CompareResults(expectedEntries, actualEntriesWithLib);
        }

        private static void CompareResults(IList<ApplicationUninstallerEntry> expectedEntries, IList<ApplicationUninstallerEntry> actualEntries)
        {
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
