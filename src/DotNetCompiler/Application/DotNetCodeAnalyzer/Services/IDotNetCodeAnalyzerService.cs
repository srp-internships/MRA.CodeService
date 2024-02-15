using Application.DotNetCodeAnalyzer.Commands;
using Application.DotNetCodeAnalyzer.DTO;
using System.Threading.Tasks;

namespace Application.DotNetCodeAnalyzer.Services
{
    public interface IDotNetCodeAnalyzerService
    {
        Task<CodeAnalyzeOutput> AnalyzeCodeAsync(DotNetAnalyzeCodeCommand codeAnalyzerParameter);
    }
}