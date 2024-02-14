using Application.DotNetCodeAnalyzer.Commands;
using DotNetCompiler.Console.Repositories;
using DotNetCompiler.Console.Strategies;
using System;
using System.Threading.Tasks;

namespace DotNetCompiler.Console.Startup
{
    internal class ApplicationStartup
    {
        IApplicationParameterParser _applicationParameterParser;
        IDotNetFrameworkProvider _dotNetFrameworkProvider;
        IDotNetCodeAnalyzerResolver _dotNetCodeAnalyzerResolver;

        public ApplicationStartup(
            IApplicationParameterParser applicationParameterParser,
            IDotNetFrameworkProvider dotNetFrameworkProvider,
            IDotNetCodeAnalyzerResolver dotNetCodeAnalyzerResolver
            )
        {
            _applicationParameterParser = applicationParameterParser;
            _dotNetFrameworkProvider = dotNetFrameworkProvider;
            _dotNetCodeAnalyzerResolver = dotNetCodeAnalyzerResolver;
        }
        const int TimeoutSecond = 5;
        public async Task Run(string[] args)
        {
            if (_applicationParameterParser.TryParse(args, out DotNetAnalyzeCodeCommand dotNetAnalyzeCodeCommand))
            {
                var dotNetInfoDTO = dotNetAnalyzeCodeCommand.DotNetVersionInfo;
                var dotNetInfo = _dotNetFrameworkProvider.GetDotNetVersion(dotNetInfoDTO?.Version, dotNetInfoDTO?.Language);
                if (dotNetInfo == null)
                    throw new InvalidOperationException($"Dot net version does not support: Version: {dotNetInfoDTO.Version}, Language: {dotNetInfoDTO.Language}");

                var codeAnalyzer = _dotNetCodeAnalyzerResolver.GetDotNetCodeAnalyzerService(dotNetInfo);
                if (codeAnalyzer == null)
                    throw new InvalidOperationException($"Code analyzer was not found: Version: {dotNetInfo.Version}, Language: {dotNetInfo.Language}");

                var codeAnalyzerTask = codeAnalyzer.AnalyzeCodeAsync(dotNetAnalyzeCodeCommand);
                if (await Task.WhenAny(codeAnalyzerTask, Task.Delay(TimeoutSecond * 1000)) == codeAnalyzerTask)
                {
                    var result = codeAnalyzerTask.Result;
                    if (!result.Success)
                    {
                        System.Console.Write(result.Errors);
                        Environment.Exit(1);
                    }
                }
                else
                {
                    System.Console.Write("Time out on analyzing code");
                    Environment.Exit(1);
                }
            }
            else
            {
                throw new ArgumentException($"Invalid arguments {string.Join(",", args)}. It should have single JSON argument");
            }
        }
    }
}
