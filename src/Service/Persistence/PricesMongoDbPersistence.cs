using MongoDB.Driver;
using PipServices3.Commons.Data;
using PipServices3.MongoDb.Persistence;
using Prices.Data.Version1;
using System;
using System.Threading.Tasks;

namespace Prices.Persistence
{
    public class PricesMongoDbPersistence : IdentifiableMongoDbPersistence<PriceV1, string>, IPricesPersistence
    {
        public PricesMongoDbPersistence() 
            : base("prices")
        { }

        private new FilterDefinition<PriceV1> ComposeFilter(FilterParams filterParams)
        {
            filterParams = filterParams ?? new FilterParams();

            var builder = Builders<PriceV1>.Filter;
            var filter = builder.Empty;

            var id = filterParams.GetAsNullableString("id");
            var pricefileId = filterParams.GetAsNullableString("pricefile_id");
            var externalRefId = filterParams.GetAsNullableString("external_ref_id");
            var productId = filterParams.GetAsNullableString("product_id");
            var partId = filterParams.GetAsNullableString("part_id");
            var sku = filterParams.GetAsNullableString("sku");
            var fromDateStart = filterParams.GetAsNullableDateTime("from_date_start");
            var toDateStart = filterParams.GetAsNullableDateTime("to_date_start");
            var fromDateEnd = filterParams.GetAsNullableDateTime("from_date_end");
            var toDateEnd = filterParams.GetAsNullableDateTime("to_date_end");
            var promoCode = filterParams.GetAsNullableString("promo_code");
            var skus = filterParams.GetAsNullableString("skus");
            var skuList = !string.IsNullOrWhiteSpace(skus) ? skus.Split(',') : null;
            var search = filterParams.GetAsNullableString("search");

            if (!string.IsNullOrWhiteSpace(id)) filter &= builder.Eq(b => b.Id, id);
            if (!string.IsNullOrWhiteSpace(pricefileId)) filter &= builder.Eq(b => b.PriceFileId, pricefileId);
            if (!string.IsNullOrWhiteSpace(externalRefId)) filter &= builder.Eq(b => b.ExternalRefId, externalRefId);
            if (!string.IsNullOrWhiteSpace(productId)) filter &= builder.Eq(b => b.ProductId, productId);
            if (!string.IsNullOrWhiteSpace(partId)) filter &= builder.Eq(b => b.PartId, partId);
            if (!string.IsNullOrWhiteSpace(sku)) filter &= builder.Eq(b => b.Sku, sku);
            if (fromDateStart.HasValue) filter &= builder.Gte(b => b.DateStart, fromDateStart);
            if (toDateStart.HasValue) filter &= builder.Lte(b => b.DateStart, toDateStart);
            if (fromDateEnd.HasValue) filter &= builder.Gte(b => b.DateEnd, fromDateEnd);
            if (toDateEnd.HasValue) filter &= builder.Lte(b => b.DateEnd, toDateEnd);
            if (!string.IsNullOrWhiteSpace(promoCode)) filter &= builder.Eq(b => b.PromoCode, promoCode);
            if (skuList != null) filter &= builder.In(b => b.Sku, skuList);
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchFilter = builder.Eq(b => b.Id, search);
                searchFilter |= builder.Eq(b => b.PriceFileId, search);
                searchFilter |= builder.Eq(b => b.ProductId, search);
                searchFilter |= builder.Eq(b => b.PartId, search);
                searchFilter |= builder.Eq(b => b.ExternalRefId, search);
                searchFilter |= builder.Eq(b => b.Sku, search);
                searchFilter |= builder.Eq(b => b.PromoCode, search);
                filter &= searchFilter;
            }

            return filter;
        }

        public async Task<PriceV1> GetOneBySkuAsync(string correlationId, string sku)
        {
            var builder = Builders<PriceV1>.Filter;
            var filter = builder.Eq(a => a.Sku, sku);
            var result = await _collection.Find(filter).FirstOrDefaultAsync();

            if (result != null)
                _logger.Trace(correlationId, "Retrieved from {0} with sku = {1}", _collectionName, sku);
            else
                _logger.Trace(correlationId, "Nothing found from {0} with sku = {1}", _collectionName, sku);

            return result;
        }

        public async Task<DataPage<PriceV1>> GetPageByFilterAsync(string correlationId, FilterParams filter, PagingParams paging)
        {
            return await GetPageByFilterAsync(correlationId, ComposeFilter(filter), paging);
        }
    }
}
