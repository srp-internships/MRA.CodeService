using Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContextInitializer : IApplicationDbContextInitializer
    {
        ApplicationDbContext _applicationDbContext;

        public ApplicationDbContextInitializer(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void Initialize()
        {
            _applicationDbContext.Database.Migrate();
        }
    }
}
