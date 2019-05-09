using PipServices3.Commons.Data;
using Prices.Data.Version1;
using System.Threading.Tasks;

namespace Prices.Persistence
{
    public interface IPricesPersistence
    {
        Task<DataPage<PriceV1>> GetPageByFilterAsync(string correlationId, FilterParams filter, PagingParams paging);
        Task<PriceV1> GetOneByIdAsync(string correlationId, string id);
        Task<PriceV1> GetOneBySkuAsync(string correlationId, string sku);
        Task<PriceV1> CreateAsync(string correlationId, PriceV1 item);
        Task<PriceV1> UpdateAsync(string correlationId, PriceV1 item);
        Task<PriceV1> DeleteByIdAsync(string correlationId, string id);
    }
}
