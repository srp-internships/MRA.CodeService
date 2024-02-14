using Application.DotNetCodeAnalyzer.Commands;
using Application.DotNetCodeAnalyzer.DTO;
using DotNetCompiler.Console.Repositories;
using DotNetCompiler.Console.Services.CSharp;
using DotNetCompiler.Console.Services.DotNetCompiler;
using DotNetCompiler.Console.Strategies;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetCompiler.Console.Test.Services
{
    public class CSharpCodeAnalyzerServiceTests : TestBase
    {
        CSharpCodeAnalyzerService _cSharpCodeAnalyzerService;
        protected override void InitTest()
        {
            base.InitTest();

            var directoryProvider = new DotNetDirectoryProvider();
            var dotNetFrameworkProvider = new DotNetFrameworkProvider(directoryProvider);

            var cSharpCompiler = new CSharpCodeCompilerService(dotNetFrameworkProvider);
            var compilerResolver = new DotNetCompilerResolver(new List<IDotNetCompilerService>() { cSharpCompiler });
            _cSharpCodeAnalyzerService = new CSharpCodeAnalyzerService(dotNetFrameworkProvider, compilerResolver);
        }

        [Test]
        public async Task CodeAnalyzer_ShouldReturnSuccess_WhenTestWillBePassedTest()
        {
            List<string> codes = new();
            codes.Add(GetSumFunctionTaskTestCode());
            codes.Add(GetSumOfResolvedFunctionTaskCode());

            CodeAnalyzeOutput codeAnalyzeOutput = await _cSharpCodeAnalyzerService.AnalyzeCodeAsync(GetNetAnalyzeCodeCommand(codes));
            Assert.That(codeAnalyzeOutput.Success, Is.True);
        }

        [Test]
        public async Task CodeAnalyzer_ShouldNotReturnSuccess_WhenTestWillBeFailedTest()
        {
            List<string> codes = new();
            codes.Add(GetSumFunctionTaskTestCode());
            codes.Add(GetSumUnResolvedFunctionTaskCode());
            CodeAnalyzeOutput codeAnalyzeOutput = await _cSharpCodeAnalyzerService.AnalyzeCodeAsync(GetNetAnalyzeCodeCommand(codes));
            Assert.That(codeAnalyzeOutput.Success, Is.False);
            Assert.That(codeAnalyzeOutput.Errors, Contains.Substring("Sum function is not returned number as expected"));
        }
        DotNetAnalyzeCodeCommand GetNetAnalyzeCodeCommand(List<string> codes)
        {
            var version = GetCSharpNet8VersionDTO();
            return new DotNetAnalyzeCodeCommand() { Codes = codes, DotNetVersionInfo = version };
        }

        [Test]
        public async Task CodeAnalyzer_UsiingTestCases_WhenTestWillBeFailedTest()
        {
            List<string> codes = new();
            codes.Add(GetSumFunctionTaskTestCaseCode());
            codes.Add(GetSumUnResolvedFunctionTaskCode());

            CodeAnalyzeOutput codeAnalyzeOutput = await _cSharpCodeAnalyzerService.AnalyzeCodeAsync(GetNetAnalyzeCodeCommand(codes));
            Assert.That(codeAnalyzeOutput.Success, Is.False);
        }

        [Test]
        public async Task CodeAnalyzer_UsiingTestCases_WhenTestWillBePassedTest()
        {
            List<string> codes = new();
            codes.Add(GetSumFunctionTaskTestCaseCode());
            codes.Add(GetSumOfResolvedFunctionTaskCode());

            CodeAnalyzeOutput codeAnalyzeOutput = await _cSharpCodeAnalyzerService.AnalyzeCodeAsync(GetNetAnalyzeCodeCommand(codes));
            Assert.That(codeAnalyzeOutput.Success, Is.True);
        }

        [Test]
        public async Task CodeAnalyzer_UsiingTestCasesForThreeElements_WhenTestWillBePassedTest()
        {
            List<string> codes = new();
            codes.Add(GetSumThreeNumbersFunctionTaskTestCaseCode());
            codes.Add(GetSumThreeNumbersOfResolvedFunctionTaskCode());

            CodeAnalyzeOutput codeAnalyzeOutput = await _cSharpCodeAnalyzerService.AnalyzeCodeAsync(GetNetAnalyzeCodeCommand(codes));
            Assert.That(codeAnalyzeOutput.Success, Is.True);
        }

        [Test]
        public async Task CodeAnalyzer_UsiingTestCasesForThreeElements_WhenTestWillBeFailedTest()
        {
            List<string> codes = new();
            codes.Add(GetSumThreeNumbersFunctionTaskTestCaseCode());
            codes.Add(GetSumThreeNumbersOfUnResolvedFunctionTaskCode());

            CodeAnalyzeOutput codeAnalyzeOutput = await _cSharpCodeAnalyzerService.AnalyzeCodeAsync(GetNetAnalyzeCodeCommand(codes));
            Assert.That(codeAnalyzeOutput.Success, Is.False);
        }

        [Test]
        public async Task CodeAnalyzer_UsingTestCasesForArrays_WhenTestWillBePassedTest()
        {
            List<string> codes = new();
            codes.Add(ArrayInversionFunctionTestCode());
            codes.Add(ArrayInversionFunctionCode());

            CodeAnalyzeOutput codeAnalyzeOutput = await _cSharpCodeAnalyzerService.AnalyzeCodeAsync(GetNetAnalyzeCodeCommand(codes));
            Assert.That(codeAnalyzeOutput.Success, Is.True);
        }
    }
}
