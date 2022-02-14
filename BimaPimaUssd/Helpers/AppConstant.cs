using System;

namespace BimaPimaUssd.Helpers
{
    public class AppConstant
    {
        public static readonly string Header = "application/json";
        public static readonly string Env = "ussdtest";
        public static readonly string Tenant = "acre";
        public static readonly string Url = "https://openapi-staging.etherisc.com";
        public static readonly string SMS_URL = "http://167.172.47.173/acresms/sms.php";
        public static readonly string PushSTKEP = "https://api.safaricom.co.ke/mpesa/stkpush/v1/processrequest";
        public static readonly string TOKEN_URL = "https://api.safaricom.co.ke/oauth/v1/generate?grant_type=client_credentials";
        public static readonly string REVERSE_GEOCODE_URL = "https://maps.googleapis.com/maps/api/geocode/json";
        public static readonly string BASE_URL_FARMER = "https://farmer-api.agrismart.co.ke";
        public static readonly string COMPANY = "ACRE_AFRICA";
        public static readonly string Country = "KE";
        public static readonly string Currency = "KES";

        public static int PayBill = 697744;
        public static readonly string PassKey = "efa477b21f09a30aed4735658f4a35d736e24a7d53f3a61552501834fce70571";

        public static int GetLastDayOfWeek(int Month, int week)
        {
            return week switch
            {
                1 => 7,
                2 => 14,
                3 => 21,
                4 => 28,
                5 => new DateTime(DateTime.Now.Year, Month + 1, 1).AddDays(-1).Day,
                _ => 30,
            };


        }
    }
   
    public static class IFVM
    {
        #region 70

        public static string Invalid => "END Inavid entry. Please try again.";
        public static string InvalidCode => "CON Inavid farmer code. Please try again.";

        //common
        internal static string ThankFarmer()
        {
            throw new NotImplementedException();
        }

        internal static string CheckExisting() => "CON Welcome to our service.\n1.Existing farmer.\n2. New farmer";
        internal static string CheckWelcome() => "CON Welcome back\n 1. Continue";



        internal static string PlantingWeek()
        {
            throw new NotImplementedException();
        }


        internal static string CollectCode() => "CON Enter farmer code.\n";

        internal static string LoadValueChains(string name) => $"CON Cheers {name}!! Select crop\n 1. Maize\n2. Greengrams\n 3. Sorghum\n 4. Beans";

        internal static string TypeMonth() => "CON Select planting month.\n 1. January\n2. February\n3.March \n4. April \n5. May\n6. June\n7. July\n8.August\n9. September \n10. October \n11. November \n12.December";

        internal static string GetWeek() => "CON Select planting week\n 1. 1st week\n2. 2nd week\n3. 3rd week\n4. 4th week\n5. 5th week";

        internal static string SelectPayMethod() => $"CON Select payment method.\n 1. Cash\n 2. Mpesa";
        internal static string ConfirmPay(decimal premium, decimal subsidy,int rate ) => $"CON Confirm. Pay Kes {premium - subsidy } for max KES {premium * rate} payout \n 1. Confirm\n 2. Cancel";

        internal static string ProcessCash() => $"END Thank you for using our service. Find more details in your SMS";
        internal static string ProcessCancel() => $"END Thank you for using our service. Feel free to try again.";

        internal static string ProcessMpesa() => $"END Thank you\nEnter your MPESA PIN in the next prompt to complete the request";

        internal static string GetAmount() => "CON  Select insurance cover\n 1. Pay Kes 200 for max KES 2000 payout\n 2. Pay amount above Kes 200";
        internal static string GetPhone(string phone) => $"CON Select phone number to pay\n 1. {phone}\n 2. Enter new number.";

        internal static string EnterMpesaNo() => "CON ENter phone number";
        #endregion 70


        //common
        #region Common
        internal static string County => "CON Please enter your County";

        internal static string DisAgree => $"END Sorry, you have to agree to the terms and conditions to proceed with this service.\n";

