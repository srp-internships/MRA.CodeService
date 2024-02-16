using Application.Admin.DTO;
using Application.Admin.Queries;
using Application.DotNetCodeAnalyzer.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [AdminApiKey]
    public class AdminController : ApiControllerBase
    {
        [HttpPost("NewCompany")]
        public async Task<ActionResult<CompanyDTO>> NewCompany([FromBody] AddCompanyCommand addCompanyCommand)
        {
            return await Mediator.Send(addCompanyCommand);
        }

        [HttpGet("Companies")]
        public async Task<ActionResult<List<CompanyDTO>>> Companies([FromQuery] GetCompaniesQuery getCompaniesQuery)
        {
            return await Mediator.Send(getCompaniesQuery);
        }
    }
}
