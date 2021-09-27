using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using chocolatey;
using chocolatey.infrastructure.app.domain;
using chocolatey.infrastructure.app.services;
using chocolatey.infrastructure.results;

namespace BenchmarkChoco
{
    [SimpleJob(RunStrategy.Monitoring, launchCount: 1, warmupCount: 1, targetCount: 50)]
    public class ChocoStrategy
    {
        [Benchmark]
        public void ParsingExecutableOutputStrategy()
        {
            var choco = new ChocolateyFactory();
            choco.GetUninstallerEntries(r => Console.WriteLine(r.Message));
        }

        [Benchmark]
        public void UsingLibraryStrategy()
        {
            var choco = new GetChocolatey();
            var packages =  GetUninstallerEntries(choco);
            packages.ForEach(p => Console.WriteLine(p.Title));
        }

        public IList<Package> GetUninstallerEntries(GetChocolatey choco)
        {
            return GetUninstallerEntries(choco);
        }

        public async Task<IEnumerable<Package>> GetInstalledPackages(GetChocolatey _choco)
        {
            _choco.Set(
                config =>
                {
                    config.CommandName = nameof(CommandNameType.list);
                    config.ListCommand.LocalOnly = true;
                });
            var packages = await _choco.ListAsync<PackageResult>().ConfigureAwait(false);
            return packages
                .Select(package => GetMappedPackage(_choco, package, true))
                .ToArray();
        }

        private static Package GetMappedPackage(GetChocolatey choco, PackageResult package, bool forceInstalled = false)
        {
            //var mappedPackage = package == null ? null : mapper.Map<Package>(package.Package);
            var mappedPackage = package == null ? null : MapFromPackageResult(package);
            if (mappedPackage != null)
            {
                var packageInfoService = choco.Container().GetInstance<IChocolateyPackageInformationService>();
                var packageInfo = packageInfoService.get_package_information(package.Package);
                mappedPackage.IsPinned = packageInfo.IsPinned;
                mappedPackage.IsInstalled = !string.IsNullOrWhiteSpace(package.InstallLocation) || forceInstalled;
                mappedPackage.IsSideBySide = packageInfo.IsSideBySide;

                mappedPackage.IsPrerelease = !string.IsNullOrWhiteSpace(mappedPackage.Version.SpecialVersion);

                // Add a sanity check here for pre-release packages
                // By default, pre-release packages are marked as IsLatestVersion = false, however, IsLatestVersion is
                // what is used to show/hide the Out of Date message in the UI.  In these cases, if it is a pre-release
                // mark IsLatestVersion as true, and then the outcome of the call to choco outdated will correct whether
                // it is actually Out of Date or not
                if (mappedPackage.IsPrerelease && mappedPackage.IsAbsoluteLatestVersion && !mappedPackage.IsLatestVersion)
                {
                    mappedPackage.IsLatestVersion = true;
                }
            }

            return mappedPackage;
        }

        private static Package MapFromPackageResult(PackageResult result)
        {
            var package = new Package
            {
                Title = result.Name,
                Version = new NuGet.SemanticVersion(result.Version),
                IsInstalled = result.ExitCode == 0
            };

            if (!string.IsNullOrEmpty(result.SourceUri))
            {
                package.Source = new Uri(result.SourceUri);
            }

            return package;
        }
    }
}
