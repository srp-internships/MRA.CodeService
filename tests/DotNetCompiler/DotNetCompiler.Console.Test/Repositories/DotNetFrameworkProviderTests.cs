using DotNetCompiler.Console.Repositories;
using NUnit.Framework;
using System.IO;
using System.Linq;

namespace DotNetCompiler.Console.Test.Repositories
{
    public class DotNetFrameworkProviderTests
    {
        DotNetFrameworkProvider _dotNetFrameworkProvider;

        [SetUp]
        public void SetUp()
        {
            var directoryProvider = new DotNetDirectoryProvider();
            _dotNetFrameworkProvider = new DotNetFrameworkProvider(directoryProvider);
        }

        [Test]
        public void GetLanguages_ShouldContainsCSharp_Test()
        {
            var languages = _dotNetFrameworkProvider.GetLanguages();
            Assert.That(languages, Is.Not.Empty);

            var csharp = languages.FirstOrDefault(s => s == "CSharp");
            Assert.That(csharp, Is.Not.Null, string.Join(',', languages));
        }

        [Test]
        public void GetDotNetVersion_ShouldContainsCSharpDotNet8Version_Test()
        {
            var netSixVersion = _dotNetFrameworkProvider.GetDotNetVersion("NET8", "CSharp");
            Assert.That(netSixVersion, Is.Not.Null);
        }

        [Test]
        public void AvailableMetaDataPath_ShouldExistsInEveryVersion_Test()
        {
            var allVersions = _dotNetFrameworkProvider.GetDotNetVersions();
            foreach (var version in allVersions)
            {
                Assert.That(version.AvailableMetaData.All(s => File.Exists(s)), Is.True);
            }
        }
    }
}
