using Microsoft.EntityFrameworkCore;
using Web_api_with_jwt_auth.Model;

namespace Web_api_with_jwt_auth.Data
{
    public class Application_db_context : DbContext
    {
        public Application_db_context(DbContextOptions dbContext):base(dbContext)
        {
            
        }
        public DbSet<User> users { get; set; }
    }
}
