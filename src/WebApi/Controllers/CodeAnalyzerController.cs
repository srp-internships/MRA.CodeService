using Application.DotNetCodeAnalyzer.DTO;
using Microsoft.AspNetCore.Mvc;
using Application.DotNetCodeAnalyzer.Commands;
using System.Threading.Tasks;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiKey]
    public class CodeAnalyzerController : ApiControllerBase
    {
        [HttpPost("Analyze")]
        public async Task<ActionResult<CodeAnalyzeOutput>> Analyze([FromBody] DotNetAnalyzeCodeCommand getCoursesQuery)
        {
            return await Mediator.Send(getCoursesQuery);
        }
    }
}