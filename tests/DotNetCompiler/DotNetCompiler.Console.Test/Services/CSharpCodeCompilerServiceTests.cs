using DotNetCompiler.Console.Services.DotNetCompiler;
using NUnit.Framework;

namespace DotNetCompiler.Console.Test.Services
{
    public class CSharpCodeCompilerServiceTests : TestBase
    {
        [Test]
        public void CodeShouldBeCompiledSuccessfully_WhenTextCodesAreCorrectTest()
        {
            string code = GetValidCode();
            CompileOutput result = CompileUsingCSharpAndDotNetSix(code);
            Assert.That(result.CompileSucceeded, Is.True);
            Assert.That(result.Assembly, Is.Not.Null);
        }

        [Test]
        public void AssemblyShouldContains_ClassesInTextCodeTest()
        {
            string code = GetValidCode();
            CompileOutput result = CompileUsingCSharpAndDotNetSix(code);
            Assert.That(result.Assembly.GetType("Program"), Is.Not.Null);
        }

        [Test]
        public void CompilerShouldReturnFalseWithError_WhenTextCodeIsInvalidTest()
        {
            string code = GetInValidCode();
            CompileOutput result = CompileUsingCSharpAndDotNetSix(code);
            Assert.That(result.CompileSucceeded, Is.False);
            Assert.That(result.Errors, Is.Not.Empty);
        }
    }
}
