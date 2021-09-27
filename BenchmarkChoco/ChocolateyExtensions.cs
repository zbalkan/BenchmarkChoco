﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chocolatey;
using chocolatey.infrastructure.results;

namespace BenchmarkChoco
{
    public static class ChocolateyExtensions
    {
        public static Task RunAsync(this GetChocolatey chocolatey)
        {
            return Task.Run(() => chocolatey.Run());
        }

        public static Task<ICollection<T>> ListAsync<T>(this GetChocolatey chocolatey)
        {
            return Task.Run(() => (ICollection<T>)chocolatey.List<T>().ToList());
        }

        public static Task<ICollection<PackageResult>> ListPackagesAsync(this GetChocolatey chocolatey)
        {
            return Task.Run(() => (ICollection<PackageResult>)chocolatey.List<PackageResult>().ToList());
        }
    }
}
