using PipServices3.Commons.Data;
using PipServices3.Commons.Refer;
using PipServices3.Rpc.Clients;
using Prices.Data.Version1;
using Prices.Logic;
using System.Threading.Tasks;

namespace Prices.Clients.Version1
{
    public class PricesDirectClientV1 : DirectClient<IPricesController>, IPricesClientV1
    {
        public PricesDirectClientV1() : base()
        {
            _dependencyResolver.Put("controller", new Descriptor("prices", "controller", "*", "*", "1.0"));
        }

        public async Task<DataPage<PriceV1>> GetPricesAsync(
            string correlationId, FilterParams filter, PagingParams paging)
        {
            using (Instrument(correlationId, "prices.get_prices"))
            {
                return await _controller.GetPricesAsync(correlationId, filter, paging);
            }
        }

        public async Task<PriceV1> GetPriceByIdAsync(string correlationId, string id)
        {
            using (Instrument(correlationId, "prices.get_price_by_id"))
            {
                return await _controller.GetPriceByIdAsync(correlationId, id);
            }
        }

        public async Task<PriceV1> GetPriceBySkuAsync(string correlationId, string sku)
        {
            using (Instrument(correlationId, "prices.get_price_by_sku"))
            {
                return await _controller.GetPriceBySkuAsync(correlationId, sku);
            }
        }

        public async Task<PriceV1> CreatePriceAsync(string correlationId, PriceV1 price)
        {
            using (Instrument(correlationId, "prices.create_price"))
            {
                return await _controller.CreatePriceAsync(correlationId, price);
            }
        }

        public async Task<PriceV1> UpdatePriceAsync(string correlationId, PriceV1 price)
        {
            using (Instrument(correlationId, "prices.update_price"))
            {
                return await _controller.UpdatePriceAsync(correlationId, price);
            }
        }

        public async Task<PriceV1> DeletePriceByIdAsync(string correlationId, string id)
        {
            using (Instrument(correlationId, "prices.delete_price_by_id"))
            {
                return await _controller.DeletePriceByIdAsync(correlationId, id);
            }
        }
    }
}
