using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthentificationService.Data
{
    public class EcommerceAuthDb : IdentityDbContext<IdentityUser>
    {
        public EcommerceAuthDb(DbContextOptions<EcommerceAuthDb> options)
            : base(options)
        {
        }
    }
}
