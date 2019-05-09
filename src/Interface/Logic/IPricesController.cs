using PipServices3.Commons.Data;
using Prices.Data.Version1;
using System.Threading.Tasks;

namespace Prices.Logic
{
    public interface IPricesController
    {
        Task<DataPage<PriceV1>> GetPricesAsync(string correlationId, FilterParams filter, PagingParams paging);
        Task<PriceV1> GetPriceByIdAsync(string correlationId, string id);
        Task<PriceV1> GetPriceBySkuAsync(string correlationId, string udi);
        Task<PriceV1> CreatePriceAsync(string correlationId, PriceV1 beacon);
        Task<PriceV1> UpdatePriceAsync(string correlationId, PriceV1 beacon);
        Task<PriceV1> DeletePriceByIdAsync(string correlationId, string id);
    }
}
