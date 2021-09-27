using System.Collections.Generic;

namespace BenchmarkChoco
{
    public interface IJunkCreator
    {
        void Setup(ICollection<ApplicationUninstallerEntry> allUninstallers);
        IEnumerable<IJunkResult> FindJunk(ApplicationUninstallerEntry target);
        string CategoryName { get; }
    }
}