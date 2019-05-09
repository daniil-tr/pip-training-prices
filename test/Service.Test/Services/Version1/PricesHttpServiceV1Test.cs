using PipServices3.Commons.Config;
using PipServices3.Commons.Convert;
using PipServices3.Commons.Data;
using PipServices3.Commons.Refer;
using Prices.Data.Version1;
using Prices.Logic;
using Prices.Persistence;
using Prices.Services.Version1;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Service.Test.Services.Version1
{
    public class PricesHttpServiceV1Test
    {
        private PriceV1 PRICE1 = new PriceV1
        {
            Id = "1",
            PriceFileId = "2",
            ExternalRefId = "3",
            ProductId = "4",
            PartId = "5",
            Sku = "TREWQ",
            PriceNet = 125.0,
            PriceNetFull = 131.55,
            PriceRetail = 140.2,
            PriceRetailFull = 145.99,
            DateStart = new DateTime(2019, 05, 07),
            DateEnd = new DateTime(2019, 12, 29),
            PromoCode = "100",
            Priority = 200,
            Note = "Note 1",
        };

        private PriceV1 PRICE2 = new PriceV1
        {
            Id = "2",
            PriceFileId = "2",
            ExternalRefId = "3",
            ProductId = "4",
            PartId = "5",
            Sku = "QWERT",
            PriceNet = 25.0,
            PriceNetFull = 31.55,
            PriceRetail = 40.2,
            PriceRetailFull = 45.99,
            DateStart = new DateTime(2019, 06, 08),
            DateEnd = new DateTime(2019, 12, 29),
            PromoCode = "101",
            //Priority = 1,
            Note = "Note 2",
        };

        private static readonly ConfigParams HttpConfig = ConfigParams.FromTuples(
            "connection.protocol", "http",
            "connection.host", "localhost",
            "connection.port", "3300"
        );

        private PricesMemoryPersistence _persistence;
        private PricesController _controller;
        private PricesHttpServiceV1 _service;

        public PricesHttpServiceV1Test()
        {
            _persistence = new PricesMemoryPersistence();
            _controller = new PricesController();
            _service = new PricesHttpServiceV1();

            IReferences references = References.FromTuples(
                new Descriptor("prices", "persistence", "memory", "default", "1.0"), _persistence,
                new Descriptor("prices", "controller", "default", "default", "1.0"), _controller,
                new Descriptor("prices", "service", "http", "default", "1.0"), _service
            );

            _controller.SetReferences(references);

            _service.Configure(HttpConfig);
            _service.SetReferences(references);

            Task.Run(() => _service.OpenAsync(null));
            Thread.Sleep(1000);
        }

        [Fact]
        public async Task TestCrudOperationsAsync()
        {
            // Create the first price
            var price = await Invoke<PriceV1>("create_price", new { price = PRICE1 });

            Assert.NotNull(price);
            Assert.True(PRICE1.Equals(price));

            // Create the second price
            price = await Invoke<PriceV1>("create_price", new { price = PRICE2 });

            Assert.NotNull(price);
            //Assert.True(PRICE2.Equals(price));
            Assert.Equal(price.Priority, PriceV1.DEFAULT_PRIORITY);

            // Get all prices
            var page = await Invoke<DataPage<PriceV1>>(
                "get_prices",
                new
                {
                    filter = new FilterParams(),
                    paging = new PagingParams()
                }
            );

            Assert.NotNull(page);
            Assert.Equal(2, page.Data.Count);

            var price1 = page.Data[0];

            // Update the price
            price1.Note = "ABC";

            price = await Invoke<PriceV1>("update_price", new { price = price1 });

            Assert.NotNull(price);
            Assert.Equal(price1.Id, price.Id);
            Assert.Equal("ABC", price.Note);

            // Get price by sku
            price = await Invoke<PriceV1>("get_price_by_sku", new { sku = price1.Sku });

            Assert.NotNull(price);
            Assert.Equal(price1.Id, price.Id);

            // Delete the price
            price = await Invoke<PriceV1>("delete_price_by_id", new { price_id = price1.Id });

            Assert.NotNull(price);
            Assert.Equal(price1.Id, price.Id);

            // Try to get deleted price
            price = await Invoke<PriceV1>("get_price_by_id", new { price_id = price1.Id });

            Assert.Null(price);
        }

        private static async Task<T> Invoke<T>(string route, dynamic request)
        {
            using (var httpClient = new HttpClient())
            {
                var requestValue = JsonConverter.ToJson(request);
                using (var content = new StringContent(requestValue, Encoding.UTF8, "application/json"))
                {
                    var response = await httpClient.PostAsync("http://localhost:3300/v1/prices/" + route, content);
                    var responseValue = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConverter.FromJson<T>(responseValue);
                    return result;
                }
            }
        }
    }
}
