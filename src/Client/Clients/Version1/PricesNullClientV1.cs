using System.Threading.Tasks;
using PipServices3.Commons.Data;
using Prices.Data.Version1;

namespace Prices.Clients.Version1
{
    public class PricesNullClientV1 : IPricesClientV1
    {
        public async Task<PriceV1> CreatePriceAsync(string correlationId, PriceV1 price)
        {
            return await Task.FromResult(new PriceV1());
        }

        public async Task<PriceV1> DeletePriceByIdAsync(string correlationId, string id)
        {
            return await Task.FromResult(new PriceV1());
        }

        public async Task<PriceV1> GetPriceByIdAsync(string correlationId, string id)
        {
            return await Task.FromResult(new PriceV1());
        }

        public async Task<PriceV1> GetPriceBySkuAsync(string correlationId, string sku)
        {
            return await Task.FromResult(new PriceV1());
        }

        public async Task<DataPage<PriceV1>> GetPricesAsync(string correlationId, FilterParams filter, PagingParams paging)
        {
            return await Task.FromResult(new DataPage<PriceV1>());
        }

        public async Task<PriceV1> UpdatePriceAsync(string correlationId, PriceV1 price)
        {
            return await Task.FromResult(new PriceV1());
        }
    }
}
