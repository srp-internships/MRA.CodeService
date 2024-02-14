using AutoMapper;
using Core.Mapping;
using Domain.Entities;

namespace Application.Admin.DTO
{
    public record CompanyDTO : IMapFrom<Company>
    {
        public string Name { get; set; }
        public string Apikey { get; set; }
        public string Host { get; set; }

        public void Mapping(Profile profile)
        {
            var map = profile.CreateMap<Company, CompanyDTO>();
            map.ForMember(s => s.Apikey, op => op.MapFrom(x => x.ApiKey.SecretKey));
        }
    }
}
