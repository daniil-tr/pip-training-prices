using PipServices3.Commons.Refer;
using PipServices3.Rpc.Services;

namespace Prices.Services.Version1
{
    public class PricesHttpServiceV1 : CommandableHttpService
    {
        public PricesHttpServiceV1()        
            : base("v1/prices")
        {
            _dependencyResolver.Put("controller", new Descriptor("prices", "controller", "default", "*", "1.0"));
        }
    }
}
