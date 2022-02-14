using BimaPimaUssd.Helpers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace BimaPimaUssd.Models
{
    public class Reg_User
    {
        public bool IsLogged { get; set; }
        public string Token { get; set; }

    }

    public class Activation
    {
        public Activation(string exten)
        {
            DateActivated = DateTime.Now;
            Extension = exten;
            PaymentAmount =0;
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string? Id { get; set; }
        public string Extension { get; set; }
        public decimal PaymentAmount { get; set; }

        public string PhoneNumber { get; set; }
        public string VoucherCode { get; set; }
        public string ValueChain { get; set; }
        public string SerialNumber { get; set; }
        public string Denomination { get; set; }
        [JsonPropertyName("Project name")]
        public string Projectname { get; set; }
        public string ProductName { get; set; }
        public DateTime DateActivated { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

    }
    public class PBI
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string? Id { get; set; }
        [JsonPropertyName("County")]
        public string County { get; set; }
        [JsonPropertyName("VC ID")]
        public string VCID { get; set; }
        [JsonPropertyName("VC")]
        public string VC { get; set; }
        [JsonPropertyName("farmerCode")]
        public string farmercode { get; set; }
        [JsonPropertyName("UniqueCode")]
        public string UniqueCode { get; set; }
        [JsonPropertyName("farmer_name")]
        public string farmer_name { get; set; }
        [JsonPropertyName("MainPhoneNumber")]
        public string MainPhoneNumber { get; set; }
        [JsonPropertyName("subsidy")]
        public string subsidy { get; set; }
        [JsonPropertyName("SubsidyAmount")]
        public decimal SubsidyAmount { get; set; }
        [JsonPropertyName("insurancePayment")]
        public decimal InsurancePayment { get; set; }
        [JsonPropertyName("Rate")]
        public int Rate { get; set; }
    }
    public class CardsSerial
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string? Id { get; set; }
        [JsonPropertyName("Value Chain")]
        public string ValueChain { get; set; }
        [JsonPropertyName("Voucher Code")]
        public string VoucherCode { get; set; }
        [JsonPropertyName("Serial Number")]
        public string SerialNumber { get; set; }
        [JsonPropertyName("Denomination")]
        public string Denomination { get; set; }
        [JsonPropertyName("Project name")]
        public string Projectname { get; set; }
        [JsonPropertyName("Product Name")]
        public string ProductName { get; set; }
     
    }

    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore]
        public string? Id { get; set; }
        public string Msisdn { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string PlantingMonth { get; set; }
        public string PlantingWeek { get; set; }
        //public DateTime ComputedPlantingDate 
        //{
        //    get
        //    {
        //        return new DateTime(DateOfQuery.Year, Convert.ToInt32(PlantingMonth),AppConstant.GetLastDayOfWeek(Convert.ToInt32(PlantingMonth), Convert.ToInt32(PlantingWeek)));
        //    }
        //}

        public string NearestPrimarySchool { get; set; }
        public DateTime DateOfQuery { get; set; }



    }

}
