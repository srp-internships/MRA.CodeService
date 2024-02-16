using Domain.Entities;
using DotNetCompiler.Console.Services.DotNetCompiler;
using System.Collections.Generic;
using System.Linq;

namespace DotNetCompiler.Console.Strategies
{
    internal class DotNetCompilerResolver : IDotNetCompilerResolver
    {
        IEnumerable<IDotNetCompilerService> _services;

        public DotNetCompilerResolver(IEnumerable<IDotNetCompilerService> services)
        {
            _services = services;
        }

        public IDotNetCompilerService GetDotNetCompilerService(DotNetVersionInfo dotNetVersionInfo)
        {
            return _services.FirstOrDefault(s => s.DotNetVersionInfo.Equals(dotNetVersionInfo));
        }
    }
}
