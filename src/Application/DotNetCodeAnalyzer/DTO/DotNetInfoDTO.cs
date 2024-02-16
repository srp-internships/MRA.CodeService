using Core.Mapping;
using Domain.Entities;

namespace Application.DotNetCodeAnalyzer.DTO
{
    public record DotNetInfoDTO : IMapFrom<DotNetVersionInfo>
    {
        public string Language { get; set; }

        public string Version { get; set; }
    }
}
