using Application.DotNetCodeAnalyzer.Commands;
using Newtonsoft.Json;
using System;

namespace DotNetCompiler.Console.Startup
{
    internal class ApplicationParameterParser : IApplicationParameterParser
    {
        public bool TryParse(string[] args, out DotNetAnalyzeCodeCommand codeAnalyzerParameter)
        {
            codeAnalyzerParameter = null;
            if (args.Length == 1)
            {
                try
                {
                    byte[] bytes = Convert.FromBase64String(args[0]);
                    var base64Json = System.Text.Encoding.UTF8.GetString(bytes); ;
                    codeAnalyzerParameter = JsonConvert.DeserializeObject<DotNetAnalyzeCodeCommand>(base64Json);
                    return true;
                }
                catch { }
            }
            return false;
        }
    }
}
