using Domain.Entities;
using System.Collections.Generic;

namespace DotNetCompiler.Console.Repositories
{
    public interface IDotNetFrameworkProvider
    {
        public DotNetVersionInfo GetDotNetVersion(string version, string language);

        public ICollection<DotNetVersionInfo> GetDotNetVersions();

        public ICollection<string> GetLanguages();
    }
}
