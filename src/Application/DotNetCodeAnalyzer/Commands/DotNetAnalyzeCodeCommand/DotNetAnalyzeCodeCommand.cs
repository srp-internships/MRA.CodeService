using Application.DotNetCodeAnalyzer.DTO;
using Application.DotNetCodeAnalyzer.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DotNetCodeAnalyzer.Commands
{
    public class DotNetAnalyzeCodeCommand : IRequest<CodeAnalyzeOutput>
    {
        public List<string> Codes { get; set; }

        public DotNetInfoDTO DotNetVersionInfo { get; set; }

        public override string ToString()
        {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            return JsonSerializer.Serialize((object)this, jso);
        }
    }

    public class DotNetAnalyzeCodeCommandHandler : IRequestHandler<DotNetAnalyzeCodeCommand, CodeAnalyzeOutput>
    {
        IDotNetCodeAnalyzerService _dotNetCodeAnalyzerService;
        ILogger<DotNetAnalyzeCodeCommand> _logger;

        public DotNetAnalyzeCodeCommandHandler(IDotNetCodeAnalyzerService dotNetCodeAnalyzerService, ILogger<DotNetAnalyzeCodeCommand> logger)
        {
            _dotNetCodeAnalyzerService = dotNetCodeAnalyzerService;
            _logger = logger;
        }

        public async Task<CodeAnalyzeOutput> Handle(DotNetAnalyzeCodeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Analyze code: {request}");
            return await _dotNetCodeAnalyzerService.AnalyzeCodeAsync(request);
        }
    }
}
