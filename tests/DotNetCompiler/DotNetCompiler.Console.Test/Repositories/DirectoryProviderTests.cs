using DotNetCompiler.Console.Repositories;
using NUnit.Framework;

namespace DotNetCompiler.Console.Test.Repositories
{
    public class DirectoryProviderTests
    {
        readonly DotNetDirectoryProvider _directoryProvider = new();
        [Test]
        public void ProvidedResourcePathOf_DotNetDirectoryProvider_ShouldExistsTest()
        {
            var path = _directoryProvider.GetResourcesPath();
            Assert.That(path, Does.Exist);
        }
    }
}
