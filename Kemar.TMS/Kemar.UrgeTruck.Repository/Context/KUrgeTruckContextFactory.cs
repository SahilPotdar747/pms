using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Kemar.UrgeTruck.Repository.Context
{
    public class KUrgeTruckContextFactory : IKUrgeTruckContextFactory
    {
        private readonly IConfiguration _configuration;
        public static string TokenSecrete { get; set; }
        public static int RefreshTokenTTL { get; set; }
        public static int TokenTTL { get; set; }

        public KUrgeTruckContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public KUrgeTruckContext CreateKGASContext()
        {
            var options = new DbContextOptionsBuilder<KUrgeTruckContext>();
            options.UseSqlServer(_configuration.GetConnectionString("DataSQLContext"));
            TokenSecrete = _configuration.GetSection("AppSettings").GetSection("Secret").Value;
            RefreshTokenTTL = Convert.ToInt16(_configuration.GetSection("AppSettings").GetSection("RefreshTokenTTL").Value);
            TokenTTL = Convert.ToInt16(_configuration.GetSection("AppSettings").GetSection("TokenTTL").Value);

            return new KUrgeTruckContext(options.Options);
        }
    }
}
