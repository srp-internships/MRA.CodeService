using Application.Admin.DTO;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DotNetCodeAnalyzer.Commands
{
    public class AddCompanyCommand : IRequest<CompanyDTO>
    {
        public string CompanyName { get; set; }

        public string Host { get; set; }
    }

    public class AddCompanyCommandHandler : IRequestHandler<AddCompanyCommand, CompanyDTO>
    {
        IApplicationDbContext _applicationDbContext;
        IMapper _mapper;
        public AddCompanyCommandHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<CompanyDTO> Handle(AddCompanyCommand request, CancellationToken cancellationToken)
        {
            var newCompany = await CreateNewCompany(request, cancellationToken);
            return _mapper.Map<Company, CompanyDTO>(newCompany);
        }

        async Task<Company> CreateNewCompany(AddCompanyCommand request, CancellationToken cancellationToken)
        {
            var newCompany = new Company()
            {
                Name = request.CompanyName,
                Host = request.Host,
                ApiKey = new ApiKey
                {
                    SecretKey = Guid.NewGuid().ToString()
                }
            };
            await _applicationDbContext.AddNewEntity(newCompany);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return newCompany;
        }
    }
}
