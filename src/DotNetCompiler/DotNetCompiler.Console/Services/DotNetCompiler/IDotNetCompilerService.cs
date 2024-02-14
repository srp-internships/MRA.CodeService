using Domain.Entities;
using System.Collections.Generic;

namespace DotNetCompiler.Console.Services.DotNetCompiler
{
    public interface IDotNetCompilerService
    {
        public DotNetVersionInfo DotNetVersionInfo { get; }
        public CompileOutput CompileTexts(IEnumerable<string> codes);
    }
}
