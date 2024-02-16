using Application.DotNetCodeAnalyzer.Services;
using Domain.Entities;
using DotNetCompiler.Console.Services.DotNetCompiler;
using System.Collections.Generic;
using System.Linq;

namespace DotNetCompiler.Console.Strategies
{
    internal class DotNetCodeAnalyzerResolver : IDotNetCodeAnalyzerResolver
    {
        IEnumerable<IDotNetCodeAnalyzerConsoleService> _services;

        public DotNetCodeAnalyzerResolver(IEnumerable<IDotNetCodeAnalyzerConsoleService> services)
        {
            _services = services;
        }

        public IDotNetCodeAnalyzerService GetDotNetCodeAnalyzerService(DotNetVersionInfo dotNetVersionInfo)
        {
            return _services.FirstOrDefault(s => s.DotNetVersionInfo.Equals(dotNetVersionInfo));
        }
    }
}
