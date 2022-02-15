namespace BimaPimaUssd.Models
{
    public class ServerResponse
    {
        //public string mobileNumber { get => phoneNumber.Trim().Replace("+", ""); set => phoneNumber = value; }
        //public string session_id { get; set; }
        //public string ussdCode { get; set; }
        //public string latitude { get; set; }
        //public string longitude { get; set; }

        //private string phoneNumber;
        //public string Text { get; set; }
        //public string PhoneNumber { get => phoneNumber.Trim().Replace("+", ""); set => phoneNumber = value; }
        //public string SessionId { get; set; }
        //public string ServiceCode { get; set; }
        private string phoneNumber;

      //  public string input { get; set; }
        public string mobileNumber { get => phoneNumber.Trim().Replace("+", ""); set => phoneNumber = value; }
        public string session_id { get; set; }
        public string ussdCode { get; set; }
        public string latitude { get; set; } 
        public string longitude { get; set; } 

        //live
        //public string mobileNumber { get; set; }
        //public string level { get; set; }
        //public string session_id { get; set; }
        //public string network { get; set; }
        //public string ussdCode { get; set; }
        private string _input;

        public string input
        {
            get { return System.Web.HttpUtility.UrlDecode(_input?.TrimStart('*')); }
            set { _input = value; }
        }
    }
    public class ResUssd
    {
        public ResUssd(string message,string session,string phone)
        {
            USSDMESSAGE = message;
            SESSIONID = session;
            MSISDN = phone;
        }
        private bool eND;

        public bool END => USSDMESSAGE.StartsWith("END");
        public string USSDMESSAGE { get; set; }
        public string SESSIONID { get; set; }
        public string MSISDN { get; set; }
    }
}
