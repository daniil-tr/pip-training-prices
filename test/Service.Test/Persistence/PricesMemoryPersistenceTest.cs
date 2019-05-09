using PipServices3.Commons.Config;
using Prices.Persistence;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Service.Test.Persistence
{
    public class PricesMemoryPersistenceTest : IDisposable
    {
        private PricesMemoryPersistence _persistence;
        private PricesPersistenceFixture _fixture;

        public PricesMemoryPersistenceTest()
        {
            _persistence = new PricesMemoryPersistence();
            _persistence.Configure(new ConfigParams());

            _fixture = new PricesPersistenceFixture(_persistence);

            _persistence.OpenAsync(null).Wait();
        }

        public void Dispose()
        {
            _persistence.CloseAsync(null).Wait();
        }

        [Fact]
        public async Task TestCrudOperationsAsync()
        {
            await _fixture.TestCrudOperationsAsync();
        }

        [Fact]
        public async Task TestGetWithFiltersAsync()
        {
            await _fixture.TestGetWithFiltersAsync();
        }
    }
}
