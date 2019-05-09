using MongoDB.Driver;
using PipServices3.Commons.Data;
using PipServices3.Data.Persistence;
using Prices.Data.Version1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prices.Persistence
{
    public class PricesMemoryPersistence : IdentifiableMemoryPersistence<PriceV1, string>, IPricesPersistence
    {
        public PricesMemoryPersistence()
        {
            _maxPageSize = 1000;
        }

        private List<Func<PriceV1, bool>> ComposeFilter(FilterParams filter)
        {
            filter = filter ?? new FilterParams();

            var id = filter.GetAsNullableString("id");
            var pricefileId = filter.GetAsNullableString("pricefile_id");
            var externalRefId = filter.GetAsNullableString("external_ref_id");
            var productId = filter.GetAsNullableString("product_id");
            var partId = filter.GetAsNullableString("part_id");
            var sku = filter.GetAsNullableString("sku");
            var dateStart = filter.GetAsNullableDateTime("date_start");
            var dateEnd = filter.GetAsNullableDateTime("date_end");
            var promoCode = filter.GetAsNullableString("promo_code");

            var skus = filter.GetAsNullableString("skus");
            var skuList = !string.IsNullOrEmpty(skus) ? skus.Split(',') : null;

            return new List<Func<PriceV1, bool>>
            {
                (item) =>
                {
                    if (id != null && item.Id != id) return false;
                    if (pricefileId != null && item.PriceFileId != pricefileId) return false;
                    if (externalRefId != null && item.ExternalRefId != externalRefId) return false;
                    if (productId != null && item.ProductId != productId) return false;
                    if (partId != null && item.PartId != partId) return false;
                    if (sku != null && item.Sku != sku) return false;
                    if (dateStart != null && item.DateStart != dateStart) return false;
                    if (dateEnd != null && item.DateEnd != dateEnd) return false;
                    if (promoCode != null && item.PromoCode != promoCode) return false;
                    if (skuList != null && !skuList.Contains(item.Sku)) return false;
                    return true;
                }
            };
        }

        public async Task<PriceV1> GetOneBySkuAsync(string correlationId, string sku)
        {
            PriceV1 item = null;

            lock (_lock)
            {
                item = _items.Find((x) => { return x.Sku == sku; });
            }

            if (item != null)
                _logger.Trace(correlationId, "Found price by sku {0}", sku);
            else
                _logger.Trace(correlationId, "Cannot find price by sku {0}", sku);

            return await Task.FromResult(item);
        }

        public Task<DataPage<PriceV1>> GetPageByFilterAsync(string correlationId, FilterParams filter, PagingParams paging)
        {
            return GetPageByFilterAsync(correlationId, ComposeFilter(filter), paging);
        }
    }
}
