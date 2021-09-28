using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using chocolatey;
using chocolatey.infrastructure.app.domain;
using chocolatey.infrastructure.results;

namespace BenchmarkChoco
{
    public sealed class ChocolateyFactoryWithLib
    {
        private static bool? _chocoIsAvailable;

        private readonly GetChocolatey _choco;

        // KEEP: to check if Choco is available
        private static bool ChocoIsAvailable
        {
            get
            {
                if (!_chocoIsAvailable.HasValue)
                {
                    _chocoIsAvailable = false;
                    GetChocoInfo();
                }
                return _chocoIsAvailable.Value;
            }
        }

        public static string ChocoFullFilename { get; private set; }

        public ChocolateyFactoryWithLib()
        {
            _choco = Lets.GetChocolatey();
        }

        // KEEP: to check if Choco is available, no need for location
        private static void GetChocoInfo()
        {
            try
            {
                ChocoFullFilename = PathTools.GetFullPathOfExecutable("choco.exe");
                if (string.IsNullOrEmpty(ChocoFullFilename)) return;

                var result = StartProcessAndReadOutput(ChocoFullFilename, string.Empty);
                if (result.StartsWith("Chocolatey", StringComparison.Ordinal))
                {
                    _chocoIsAvailable = true;
                }
            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex);
            }
        }

        // KEEP: One and only public method. Rewrite almost all. 
        [SuppressMessage("Redundancy", "RCS1163:Unused parameter.", Justification = "Callback is used with the result, not in the method itself.")]
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Callback is used with the result, not in the method itself.")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Just stop nagging.")]
        public IList<ApplicationUninstallerEntry> GetUninstallerEntries(ListGenerationProgress.ListGenerationCallback progressCallback = null)
        {
            var results = new List<ApplicationUninstallerEntry>();
            if (!ChocoIsAvailable) return results;
            _choco.Set(
                config =>
                {
                    config.CommandName = nameof(CommandNameType.list);
                    config.ListCommand.LocalOnly = true;
                });
            foreach (var package in _choco.List<PackageResult>())
            {
                var entry = new ApplicationUninstallerEntry
                {
                    // Info in the package result
                    DisplayName = package.Name,
                    DisplayVersion = ApplicationEntryTools.CleanupDisplayVersion(package.Version),
                    InstallLocation = package.InstallLocation,
                    InstallSource = package.Source,

                    // Info in the nested IPackage
                    AboutUrl = package.Package.ProjectUrl?.AbsolutePath ?? package.Package.DocsUrl?.AbsolutePath,
                    Comment = package.Package.Description ?? package.Package.Tags,

                    // TODO
                    DisplayIcon = string.Empty,
                    EstimatedSize = FileSize.Empty,
                    IconBitmap = null,
                    InstallDate = DateTime.Today,
                    Is64Bit = MachineType.X64,
                    RatingId = $"Choco {package.Name}",
                    UninstallerKind = UninstallerType.Chocolatey
                };

                var psc = new ProcessStartCommand(ChocoFullFilename, $"uninstall {package.Name} -y -r");
                entry.UninstallString = psc.ToString();

                // Prevent chocolatey from trying to run the original uninstaller (it's deleted by now), only remove the package
                psc.Arguments += " -n --skipautouninstaller";
                var junk = new RunProcessJunk(entry, null, psc, Localisation.ChocolateyFactory_UninstallInChocolateyJunkName);
                junk.Confidence.Add(ConfidenceRecords.ExplicitConnection);
                junk.Confidence.Add(4);
                entry.AdditionalJunk.Add(junk);

                results.Add(entry);
            }
            return results;
        }

        private static string StartProcessAndReadOutput(string filename, string args)
        {
            using (var process = Process.Start(new ProcessStartInfo(filename, args)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.Default
            }))
            {
                var sw = Stopwatch.StartNew();
                var output = process?.StandardOutput.ReadToEnd();
                Console.WriteLine($"[Performance] Running command {filename} {args} took {sw.ElapsedMilliseconds}ms");
                return output;
            }
        }
    }
}
