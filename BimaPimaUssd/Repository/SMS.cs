using BimaPimaUssd.Contracts;
using BimaPimaUssd.Helpers;
using BimaPimaUssd.Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace BimaPimaUssd.Repository
{
    public class SMS : IMessager
    {
        private readonly HttpHelper helper;
        public SMS(HttpHelper helper)
        {
            this.helper = helper;
        }
        public async Task<dynamic> SendMessage(string message, string To)
        {
            return await helper.ProcessPostRequest<string, SMS_OBJECT>(AppConstant.SMS_URL, payload: new SMS_OBJECT() { message = message, msisdn = To, source = "Digi farm", senderid = AppConstant.COMPANY });
        }

        public void SendMessage(string From, string message, string[] To)
        {
            throw new NotImplementedException();
        }

    }
    public class Payment : IPayment
    {
        string ConsumerKey = "INAfmzQFn2n6XlMvnn8dCoVpmT4Pm2L8";
        string ConsumerSecret = "8ZbWyeKsBGOnHLxG";
        private readonly HttpHelper helper;
        public Payment(HttpHelper helper)
        {
            this.helper = helper;
        }
        public async Task<dynamic> SendPayment(string phone, string amount,string reference)
        {
            return await helper.ProcessPostRequest<PaymentConfirmartion, MpesaPayment>(token:GetToken(ConsumerKey, ConsumerSecret).Result, AppConstant.PushSTKEP, payload: new MpesaPayment(amount, phone, DateTime.Now, reference));
        }

        private async Task<string> GetToken(string key, string secret)
        {
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(key +":"+ secret));
              var url = "https://api.safaricom.co.ke/oauth/v1/generate?grant_type=client_credentials";
           var result = await helper.ProcessGetRequest(url,credentials);
            return result;
        }

        //public void SendMessage(string From, string message, string[] To)
        //{
        //    throw new NotImplementedException();
        //}
    }
    
}
