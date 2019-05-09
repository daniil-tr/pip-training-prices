using PipServices3.Container;
using PipServices3.Rpc.Build;
using Prices.Build;

namespace Prices.Container
{
    public class PricesProcess : ProcessContainer
    {
        public PricesProcess()
            : base("prices", "Prices microservice")
        {
            _factories.Add(new DefaultRpcFactory());
            _factories.Add(new PricesServiceFactory());
        }
    }
}
