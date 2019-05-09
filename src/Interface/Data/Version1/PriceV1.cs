using PipServices3.Commons.Data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Prices.Data.Version1
{
    [DataContract]
    public class PriceV1 : IStringIdentifiable
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "pricefile_id")]
        public string PriceFileId { get; set; }

        [DataMember(Name = "external_ref_id")]
        public string ExternalRefId { get; set; }

        [DataMember(Name = "product_id")]
        public string ProductId { get; set; }

        [DataMember(Name = "part_id")]
        public string PartId { get; set; }

        [DataMember(Name = "sku")]
        public string Sku { get; set; }

        [DataMember(Name = "price_net")]
        public double PriceNet { get; set; }

        [DataMember(Name = "price_net_full")]
        public double PriceNetFull { get; set; }

        [DataMember(Name = "price_retail")]
        public double PriceRetail { get; set; }

        [DataMember(Name = "price_retail_full")]
        public double PriceRetailFull { get; set; }

        [DataMember(Name = "date_start")]
        public DateTime DateStart { get; set; }

        [DataMember(Name = "date_end")]
        public DateTime DateEnd { get; set; }

        [DataMember(Name = "promo_code")]
        public string PromoCode { get; set; }

        //[DataMember(Name = "priority")]
        //public int Priority { get; set; }

        [DataMember(Name = "priority")]
        public long? Priority { get; set; }

        [DataMember(Name = "note")]
        public string Note { get; set; }

        public const int DEFAULT_PRIORITY = 100;

        public override bool Equals(object obj)
        {
            var v = obj as PriceV1;
            bool result = v != null &&
                   Id == v.Id &&
                   PriceFileId == v.PriceFileId &&
                   ExternalRefId == v.ExternalRefId &&
                   ProductId == v.ProductId &&
                   PartId == v.PartId &&
                   Sku == v.Sku &&
                   PriceNet == v.PriceNet &&
                   PriceNetFull == v.PriceNetFull &&
                   PriceRetail == v.PriceRetail &&
                   PriceRetailFull == v.PriceRetailFull &&
                   DateStart == v.DateStart &&
                   DateEnd == v.DateEnd &&
                   PromoCode == v.PromoCode &&
                   //EqualityComparer<int?>.Default.Equals(Priority, v.Priority) &&
                   EqualityComparer<long?>.Default.Equals(Priority, v.Priority) &&
                   Note == v.Note;
            return result;
        }

        public override int GetHashCode()
        {
            var hashCode = 77621969;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PriceFileId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ExternalRefId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ProductId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PartId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Sku);
            hashCode = hashCode * -1521134295 + PriceNet.GetHashCode();
            hashCode = hashCode * -1521134295 + PriceNetFull.GetHashCode();
            hashCode = hashCode * -1521134295 + PriceRetail.GetHashCode();
            hashCode = hashCode * -1521134295 + PriceRetailFull.GetHashCode();
            hashCode = hashCode * -1521134295 + DateStart.GetHashCode();
            hashCode = hashCode * -1521134295 + DateEnd.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PromoCode);
            //hashCode = hashCode * -1521134295 + EqualityComparer<int?>.Default.GetHashCode(Priority);
            hashCode = hashCode * -1521134295 + EqualityComparer<long?>.Default.GetHashCode(Priority);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Note);
            return hashCode;
        }
    }
}
