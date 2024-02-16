using Application.DotNetCodeAnalyzer.Commands;

namespace DotNetCompiler.Console.Startup
{
    internal interface IApplicationParameterParser
    {
        bool TryParse(string[] args, out DotNetAnalyzeCodeCommand codeAnalyzerParameter);
    }
}
