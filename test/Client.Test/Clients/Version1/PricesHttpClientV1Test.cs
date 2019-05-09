using PipServices3.Commons.Config;
using PipServices3.Commons.Refer;
using Prices.Logic;
using Prices.Persistence;
using Prices.Services.Version1;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Prices.Clients.Version1
{
    public class PricesHttpClientV1Test : IDisposable
    {
        private static readonly ConfigParams HttpConfig = ConfigParams.FromTuples(
            "connection.protocol", "http",
            "connection.host", "localhost",
            "connection.port", 8080
        );

        private PricesMemoryPersistence _persistence;
        private PricesController _controller;
        private PricesHttpClientV1 _client;
        private PricesHttpServiceV1 _service;
        private PricesClientV1Fixture _fixture;

        public PricesHttpClientV1Test()
        {
            _persistence = new PricesMemoryPersistence();
            _controller = new PricesController();
            _client = new PricesHttpClientV1();
            _service = new PricesHttpServiceV1();

            IReferences references = References.FromTuples(
                new Descriptor("prices", "persistence", "memory", "default", "1.0"), _persistence,
                new Descriptor("prices", "controller", "default", "default", "1.0"), _controller,
                new Descriptor("prices", "client", "http", "default", "1.0"), _client,
                new Descriptor("prices", "service", "http", "default", "1.0"), _service
            );

            _controller.SetReferences(references);

            _service.Configure(HttpConfig);
            _service.SetReferences(references);

            _client.Configure(HttpConfig);
            _client.SetReferences(references);

            _fixture = new PricesClientV1Fixture(_client);

            _service.OpenAsync(null).Wait();
            _client.OpenAsync(null).Wait();
        }

        public void Dispose()
        {
            _client.CloseAsync(null).Wait();
            _service.CloseAsync(null).Wait();
        }

        [Fact]
        public async Task TestCrudOperationsAsync()
        {
            await _fixture.TestCrudOperationsAsync();
        }
    }
}
