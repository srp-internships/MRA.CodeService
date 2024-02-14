using System.IO;

namespace DotNetCompiler.Console.Repositories
{
    internal class DotNetDirectoryProvider : IDirectoryProvider
    {
        const string repositoryResourceFolder = "Resources";

        public string GetLanguagesPath()
        {
            return Path.Combine(GetResourcesPath(), "Languages");
        }

        public string GetLanguagePath(string language)
        {
            return Path.Combine(GetLanguagesPath(), language);
        }

        public string GetResourcesPath()
        {
            return Path.Combine(Path.GetDirectoryName(typeof(DotNetDirectoryProvider).Assembly.Location), repositoryResourceFolder);
        }
    }
}
