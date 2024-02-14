using Application.DotNetCodeAnalyzer.Commands;
using Application.DotNetCodeAnalyzer.DTO;
using Core.Exceptions;
using Shared.Test;
using static Application.IntegrationTest.TestHelper;

namespace Application.IntegrationTest.DotNetCodeAnalyzer
{
    public class DotNetAnalyzeCodeCommandTests : DotNetCodeTestBase
    {
        [Test]
        public void DotNetAnalyzeCodeCommand_ShouldReturnError_WhenDotNetVersionOrCodesAreEmptyTest()
        {
            DotNetAnalyzeCodeCommand command = new();

            ValidationFailureException validationError = Assert.ThrowsAsync<ValidationFailureException>(() => SendAsync(command));

            var dotNetVersionIsEmptyErrorWasShown = IsErrorExists("DotNetVersionInfo", "'Dot Net Version Info' must not be empty.", validationError);

            Assert.That(dotNetVersionIsEmptyErrorWasShown, Is.True);

            var codesAreEmptyErrorWasShown = IsErrorExists("Codes", "'Codes' must not be empty.", validationError);
            Assert.That(codesAreEmptyErrorWasShown, Is.True);
        }

        [Test]
        public async Task DotNetAnalyzeCodeCommand_ShouldReturnError_WhenDotNetVersionDoesNotExistsTest()
        {
            DotNetAnalyzeCodeCommand command = new() { Codes = new List<string>() { "some code" } };
            command.DotNetVersionInfo = new DotNetInfoDTO() { Language = "Javascript", Version = "6.0" };

            var codeAnalyzeOutput = await SendAsync(command);
            Assert.That(codeAnalyzeOutput.Success, Is.False);
            Assert.That(codeAnalyzeOutput.Errors, Contains.Substring("Dot net version does not support"));
        }

        [Test]
        public async Task DotNetAnalyzeCodeCommand_ShouldReturnError_WhenCompilerWasFailedTest()
        {
            DotNetAnalyzeCodeCommand command = new() { Codes = new List<string>() { "some code" } };
            command.DotNetVersionInfo = new DotNetInfoDTO() { Language = Constants.CSHARP_LANGUAGE, Version = Constants.DOTNET_EIGHT_VERSION };

            var codeAnalyzeOutput = await SendAsync(command);
            Assert.That(codeAnalyzeOutput.Success, Is.False);
            Assert.That(codeAnalyzeOutput.Errors, Contains.Substring("Code was not compiled"));
        }

        [Test]
        public async Task DotNetAnalyzeCodeCommand_ShouldReturnError_WhenCompilerSuccessAndCodeAnalyzerFailedTest()
        {
            DotNetAnalyzeCodeCommand command = new() { Codes = new() };
            command.DotNetVersionInfo = new DotNetInfoDTO() { Language = Constants.CSHARP_LANGUAGE, Version = Constants.DOTNET_EIGHT_VERSION };
            command.Codes.Add(GetSumFunctionTaskTestCode());
            command.Codes.Add(GetSumUnResolvedFunctionTaskCode());

            var codeAnalyzeOutput = await SendAsync(command);
            Assert.That(codeAnalyzeOutput.Success, Is.False);
            Assert.That(codeAnalyzeOutput.Errors, Contains.Substring("Sum function is not returned number as expected"));
        }

        [Test]
        public async Task DotNetAnalyzeCodeCommand_ShouldReturnSuccess_WhenCompilerSuccededAndCodeAnalyzerSuccededTest()
        {
            DotNetAnalyzeCodeCommand command = new() { Codes = new() };
            command.DotNetVersionInfo = new DotNetInfoDTO() { Language = Constants.CSHARP_LANGUAGE, Version = Constants.DOTNET_EIGHT_VERSION };
            command.Codes.Add(GetSumFunctionTaskTestCode());
            command.Codes.Add(GetSumOfResolvedFunctionTaskCode());

            var codeAnalyzeOutput = await SendAsync(command);
            Assert.That(codeAnalyzeOutput.Success, Is.True);
        }

        [Test]
        public async Task DotNetAnalyzeCodeCommand_ShouldReturnError_WhenStackoverflowExceptionWillBeThrownTest()
        {
            DotNetAnalyzeCodeCommand command = new() { Codes = new() };
            command.DotNetVersionInfo = new DotNetInfoDTO() { Language = Constants.CSHARP_LANGUAGE, Version = Constants.DOTNET_EIGHT_VERSION };
            command.Codes.Add(GetSumFunctionTaskTestCode());
            command.Codes.Add(GetStackoverflowCode());

            var codeAnalyzeOutput = await SendAsync(command);
            Assert.That(codeAnalyzeOutput.Success, Is.False);
        }

        [Test]
        public async Task DotNetAnalyzeCodeCommand_ShouldReturnError_WhenRunningCodeTakesMoreThan3SecondsTest()
        {
            DotNetAnalyzeCodeCommand command = new() { Codes = new() };
            command.DotNetVersionInfo = new DotNetInfoDTO() { Language = Constants.CSHARP_LANGUAGE, Version = Constants.DOTNET_EIGHT_VERSION };
            command.Codes.Add(GetSumFunctionTaskTestCode());
            command.Codes.Add(GetTimeoutCode());

            var codeAnalyzeOutput = await SendAsync(command);
            Assert.That(codeAnalyzeOutput.Success, Is.False);
            Assert.That(codeAnalyzeOutput.Errors, Contains.Substring("Time out on analyzing code"));
        }
    }
}
