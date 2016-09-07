using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Clientes2S_Backend_Auth.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class Clientes2S_Backend_Auth_DbContext : IdentityDbContext<ApplicationUser>
    {
        public Clientes2S_Backend_Auth_DbContext()
            : base("Clientes2S-Backend-Auth", throwIfV1Schema: false)
        {
        }
        
        public static Clientes2S_Backend_Auth_DbContext Create()
        {
            return new Clientes2S_Backend_Auth_DbContext();
        }

        public System.Data.Entity.DbSet<Clientes2S_Backend_Auth.Models.Client> Clients { get; set; }

        public System.Data.Entity.DbSet<Clientes2S_Backend_Auth.Models.Contact> Contacts { get; set; }

        public System.Data.Entity.DbSet<Clientes2S_Backend_Auth.Models.Job> Jobs { get; set; }
    }
}