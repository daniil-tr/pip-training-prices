using MongoDB.Driver;
using PipServices3.Commons.Data;
using PipServices3.MongoDb.Persistence;
using Prices.Data.Version1;
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
            var dateStart = filterParams.GetAsNullableDateTime("date_start");
            var dateEnd = filterParams.GetAsNullableDateTime("date_end");
            var promoCode = filterParams.GetAsNullableString("promo_code");
            var skus = filterParams.GetAsNullableString("skus");
            var skuList = !string.IsNullOrEmpty(skus) ? skus.Split(',') : null;

            if (!string.IsNullOrEmpty(id)) filter &= builder.Eq(b => b.Id, id);
            if (!string.IsNullOrEmpty(pricefileId)) filter &= builder.Eq(b => b.PriceFileId, pricefileId);
            if (!string.IsNullOrEmpty(externalRefId)) filter &= builder.Eq(b => b.ExternalRefId, externalRefId);
            if (!string.IsNullOrEmpty(productId)) filter &= builder.Eq(b => b.ProductId, productId);
            if (!string.IsNullOrEmpty(partId)) filter &= builder.Eq(b => b.PartId, partId);
            if (!string.IsNullOrEmpty(sku)) filter &= builder.Eq(b => b.Sku, sku);
            if (dateStart.HasValue) filter &= builder.Eq(b => b.DateStart, dateStart.Value);
            if (dateEnd.HasValue) filter &= builder.Eq(b => b.DateEnd, dateEnd.Value);
            if (!string.IsNullOrEmpty(promoCode)) filter &= builder.Eq(b => b.PromoCode, promoCode);
            if (skuList != null) filter &= builder.In(b => b.Sku, skuList);

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
