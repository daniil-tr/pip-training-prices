using PipServices3.Commons.Data;
using Prices.Data.Version1;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Prices.Clients.Version1
{
    public class PricesClientV1Fixture
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
            Priority = 1,
            Note = "Note 2",
        };

        private IPricesClientV1 _client;

        public PricesClientV1Fixture(IPricesClientV1 client)
        {
            _client = client;
        }

        public async Task TestCrudOperationsAsync()
        {
            // Create the first price
            var price = await _client.CreatePriceAsync(null, PRICE1);

            Assert.NotNull(price);
            Assert.True(PRICE1.Equals(price));

            // Create the second price
            price = await _client.CreatePriceAsync(null, PRICE2);

            Assert.NotNull(price);
            Assert.True(PRICE2.Equals(price));

            // Get all prices
            var page = await _client.GetPricesAsync(
                null,
                new FilterParams(),
                new PagingParams()
            );

            Assert.NotNull(page);
            Assert.Equal(2, page.Data.Count);

            var price1 = page.Data[0];

            // Update the price
            price1.Note = "ABC";

            price = await _client.UpdatePriceAsync(null, price1);

            Assert.NotNull(price);
            Assert.Equal(price1.Id, price.Id);
            Assert.Equal("ABC", price.Note);

            // Get price by sku
            price = await _client.GetPriceBySkuAsync(null, price1.Sku);

            Assert.NotNull(price);
            Assert.Equal(price1.Id, price.Id);

            // Delete the price
            price = await _client.DeletePriceByIdAsync(null, price1.Id);

            Assert.NotNull(price);
            Assert.Equal(price1.Id, price.Id);

            // Try to get deleted price
            price = await _client.GetPriceByIdAsync(null, price1.Id);

            Assert.Null(price);

            // Clean up for the second test
            await _client.DeletePriceByIdAsync(null, PRICE2.Id);
        }
    }
}
