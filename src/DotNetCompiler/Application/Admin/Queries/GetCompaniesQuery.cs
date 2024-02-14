using Application.Admin.DTO;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Admin.Queries
{
    public class GetCompaniesQuery : IRequest<List<CompanyDTO>>
    {
    }

    public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, List<CompanyDTO>>
    {
        IApplicationDbContext _applicationDbContext;
        IMapper _mapper;

        public GetCompaniesQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<List<CompanyDTO>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
        {
            var companies = await _applicationDbContext.GetEntities<Company>().Include(s => s.ApiKey).ToListAsync();
            return _mapper.Map<List<CompanyDTO>>(companies);
        }
    }
}
