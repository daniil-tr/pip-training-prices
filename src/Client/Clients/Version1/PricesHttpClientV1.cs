using PipServices3.Commons.Data;
using PipServices3.Rpc.Clients;
using Prices.Data.Version1;
using System.Threading.Tasks;

namespace Prices.Clients.Version1
{
    public class PricesHttpClientV1 : CommandableHttpClient, IPricesClientV1
    {
        public PricesHttpClientV1()
            : base("v1/prices")
        { }

        public async Task<DataPage<PriceV1>> GetPricesAsync(string correlationId, FilterParams filter, PagingParams paging)
        {
            return await CallCommandAsync<DataPage<PriceV1>>(
                "get_prices",
                correlationId,
                new
                {
                    filter = filter,
                    paging = paging
                }
            );
        }

        public async Task<PriceV1> GetPriceByIdAsync(string correlationId, string id)
        {
            return await CallCommandAsync<PriceV1>(
                "get_price_by_id",
                correlationId,
                new
                {
                    price_id = id
                }
            );
        }

        public async Task<PriceV1> GetPriceBySkuAsync(string correlationId, string sku)
        {
            return await CallCommandAsync<PriceV1>(
                "get_price_by_sku",
                correlationId,
                new
                {
                    sku = sku
                }
            );
        }

        public async Task<PriceV1> CreatePriceAsync(string correlationId, PriceV1 price)
        {
            return await CallCommandAsync<PriceV1>(
                "create_price",
                correlationId,
                new
                {
                    price = price
                }
            );
        }

        public async Task<PriceV1> UpdatePriceAsync(string correlationId, PriceV1 price)
        {
            return await CallCommandAsync<PriceV1>(
                "update_price",
                correlationId,
                new
                {
                    price = price
                }
            );
        }

        public async Task<PriceV1> DeletePriceByIdAsync(string correlationId, string id)
        {
            return await CallCommandAsync<PriceV1>(
                "delete_price_by_id",
                correlationId,
                new
                {
                    price_id = id
                }
            );
        }
    }
}
