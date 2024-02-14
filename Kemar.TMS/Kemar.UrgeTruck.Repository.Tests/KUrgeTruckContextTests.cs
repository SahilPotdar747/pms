using Kemar.UrgeTruck.Repository.Context;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Shouldly;

namespace Kemar.UrgeTruck.Repository.Tests
{
    [TestFixture]
    public class KUrgeTruckContextTests
    {
        private KUrgeTruckContext _dbContext;

        [SetUp]
        public void Setup()
        {
            _dbContext = new UrgeTruckInMemoryContext().GetTATDbContext();
        }

        [Test]
        public void Initialized_DBContext()
        {
            _dbContext.ShouldNotBeNull();
        }
    }

    public class UrgeTruckInMemoryContext
    {
        public KUrgeTruckContext GetTATDbContext(bool reset = false)
        {
            var options = new DbContextOptionsBuilder<KUrgeTruckContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryTATDatabaes")
                .Options;
            var dbContext = new KUrgeTruckContext(options);
            if (reset)
                dbContext.Database.EnsureDeleted();
            return dbContext;
        }
    }
}