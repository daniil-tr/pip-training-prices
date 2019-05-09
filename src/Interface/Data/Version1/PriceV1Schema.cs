using PipServices3.Commons.Convert;
using PipServices3.Commons.Validate;

namespace Prices.Data.Version1
{
    public class PriceV1Schema : ObjectSchema
    {
        public PriceV1Schema()
        {
            WithOptionalProperty("id", TypeCode.String);
            WithRequiredProperty("pricefile_id", TypeCode.String);
            WithRequiredProperty("external_ref_id", TypeCode.String);
            WithRequiredProperty("product_id", TypeCode.String);
            WithRequiredProperty("part_id", TypeCode.String);
            WithOptionalProperty("sku", TypeCode.String);
            WithOptionalProperty("price_net", TypeCode.Double);
            WithOptionalProperty("price_net_full", TypeCode.Double);
            WithOptionalProperty("price_retail", TypeCode.Double);
            WithOptionalProperty("price_retail_full", TypeCode.Double);
            WithOptionalProperty("date_start", TypeCode.DateTime);
            WithOptionalProperty("date_end", TypeCode.DateTime);
            WithOptionalProperty("promo_code", TypeCode.String);
            //WithOptionalProperty("priority", TypeCode.Integer);
            WithOptionalProperty("priority", TypeCode.Long);
            WithOptionalProperty("note", TypeCode.String);
        }
    }
}
