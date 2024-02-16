namespace DotNetCompiler.Console.Repositories
{
    public interface IDirectoryProvider
    {
        public string GetResourcesPath();

        public string GetLanguagesPath();

        public string GetLanguagePath(string language);
    }
}
