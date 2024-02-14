using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace Kemar.TAT.Repository.Context
{
    public class SqliteDataContext : DbContext
    {
        private readonly IConfiguration Configuration;

        public SqliteDataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sqlite database
            options.UseSqlite(Configuration.GetConnectionString("WebApiDatabase"));

        }
    }
}
