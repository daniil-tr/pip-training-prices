using PipServices3.Commons.Commands;
using PipServices3.Commons.Convert;
using PipServices3.Commons.Data;
using PipServices3.Commons.Validate;
using Prices.Data.Version1;

namespace Prices.Logic
{
    public class PricesCommandSet : CommandSet
    {
        private IPricesController _controller;

        public PricesCommandSet(IPricesController controller)
        {
            _controller = controller;

            AddCommand(MakeGetPricesCommand());
            AddCommand(MakeGetPriceByIdCommand());
            AddCommand(MakeGetPriceBySkuCommand());
            AddCommand(MakeCreatePriceCommand());
            AddCommand(MakeUpdatePriceCommand());
            AddCommand(MakeDeletePriceByIdCommand());
        }

        private ICommand MakeGetPricesCommand()
        {
            return new Command(
                "get_prices",
                new ObjectSchema()
                    .WithOptionalProperty("filter", new FilterParamsSchema())
                    .WithOptionalProperty("paging", new PagingParamsSchema()),
                async (correlationId, parameters) =>
                {
                    var filter = FilterParams.FromValue(parameters.Get("filter"));
                    var paging = PagingParams.FromValue(parameters.Get("paging"));
                    return await _controller.GetPricesAsync(correlationId, filter, paging);
                });
        }

        private ICommand MakeGetPriceByIdCommand()
        {
            return new Command(
                "get_price_by_id",
                new ObjectSchema()
                    .WithRequiredProperty("price_id", TypeCode.String),
                async (correlationId, parameters) =>
                {
                    var id = parameters.GetAsString("price_id");
                    return await _controller.GetPriceByIdAsync(correlationId, id);
                });
        }

        private ICommand MakeGetPriceBySkuCommand()
        {
            return new Command(
                "get_price_by_sku",
                new ObjectSchema()
                    .WithRequiredProperty("sku", TypeCode.String),
                async (correlationId, parameters) =>
                {
                    var sku = parameters.GetAsString("sku");
                    return await _controller.GetPriceBySkuAsync(correlationId, sku);
                });
        }

        private ICommand MakeCreatePriceCommand()
        {
            return new Command(
                "create_price",
                new ObjectSchema()
                    .WithRequiredProperty("price", new PriceV1Schema()),
                async (correlationId, parameters) =>
                {
                    var price = ConvertToPrice(parameters.GetAsObject("price"));
                    return await _controller.CreatePriceAsync(correlationId, price);
                });
        }

        private ICommand MakeUpdatePriceCommand()
        {
            return new Command(
               "update_price",
               new ObjectSchema()
                    .WithRequiredProperty("price", new PriceV1Schema()),
               async (correlationId, parameters) =>
               {
                   var price = ConvertToPrice(parameters.GetAsObject("price"));
                   return await _controller.UpdatePriceAsync(correlationId, price);
               });
        }

        private ICommand MakeDeletePriceByIdCommand()
        {
            return new Command(
               "delete_price_by_id",
               new ObjectSchema()
                   .WithRequiredProperty("price_id", TypeCode.String),
               async (correlationId, parameters) =>
               {
                   var id = parameters.GetAsString("price_id");
                   return await _controller.DeletePriceByIdAsync(correlationId, id);
               });
        }

        private PriceV1 ConvertToPrice(object value)
        {
            return JsonConverter.FromJson<PriceV1>(JsonConverter.ToJson(value));
        }

        private string[] ConvertToStringList(object value)
        {
            return JsonConverter.FromJson<string[]>(JsonConverter.ToJson(value));
        }
    }
}
