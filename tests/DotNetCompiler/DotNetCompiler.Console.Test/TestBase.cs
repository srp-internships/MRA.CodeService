using Application.DotNetCodeAnalyzer.DTO;
using Core.Exceptions;
using Domain.Entities;
using DotNetCompiler.Console.Repositories;
using DotNetCompiler.Console.Services.CSharp;
using DotNetCompiler.Console.Services.DotNetCompiler;
using NUnit.Framework;
using Shared.Test;

namespace DotNetCompiler.Console.Test
{
    public class TestBase : DotNetCodeTestBase
    {
        internal CSharpCodeCompilerService _compilerService;
        protected IDotNetFrameworkProvider _dotNetFrameworkProvider;
        protected IDirectoryProvider _dotNetDirectoryProvider;

        [SetUp]
        public void SetUp()
        {
            InitTest();
        }

        protected virtual void InitTest()
        {
            _dotNetDirectoryProvider = new DotNetDirectoryProvider();
            _dotNetFrameworkProvider = new DotNetFrameworkProvider(_dotNetDirectoryProvider);
            _compilerService = new CSharpCodeCompilerService(_dotNetFrameworkProvider);
        }

        protected CompileOutput CompileUsingCSharpAndDotNetSix(params string[] code)
        {
            return _compilerService.CompileTexts(code);
        }

        protected DotNetInfoDTO GetCSharpNet6VersionDTO()
        {
            return new DotNetInfoDTO() { Language = Constants.CSHARP_LANGUAGE, Version = Constants.DOTNET_EIGHT_VERSION };
        }

        protected DotNetInfoDTO GetCSharpNet8VersionDTO()
        {
            return new DotNetInfoDTO()
                { Language = Constants.CSHARP_LANGUAGE, Version = Constants.DOTNET_EIGHT_VERSION };
        }

        protected DotNetVersionInfo GetCSharpNet6Version()
        {
            return new DotNetVersionInfo()
                { Language = Constants.CSHARP_LANGUAGE, Version = Constants.DOTNET_EIGHT_VERSION };
        }
    }
}