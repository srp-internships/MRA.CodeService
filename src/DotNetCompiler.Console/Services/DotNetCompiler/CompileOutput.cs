using System.Reflection;

namespace DotNetCompiler.Console.Services.DotNetCompiler
{
    public record CompileOutput
    {
        public Assembly Assembly { get; init; }

        public bool CompileSucceeded { get; init; }

        public string Errors { get; init; }
    }
}
