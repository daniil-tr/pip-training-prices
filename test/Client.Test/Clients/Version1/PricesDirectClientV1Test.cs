using PipServices3.Commons.Refer;
using Prices.Logic;
using Prices.Persistence;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Prices.Clients.Version1
{
    public class PricesDirectClientV1Test : IDisposable
    {
        private PricesMemoryPersistence _persistence;
        private PricesController _controller;
        private PricesDirectClientV1 _client;
        private PricesClientV1Fixture _fixture;

        public PricesDirectClientV1Test()
        {
            _persistence = new PricesMemoryPersistence();
            _controller = new PricesController();
            _client = new PricesDirectClientV1();

            IReferences references = References.FromTuples(
                new Descriptor("prices", "persistence", "memory", "default", "1.0"), _persistence,
                new Descriptor("prices", "controller", "default", "default", "1.0"), _controller,
                new Descriptor("prices", "client", "direct", "default", "1.0"), _client
            );

            _controller.SetReferences(references);

            _client.SetReferences(references);

            _fixture = new PricesClientV1Fixture(_client);

            _client.OpenAsync(null).Wait();
        }

        public void Dispose()
        {
            _client.CloseAsync(null).Wait();
        }

        [Fact]
        public async Task TestCrudOperationsAsync()
        {
            await _fixture.TestCrudOperationsAsync();
        }
    }
}
