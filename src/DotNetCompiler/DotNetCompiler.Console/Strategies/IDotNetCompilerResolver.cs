using Domain.Entities;
using DotNetCompiler.Console.Services.DotNetCompiler;

namespace DotNetCompiler.Console.Strategies
{
    internal interface IDotNetCompilerResolver
    {
        public IDotNetCompilerService GetDotNetCompilerService(DotNetVersionInfo dotNetVersionInfo);
    }
}
