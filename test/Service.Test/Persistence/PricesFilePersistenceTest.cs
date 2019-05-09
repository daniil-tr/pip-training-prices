using PipServices3.Commons.Config;
using Prices.Persistence;
using System.Threading.Tasks;
using Xunit;

namespace Service.Test.Persistence
{
    public class PricesFilePersistenceTest
    {
        private PricesFilePersistence _persistence;
        private PricesPersistenceFixture _fixture;

        public PricesFilePersistenceTest()
        {
            ConfigParams config = ConfigParams.FromTuples(
                "path", "prices.json"
            );
            _persistence = new PricesFilePersistence();
            _persistence.Configure(config);
            _persistence.OpenAsync(null).Wait();
            _persistence.ClearAsync(null).Wait();

            _fixture = new PricesPersistenceFixture(_persistence);
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
