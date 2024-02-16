using Application.DotNetCodeAnalyzer.Services;
using Domain.Entities;

namespace DotNetCompiler.Console.Services.DotNetCompiler
{
    internal interface IDotNetCodeAnalyzerConsoleService : IDotNetCodeAnalyzerService
    {
        DotNetVersionInfo DotNetVersionInfo { get; }
    }
}
