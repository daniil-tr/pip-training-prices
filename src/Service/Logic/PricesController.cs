using PipServices3.Commons.Commands;
using PipServices3.Commons.Config;
using PipServices3.Commons.Data;
using PipServices3.Commons.Refer;
using Prices.Data.Version1;
using Prices.Persistence;
using System.Threading.Tasks;

namespace Prices.Logic
{
    public class PricesController : IPricesController, IConfigurable, IReferenceable, ICommandable
    {
        private IPricesPersistence _persistence;
        private PricesCommandSet _commandSet;

        public PricesController()
        { }

        public void Configure(ConfigParams config)
        { }

        public void SetReferences(IReferences references)
        {
            _persistence = references.GetOneRequired<IPricesPersistence>(
                new Descriptor("prices", "persistence", "*", "*", "1.0")
            );
        }

        public CommandSet GetCommandSet()
        {
            if (_commandSet == null)
                _commandSet = new PricesCommandSet(this);
            return _commandSet;
        }

        public async Task<DataPage<PriceV1>> GetPricesAsync(string correlationId, FilterParams filter, PagingParams paging)
        {
            return await _persistence.GetPageByFilterAsync(correlationId, filter, paging);
        }

        public async Task<PriceV1> GetPriceByIdAsync(string correlationId, string id)
        {
            return await _persistence.GetOneByIdAsync(correlationId, id);
        }

        public async Task<PriceV1> GetPriceBySkuAsync(string correlationId, string udi)
        {
            return await _persistence.GetOneBySkuAsync(correlationId, udi);
        }

        public async Task<PriceV1> CreatePriceAsync(string correlationId, PriceV1 price)
        {
            price.Id = price.Id ?? IdGenerator.NextLong();
            price.Priority = price.Priority ?? PriceV1.DEFAULT_PRIORITY;

            return await _persistence.CreateAsync(correlationId, price);
        }

        public async Task<PriceV1> UpdatePriceAsync(string correlationId, PriceV1 price)
        {
            price.Priority = price.Priority ?? PriceV1.DEFAULT_PRIORITY;

            return await _persistence.UpdateAsync(correlationId, price);
        }

        public async Task<PriceV1> DeletePriceByIdAsync(string correlationId, string id)
        {
            return await _persistence.DeleteByIdAsync(correlationId, id);
        }
    }
}
