using PipServices3.Commons.Config;
using PipServices3.Data.Persistence;
using Prices.Data.Version1;

namespace Prices.Persistence
{
    public class PricesFilePersistence : PricesMemoryPersistence
    {
        protected JsonFilePersister<PriceV1> _persister;

        public PricesFilePersistence()
        {
            _persister = new JsonFilePersister<PriceV1>();
            _loader = _persister;
            _saver = _persister;
        }

        public override void Configure(ConfigParams config)
        {
            base.Configure(config);
            _persister.Configure(config);
        }
    }
}
