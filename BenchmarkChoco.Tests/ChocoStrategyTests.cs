using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BenchmarkChoco.Tests
{
    [TestClass]
    public class ChocoStrategyTests
    {
        public List<ApplicationUninstallerEntry> ExpectedEntries { get; set; }
        public List<ApplicationUninstallerEntry> ActualEntries { get; set; }


        [TestInitialize]
        public void Initialize()
        {
            var originalFactory = new ChocolateyFactory();
            var suggestedFactory = new ChocolateyFactoryWithLib();

            ExpectedEntries = (List<ApplicationUninstallerEntry>) originalFactory.GetUninstallerEntries(null);

            ActualEntries = (List<ApplicationUninstallerEntry>) suggestedFactory.GetUninstallerEntries(null);
        }


        [TestMethod]
        public void MethodsReturnTheSameNumberOfPackages()
        {
            Assert.AreEqual(ExpectedEntries.Count, ActualEntries.Count);
        }

        [TestMethod]
        public void MethodsReturnTheSameEntriesGivenOne()
        {
            Assert.IsTrue(Equals(ExpectedEntries[0], ActualEntries[0]));
        }

        private static bool Equals(ApplicationUninstallerEntry entry1, ApplicationUninstallerEntry entry2)
        {
            if (entry1.DisplayName != entry2.DisplayName) return false;
            if (entry1.DisplayVersion != entry2.DisplayVersion) return false;
            if (entry1.AboutUrl != entry2.AboutUrl) return false;
            if (entry1.BundleProviderKey != entry2.BundleProviderKey) return false;
            if (entry1.Comment != entry2.Comment) return false;
            if (entry1.DisplayIcon != entry2.DisplayIcon) return false;
            if (entry1.EstimatedSize != entry2.EstimatedSize) return false;
            if (entry1.HasStartups != entry2.HasStartups) return false;
            if (entry1.InstallDate != entry2.InstallDate) return false;
            if (entry1.InstallLocation != entry2.InstallLocation) return false;
            if (entry1.InstallSource != entry2.InstallSource) return false;
            if (entry1.Is64Bit != entry2.Is64Bit) return false;
            if (entry1.IsOrphaned != entry2.IsOrphaned) return false;
            if (entry1.IsProtected != entry2.IsProtected) return false;
            if (entry1.IsRegistered != entry2.IsRegistered) return false;
            if (entry1.IsValid != entry2.IsValid) return false;
            if (entry1.IsWebBrowser != entry2.IsWebBrowser) return false;
            if (entry1.ModifyPath != entry2.ModifyPath) return false;
            if (entry1.ParentKeyName != entry2.ParentKeyName) return false;
            if (entry1.Publisher != entry2.Publisher) return false;
            if (entry1.QuietUninstallPossible != entry2.QuietUninstallPossible) return false;
            if (entry1.QuietUninstallString != entry2.QuietUninstallString) return false;
            if (entry1.RatingId != entry2.RatingId) return false;
            if (entry1.RegistryKeyName != entry2.RegistryKeyName) return false;
            if (entry1.RegistryPath != entry2.RegistryPath) return false;
            if (entry1.StartupEntries != entry2.StartupEntries) return false;
            if (entry1.SystemComponent != entry2.SystemComponent) return false;
            if (entry1.UninstallerFullFilename != entry2.UninstallerFullFilename) return false;
            if (entry1.UninstallerKind != entry2.UninstallerKind) return false;
            if (entry1.UninstallPossible  != entry2.UninstallPossible) return false;
            if (entry1.UninstallString != entry2.UninstallString) return false;
            if (entry1.ToLongString() != entry2.ToLongString()) return false;
            if (entry1.ToString() != entry2.ToString()) return false;

            return true;
        }
    }
}
