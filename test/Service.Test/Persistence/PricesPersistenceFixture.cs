using PipServices3.Commons.Data;
using Prices.Data.Version1;
using Prices.Persistence;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Service.Test.Persistence
{
    public class PricesPersistenceFixture
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

        private PriceV1 PRICE3 = new PriceV1
        {
            Id = "3",
            PriceFileId = "20",
            ExternalRefId = "30",
            ProductId = "40",
            PartId = "50",
            Sku = "ASDF",
            PriceNet = 9025.0,
            PriceNetFull = 9031.55,
            PriceRetail = 9040.2,
            PriceRetailFull = 9045.99,
            DateStart = new DateTime(2018, 06, 08),
            DateEnd = new DateTime(2021, 12, 29),
            PromoCode = "100",
            //Priority = 101,
            Note = "Note 3",
        };

        private IPricesPersistence _persistence;

        public PricesPersistenceFixture(IPricesPersistence persistence)
        {
            _persistence = persistence;
        }

        private async Task TestCreatePricesAsync()
        {
            // Create the first price
            var price = await _persistence.CreateAsync(null, PRICE1);

            Assert.NotNull(price);
            Assert.True(PRICE1.Equals(price));

            // Create the second price
            price = await _persistence.CreateAsync(null, PRICE2);

            Assert.NotNull(price);
            Assert.True(PRICE2.Equals(price));

            // Create the third price
            price = await _persistence.CreateAsync(null, PRICE3);

            Assert.NotNull(price);
            Assert.True(PRICE3.Equals(price));
        }

        public async Task TestCrudOperationsAsync()
        {
            // Create items
            await TestCreatePricesAsync();

            // Get all prices
            var page = await _persistence.GetPageByFilterAsync(
                null,
                new FilterParams(),
                new PagingParams()
            );

            Assert.NotNull(page);
            Assert.Equal(3, page.Data.Count);

            var price1 = page.Data[0];

            // Update the price
            price1.Note = "ABC";

            var price = await _persistence.UpdateAsync(null, price1);

            Assert.NotNull(price);
            Assert.Equal(price1.Id, price.Id);
            Assert.Equal("ABC", price.Note);

            // Get price by udi
            price = await _persistence.GetOneBySkuAsync(null, price1.Sku);

            Assert.NotNull(price);
            Assert.Equal(price1.Id, price.Id);

            // Delete the price
            price = await _persistence.DeleteByIdAsync(null, price1.Id);

            Assert.NotNull(price);
            Assert.Equal(price1.Id, price.Id);

            // Try to get deleted price
            price = await _persistence.GetOneByIdAsync(null, price1.Id);

            Assert.Null(price);
        }

        public async Task TestGetWithFiltersAsync()
        {
            // Create items
            await TestCreatePricesAsync();

            // Get all prices
            var page = await _persistence.GetPageByFilterAsync(
                null,
                new FilterParams(),
                new PagingParams()
            );

            Assert.NotNull(page);
            Assert.Equal(3, page.Data.Count);

            // Filter by id
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "id", "1"
                ),
                new PagingParams()
            );

            Assert.Single(page.Data);

            // Filter by sku
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "sku", "TREWQ"
                ),
                new PagingParams()
            );

            Assert.Single(page.Data);

            // Filter by skus
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "skus", "QWERT,ASDF"
                ),
                new PagingParams()
            );

            Assert.Equal(2, page.Data.Count);

            // Filter by pricefile_id
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "pricefile_id", "2"
                ),
                new PagingParams()
            );

            Assert.Equal(2, page.Data.Count);

            // Filter by external_ref_id
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "external_ref_id", "30"
                ),
                new PagingParams()
            );

            Assert.Single(page.Data);

            // Filter by product_id
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "product_id", "4"
                ),
                new PagingParams()
            );

            Assert.Equal(2, page.Data.Count);

            // Filter by part_id
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "part_id", "5"
                ),
                new PagingParams()
            );

            Assert.Equal(2, page.Data.Count);

            // Filter by from_date_start
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "from_date_start", new DateTime(2019, 01, 01)
                ),
                new PagingParams()
            );

            Assert.Equal(2, page.Data.Count);

            // Filter by to_date_start
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "to_date_start", new DateTime(2019, 01, 01)
                ),
                new PagingParams()
            );

            Assert.Single(page.Data);

            // Filter by from_date_end
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "from_date_end", new DateTime(2020, 01, 01)
                ),
                new PagingParams()
            );

            Assert.Single(page.Data);

            // Filter by to_date_end
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "to_date_end", new DateTime(2020, 01, 01)
                ),
                new PagingParams()
            );

            Assert.Equal(2, page.Data.Count);
            
            // Filter by promo_code
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "promo_code", "100"
                ),
                new PagingParams()
            );

            Assert.Equal(2, page.Data.Count);

            // Filter by search
            page = await _persistence.GetPageByFilterAsync(
                null,
                FilterParams.FromTuples(
                    "search", "TREWQ"
                ),
                new PagingParams()
            );

            Assert.Single(page.Data);
        }
    }
}
