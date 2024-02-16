using Application.DotNetCodeAnalyzer.Services;
using Domain.Entities;

namespace DotNetCompiler.Console.Strategies
{
    internal interface IDotNetCodeAnalyzerResolver
    {
        IDotNetCodeAnalyzerService GetDotNetCodeAnalyzerService(DotNetVersionInfo dotNetVersionInfo);
    }
}