       // internal static string Invalid => $"END Invalid choice. Please try again.\n";
        internal static string ToImplement => $"END Coming soon.\n";

        internal static string Terms => $"END Kindly visit https://acreafrica.com/ and read our terms and conditions.\n";

        internal static string Village => "CON Please enter your village name";

        internal static string Ward => "CON Please enter your ward";

        internal static string SubCounty => "CON Please enter your sub county";
        #endregion Common
        /// <summary>
        /// Farm input sectioN
        /// </summary>
        #region FarmInputs
        public static string Inputs => "CON Insured farm inputs\n1. Buy farm input\n2. Top up farm input";

        internal static string FarmMSg() => $"CON Thank you for enrolling with Digifarm to get the seeds in the short rains of 2021";
        internal static string PlantingMonth() => $"CON Thank you for enrolling with Digifarm to get the seeds in the short rains of 2021\nWhich month did you plant?\n1.August\n2. Sep\n3. Oct\n4. Nov\n5. Dec";
        internal static string ActivationGratitude() => $"END Dear customer, thank you for activating your crop insurance cover\n";
       // internal static string InvalidCode() => $"END The provided code is invalid. Please try again using a valid code!";
       // internal static string PlantingWeek() => $"CON Which week of the month did you plant?\n1. 1st Week\n2. 2nd Week\n3. 3rd Week\n4. 4th Week\n5. 5th Week";
        internal static string NearBy() => $"CON What is the nearest primary school, to where you planted?";
      //  internal static string ThankFarmer() => $"END Jambo! Thank you for enrolling with Digifarm.";
        public static string InputCategory => "CON Select input category to buy\n";
        public static string FarmInput => "CON Select farm input\n";
        public static string SelectMonth => "CON Please select your scheduled planting month\n";
        public static string SelectWeek => "CON Please select your scheduled planting week\n";
        public static string SelectCenter => "CON Please select collection center\n";
        public static string CustomAmount => "CON Enter the amount to pay in KES\n";
        public static string ConfirmCustomAmount(string curr, string currAmount, string crop, string purchasePrice) => $"CON Confirm pay {curr} {currAmount} now for {crop} worth KES {purchasePrice}\n1. Confirm\n2. Cancel";
        public static string Week => "CON Select Week of Month \n1. 1st Week \n2. 2nd Week \n3. 3rd Week \n4. 4th Week\n";
        public static string Pay => "CON Pay for this order \n1. Pay now \n2. Pay in bits \n3. Cancel\n";
        public static string GetConfirmOrder(int NoOfPacs, string crop, string type, string currency, string amount) => $"CON Confirm Buy {NoOfPacs} packet(s) of {type} {crop} worth {currency} {amount}\n1. Yes\n2. No";
        public static string GetSelectPackage(string package, string unit) => $"CON Please enter the quantity of {package} {unit} packets you would like to purchase\n";
        #endregion FarmInputs
        //user
        #region User
        internal static string StartRegistrationMsg = "CON Welcome to Acre Africa. Reply with\nPlease type your name\n";
        internal static readonly string RegisterSuccessMsg = "END Thank you for registering with FtMA.";
        internal static readonly string ServiceCodeMsg = "CON Please enter 6 digit  servide code";
        internal static string WelcomeMsg = $"CON Welcome to Acre Africa. Kindly read and agree to the terms and conditions before you proceed.\n1. Agree\n2. Disagree\n3. Read terms.\n";
        #endregion User
        //  FSCS
        #region FSCS
        //internal static string DetailsMsg = "END Thank you for using our service. You will receive more details on SMS";
        //internal static string FarmCode(string name) => $"CON Welcome { name} \nEnter your Farmer Service centre code (8001-8999)";
        //internal static string ServiceOptions => "CON What would you like to do?\n1. Link to farmer service center.\n2 Update associated FSC\n";
        //internal static string LoadBusinessStreams => "CON Select business stream.\n1. Input financing.\n2 Markets/Aggregation\n";
        //internal static string SelectFSCs => "CON Select one of these available FSCs.\n1. Mary Wanjohi 0711223344 maize, wheat .\n2. George Kinoti 07134889900\n";
        #endregion FSCS

