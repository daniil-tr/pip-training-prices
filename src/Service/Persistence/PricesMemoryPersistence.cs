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
            var fromDateStart = filter.GetAsNullableDateTime("from_date_start");
            var toDateStart = filter.GetAsNullableDateTime("to_date_start");
            var fromDateEnd = filter.GetAsNullableDateTime("from_date_end");
            var toDateEnd = filter.GetAsNullableDateTime("to_date_end");
            var promoCode = filter.GetAsNullableString("promo_code");

            var skus = filter.GetAsNullableString("skus");
            var skuList = !string.IsNullOrEmpty(skus) ? skus.Split(',') : null;
            var search = filter.GetAsNullableString("search");

            return new List<Func<PriceV1, bool>>
            {
                (item) =>
                {
                    if (!string.IsNullOrWhiteSpace(id) && item.Id != id) return false;
                    if (!string.IsNullOrWhiteSpace(pricefileId) && item.PriceFileId != pricefileId) return false;
                    if (!string.IsNullOrWhiteSpace(externalRefId) && item.ExternalRefId != externalRefId) return false;
                    if (!string.IsNullOrWhiteSpace(productId) && item.ProductId != productId) return false;
                    if (!string.IsNullOrWhiteSpace(partId) && item.PartId != partId) return false;
                    if (!string.IsNullOrWhiteSpace(sku) && !item.Sku.Equals(sku, StringComparison.CurrentCultureIgnoreCase)) return false;
                    if (fromDateStart != null && item.DateStart < fromDateStart) return false;
                    if (toDateStart != null && item.DateStart > toDateStart) return false;
                    if (fromDateEnd != null && item.DateEnd < fromDateEnd) return false;
                    if (toDateEnd != null && item.DateEnd > toDateEnd) return false;
                    if (!string.IsNullOrWhiteSpace(promoCode) && !item.PromoCode.Equals(promoCode, StringComparison.CurrentCultureIgnoreCase)) return false;
                    if (skuList != null && !skuList.Contains(item.Sku)) return false;
                    if (!string.IsNullOrWhiteSpace(search) && !MatchSearch(item, search)
                        //&& item.Id != search
                        //&& item.PriceFileId != search
                        //&& item.ProductId != search
                        //&& item.PartId != search
                        //&& item.ExternalRefId != search
                        //&& item.Sku.ToLower() != search
                        //&& item.PromoCode.ToLower() != search
                        )
                        return false;
                    return true;
                }
            };
        }

        private bool MatchSearch(PriceV1 item, string search)
        {
            return (item.Id != null && item.Id == search) ? true
                : (item.PriceFileId != null && item.PriceFileId == search) ? true
                : (item.ProductId != null && item.ProductId == search) ? true
                : (item.PartId != null && item.PartId == search) ? true
                : (item.ExternalRefId != null && item.ExternalRefId == search) ? true
                : (item.Sku != null && item.Sku.Equals(search, StringComparison.CurrentCultureIgnoreCase)) ? true
                : (item.PromoCode != null && item.PromoCode.Equals(search, StringComparison.CurrentCultureIgnoreCase)) ? true
                : false;
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
