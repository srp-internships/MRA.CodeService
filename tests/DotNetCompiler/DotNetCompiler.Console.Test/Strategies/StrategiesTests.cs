using DotNetCompiler.Console.Services.CSharp;
using DotNetCompiler.Console.Services.DotNetCompiler;
using DotNetCompiler.Console.Strategies;
using NUnit.Framework;
using System.Collections.Generic;

namespace DotNetCompiler.Console.Test.Factories
{
    public class StrategiesTests : TestBase
    {
        [Test]
        public void BeAble_ToGetCSharpCompilerService_FromResolverTest()
        {
            var dotNetCompilerResolver = GetDotNetCodeCompilerStrategy();
            var version = GetCSharpNet6Version();
            var cSharpCompilerService = dotNetCompilerResolver.GetDotNetCompilerService(version);
            Assert.That(cSharpCompilerService, Is.Not.Null);
            Assert.That(cSharpCompilerService.GetType(), Is.EqualTo(typeof(CSharpCodeCompilerService)));
        }

        [Test]
        public void BeAble_ToGetCSharpCodeanalyzerService_FromResolverTest()
        {
            var cSharpCodeAnalyzerServiceMock =
                new CSharpCodeAnalyzerService(_dotNetFrameworkProvider, GetDotNetCodeCompilerStrategy());
            var dotNetCodeAnalyzerResolver =
                new DotNetCodeAnalyzerResolver(new List<IDotNetCodeAnalyzerConsoleService>()
                    { cSharpCodeAnalyzerServiceMock });
            var version = GetCSharpNet6Version();
            var dotNetCodeAnalyzerService = dotNetCodeAnalyzerResolver.GetDotNetCodeAnalyzerService(version);
            Assert.That(dotNetCodeAnalyzerService, Is.Not.Null);
            Assert.That(dotNetCodeAnalyzerService.GetType(), Is.EqualTo(typeof(CSharpCodeAnalyzerService)));
        }

        DotNetCompilerResolver GetDotNetCodeCompilerStrategy()
        {
            return new DotNetCompilerResolver(new List<IDotNetCompilerService>() { _compilerService });
        }
    }
}