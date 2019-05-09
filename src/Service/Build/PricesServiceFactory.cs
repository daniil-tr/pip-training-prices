using PipServices3.Commons.Refer;
using PipServices3.Components.Build;
using Prices.Logic;
using Prices.Persistence;
using Prices.Services.Version1;

namespace Prices.Build
{
    public class PricesServiceFactory : Factory
    {
        public static Descriptor Descriptor = new Descriptor("prices", "factory", "service", "default", "1.0");
        public static Descriptor MemoryPersistenceDescriptor = new Descriptor("prices", "persistence", "memory", "*", "1.0");
        public static Descriptor MongoDbPersistenceDescriptor = new Descriptor("prices", "persistence", "mongodb", "*", "1.0");
        public static Descriptor ControllerDescriptor = new Descriptor("prices", "controller", "default", "*", "1.0");
        public static Descriptor HttpServiceDescriptor = new Descriptor("prices", "service", "http", "*", "1.0");


        public PricesServiceFactory()
        {
            RegisterAsType(MemoryPersistenceDescriptor, typeof(PricesMemoryPersistence));
            RegisterAsType(MongoDbPersistenceDescriptor, typeof(PricesMongoDbPersistence));
            RegisterAsType(ControllerDescriptor, typeof(PricesController));
            RegisterAsType(HttpServiceDescriptor, typeof(PricesHttpServiceV1));
        }
    }
}
