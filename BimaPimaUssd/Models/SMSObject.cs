using BimaPimaUssd.Helpers;
using System;
using System.Text;

namespace BimaPimaUssd.Models
{
    public class SMS_OBJECT
    {
        public string message { get; set; }
        public string msisdn { get; set; }
        public string source { get; set; }
        public string senderid { get; set; }
    }
    public class MpesaPayment
    {
        public MpesaPayment(string amount, string phoneNumber, DateTime time,string reference)
        {
            BusinessShortCode = AppConstant.PayBill.ToString();
            PartyB = AppConstant.PayBill.ToString();
            Amount = amount;
            Timestamp = time.ToString("YYYYMMDDHHmmss");
            Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(AppConstant.PayBill + AppConstant.PassKey + time.ToString("YYYYMMDDHHmmss")));
            TransactionType = "CustomerPayBillOnline";
            PartyA = phoneNumber;
            PhoneNumber = phoneNumber;
            CallBackURL = AppConstant.Callback;
            AccountReference = reference;
            TransactionDesc = "Activation payment";

        }

        public string BusinessShortCode { get; set; }
        public string Password { get; set; }
        public string TransactionType { get; set; }
        public string Timestamp { get; set; }
        public string PartyA { get; set; }
        public string PartyB { get; set; }
        public string Amount { get; set; }
        public string PhoneNumber { get; set; }
        public string CallBackURL { get; set; }
        public string AccountReference { get; set; }
        public string TransactionDesc { get; set; }
    }


    public class PaymentConfirmartion
    {
        public string MerchantRequestID { get; set; }
        public string CheckoutRequestID { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
        public string CustomerMessage { get; set; }
    }
    public class TokenRequest
    {
        public string Credentials { get; set; }
        public TokenRequest(string key,  string secret )
        {
            Credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(key + secret));
        }
    }
}
