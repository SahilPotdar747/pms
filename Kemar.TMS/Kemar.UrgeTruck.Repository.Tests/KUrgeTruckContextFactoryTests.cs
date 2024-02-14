using Kemar.UrgeTruck.Repository.Context;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;
using Shouldly;

namespace Kemar.UrgeTruck.Repository.Tests
{
    [TestFixture]
   public class KUrgeTruckContextFactoryTests
    {
        [Test]
        public void ShouldCreateUrgeTruckContext()
        {
            var fakeConfiguration = Substitute.For<IConfiguration>();
            fakeConfiguration.GetConnectionString("SQLConnection").Returns("conn");
            var kUrgeTruckContextFactory = new KUrgeTruckContextFactory(fakeConfiguration);

            var result = kUrgeTruckContextFactory.CreateKGASContext();

            result.ShouldBeOfType(typeof(KUrgeTruckContext));
        }
    }
}
