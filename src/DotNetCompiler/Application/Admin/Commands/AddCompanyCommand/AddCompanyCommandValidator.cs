using Application.Repositories;
using Domain.Entities;
using FluentValidation;
using System.Linq;

namespace Application.DotNetCodeAnalyzer.Commands
{
    public class AddCompanyCommandValidator : AbstractValidator<AddCompanyCommand>
    {
        public AddCompanyCommandValidator(IApplicationDbContext applicationDbContext)
        {
            RuleFor(addCompany => addCompany.CompanyName).NotNull();
            RuleFor(addCompany => addCompany.Host).NotNull();

            RuleFor(addCompany => addCompany.CompanyName).Must(s => !applicationDbContext.GetEntities<Company>().Any(c => s == c.Name)).WithMessage("Name must be unique");
            RuleFor(addCompany => addCompany.Host).Must(s => !applicationDbContext.GetEntities<Company>().Any(c => s == c.Host)).WithMessage("Host must be unique");
        }
    }
}
