using PipServices3.Commons.Config;
using PipServices3.Commons.Data;
using PipServices3.Commons.Refer;
using Prices.Data.Version1;
using Prices.Logic;
using Prices.Persistence;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Service.Test.Logic
{
    public class PricesControllerTest : IDisposable
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
            //Priority = 200,
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

        private PricesMemoryPersistence _persistence;
        private PricesController _controller;

        public PricesControllerTest()
        {
            _persistence = new PricesMemoryPersistence();
            _persistence.Configure(new ConfigParams());

            _controller = new PricesController();

            var references = References.FromTuples(
                new Descriptor("prices", "persistence", "memory", "*", "1.0"), _persistence,
                new Descriptor("prices", "controller", "default", "*", "1.0"), _controller
            );

            _controller.SetReferences(references);

            _persistence.OpenAsync(null).Wait();
        }

        public void Dispose()
        {
            _persistence.CloseAsync(null).Wait();
        }

        [Fact]
        public async Task TestCrudOperationsAsync()
        {
            // Create the first price
            var price = await _controller.CreatePriceAsync(null, PRICE1);

            Assert.NotNull(price);
            Assert.True(PRICE1.Equals(price));

            // Create the second price
            price = await _controller.CreatePriceAsync(null, PRICE2);

            Assert.NotNull(price);
            Assert.True(PRICE2.Equals(price));
            // Check priority default value
            //Assert.Equal(price.Priority, PriceV1.DEFAULT_PRIORITY);

            // Get all prices
            var page = await _controller.GetPricesAsync(
                null,
                new FilterParams(),
                new PagingParams()
            );

            Assert.NotNull(page);
            Assert.Equal(2, page.Data.Count);

            var price1 = page.Data[0];

            // Update the price
            price1.Note = "ABC";

            price = await _controller.UpdatePriceAsync(null, price1);

            Assert.NotNull(price);
            Assert.Equal(price1.Id, price.Id);
            Assert.Equal("ABC", price.Note);

            // Get price by sku
            price = await _controller.GetPriceBySkuAsync(null, price1.Sku);

            Assert.NotNull(price);
            Assert.Equal(price1.Id, price.Id);

            // Delete the price
            price = await _controller.DeletePriceByIdAsync(null, price1.Id);

            Assert.NotNull(price);
            Assert.Equal(price1.Id, price.Id);

            // Try to get deleted price
            price = await _controller.GetPriceByIdAsync(null, price1.Id);

            Assert.Null(price);
        }
    }
}
