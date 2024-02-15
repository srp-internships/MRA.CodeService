using Application.DotNetCodeAnalyzer.Services;
using Application.DotNetCodeAnalyzer.DTO;
using Application.DotNetCodeAnalyzer.Commands;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Text;
using System.Reflection;
using Core.Extensions;
using Microsoft.Extensions.Logging;

namespace DotNetCompiler.Console.Services
{
    public class DotNetCodeAnalyzerService : IDotNetCodeAnalyzerService
    {
        const string ConsoleApplicationName = "DotNetCompiler.Console";
        readonly ILogger<DotNetCodeAnalyzerService> _logger;

        public DotNetCodeAnalyzerService(ILogger<DotNetCodeAnalyzerService> logger)
        {
            _logger = logger;
        }

        public async Task<CodeAnalyzeOutput> AnalyzeCodeAsync(DotNetAnalyzeCodeCommand codeAnalyzerParameter)
        {
            using (Process process = new())
            {
                try
                {
                    process.StartInfo.FileName = Path.Combine(GetExecutingAssemblyPath(), ConsoleApplicationName);
                    process.StartInfo.Arguments = GetParameter(codeAnalyzerParameter);
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.StandardInputEncoding = Encoding.UTF8;
                    process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                    process.StartInfo.StandardErrorEncoding = Encoding.UTF8;
                    process.StartInfo.WorkingDirectory = GetExecutingAssemblyPath();
                    process.Start();
                    await process.WaitForExitAsync();
                    string output = process.StandardOutput.ReadToEnd();
                    string err = process.StandardError.ReadToEnd();

                    if (process.ExitCode == ProcessSuccessCode)
                    {
                        return SuccessResult();
                    }
                    else
                    {
                        return FailResult(output + err, false);
                    }
                }
                catch (Exception ex)
                {
                    var files = Directory.GetFiles(Assembly.GetExecutingAssembly().Location.GetDirectoryPathFromFile());
                    _logger.LogError($"Error on compiling codes: {ex.Message}, Files: {string.Join(",", files)}");
                    return FailResult($"Внутренняя ошибка сервера", true);
                }
            }
        }
        const int ProcessSuccessCode = 0;

        string GetParameter(DotNetAnalyzeCodeCommand codeAnalyzerParameter)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(codeAnalyzerParameter)));
        }

        string GetExecutingAssemblyPath()
        {
            return Assembly.GetExecutingAssembly().Location.GetDirectoryPathFromFile();
        }

        CodeAnalyzeOutput SuccessResult()
        {
            return new CodeAnalyzeOutput { Success = true };
        }

        CodeAnalyzeOutput FailResult(string errors, bool internalError)
        {
            return new CodeAnalyzeOutput { Success = false, Errors = errors, InternalError = internalError };
        }
    }
}