        #region Bima
        internal static string BimaWelcomeMsg => $"CON Welcome to Acre africa.\n1. Insured farm inputs\n2. Bima pima insurance\n 3. Farmer's account\n 4. Farming advice\n5. Input financing";
       // internal static string PayMsg(string amount, InsuranceProduct item) => $"CON Confirm pay for first installment of KES {amount} for {item.Crop} for a maximum payout of KES {int.Parse(amount) * item.Premium_rate} incase of extreme weather events.\n1. Pay now\n2. Pay in bits";
       // internal static string ProcessCancel => "END Thank you for using our service. Welcome again";
        internal static string BitsMsg => "CON Enter amount(KES), start with KES 50 or more\n";
        internal static string FinalizePayment => $"END Thank you. Please enter mpesa pin on the next prompt.\n";
        internal static string Weeks => "CON Select Week of Month \n1. 1st Week \n2. 2nd Week \n3. 3rd Week \n4. 4th Week\n";
        internal static string PolicyDetailsMsg => "END Thank you for using our service.\nAn SMS about this order will be send to you.\n";
        internal static string PolicyOptionsMsg => $"CON What would like to do?\n1. View order details.\n2. View claims\n";
        internal static string PolictCover => "CON Select one of these crops for the cover.\n";
        internal static string ActivationCodeMsg => "CON Enter your 6-digit activation code.\n";
        internal static string ValidCodeMsg => "CON Invalid code! Please try again.";
        internal static string ExpiredCoverMsg => $"END Sorry, this cover is expired";
        internal static string RestError => "END Internal error occured. Kindly try again later.";
        //internal static string GetActivationSuccessMsg(Activation activation) => $"END You have successfully activated cover for {activation.crop} for maximum payout of KES {activation.premium_amount} incase of extreme weather conditions.\n";

        internal static string ActivationSms => "Dear customer, thank you for activating your crop insurance cover.";

        internal static string FailedPaymentOnActivation => "END Kindly note that the payment has not been activated";
        internal static string BimaActions(string name) => $"CON Welcome { name} \n Reply with\n 1. Buy Bima pima insurance \n 2. Activate Bima pima insurance \n 3 .Top up your Bima insurance.\n";
        //internal static string LoadClaims(ActivePolicy policy, Common common) => policy is null ? "END Could not find the claim." : common.ProcessClaims(policy.Order_no);
        //internal static string LoadCropCovers(InsuranceProduct item) => $"CON Select cover\n1. Pay KES 50 for max KES {50 * item.Premium_rate} payout\n2. Pay KES 100 for max KES {100 * item.Premium_rate} payout\n3. Pay KES 500 for max KES {500 * item.Premium_rate} payout\n4. Pay above KES 500\n";
        //internal static string SelectCoverMsg(float amt, InsuranceProduct item) => $"CON Buy bima pima for {item.Crop} KES {amt} for a maximum payout of KES {amt * item.Premium_rate} in case of extreme weather events?\n1. Pay now\n2. Pay in bits\n";
        //internal static string PayConfirmMsg(string amount, InsuranceProduct item) => $"CON Confirm pay for first installment of KES {amount} for {item.Crop} for a maximum payout of KES {int.Parse(amount) * item.Premium_rate} incase of extreme weather events.\n1. Confirm\n2. Cancel";
        #endregion Bima

        //adviisory

        //internal static string welcometoAdvisory => "CON Welcome to our advisory service.\n1. What to grow in? \n2. Where to grow?\n";
        //internal static string WhatToGrow => "CON Enter what to grow in?\n";
        //internal static string whereToGrow => "CON Enter where to grow in?\n";
        //internal static string MissingLocation => "END Sorry, unable to get location \n";
        //internal static string AdviceOptions => "CON Invalid Input. Select \n1. What to grow in? \n2. Where to grow? \n";
        //internal static string WhereToGrow => "Where to grow? \n";

    }
}
