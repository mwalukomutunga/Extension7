using BimaPimaUssd.Contracts;
using BimaPimaUssd.Helpers;
using BimaPimaUssd.Models;
using BimaPimaUssd.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BimaPimaUssd.ViewModels
{
    public class FTMAModel
    {
        private readonly ServerResponse serverResponse;
        private readonly IRepository repository;
        private readonly IPayment Payment;
        private readonly Stack<int> levels;
        private readonly Repository<PBI> _repository;
        readonly Repository<FarmerActivation> _service;
        readonly Repository<VC> _VC;

        public string _Name { get; }
        public FTMAModel(ServerResponse serverResponse, IRepository repository, IPayment paymnt, IStoreDatabaseSettings settings)
        {
            _Name = serverResponse.mobileNumber;
            this.serverResponse = serverResponse;
            this.repository = repository;
            Payment = paymnt;
            levels = repository.levels[serverResponse.session_id];
            _repository = new Repository<PBI>(settings, "PBIData");
            _VC = new Repository<VC>(settings, "VCData700");
            //_VC = new Repository<VC>(settings, "VCData7001"); test
            _service = new Repository<FarmerActivation>(settings, "FarmerActivation");
        }
        public string MainMenu => levels.Pop() switch
        {
            0 => PlantingMonth,
            1 => CheckExisting,
            2 => ChooseFarmerType,
            3 => ProcessCode,
            4 => GetMonth,
            5 => GetWeek,
            6 => EnterAmount,
            7 => EnterPhone,
            8 => GetProcessMpesaPayment().Result,
            9 => Pay,
            10 => ConfirmPay,
            11 => ConfirmPayOptions,
            12 => ProcessConfirmation,
            13 => ProcessMpesaConfirmation,
            14 => ProcessValueChains,
            15 => EnterCustomPhone,
            16 => PartialPay,
            17 => ProcessPartialPay,
            18 => CheckVC,
            19 => InputNo,
            _ => IFVM.Invalid
        };


        private string PlantingMonth
        {
            get
            {
                bool IsFirstTime = _VC.GetByProperty("Phone", serverResponse.mobileNumber) is null;
                if(IsFirstTime) levels.Push(18);
                else levels.Push(1);
                return IFVM.CheckWelcome();
            }
        }
        private string CheckExisting
        {
            get
            {
                levels.Push(2);
                bool IsFirstTime = _VC.GetByProperty("Phone", serverResponse.mobileNumber) is null;
                if (IsFirstTime)
                {
                    var str = ValidatePhone();
                    if (str is not null) return str;
                }
                return IFVM.CheckExisting();
            }
        }

        private string ValidatePhone()
        {
            var vc = repository.Requests[serverResponse.session_id].Last.Value;
            PBI pbi = _repository.GetByProperty("VCID", vc.ToUpper());
            if (pbi is null)
            {             
                levels.Pop();
                levels.Push(1);
                return IFVM.InvalidVC;
            }
            _VC.InsertRecord(new VC { Phone = serverResponse.mobileNumber, County = pbi .County});
            return null;
        }

        private string ChooseFarmerType
        {
            get
            {
                return repository.Requests[serverResponse.session_id].Last.Value switch
                {
                    "1" => ProcessExisting(),
                    "2" => ProcessNew(),
                    _ => IFVM.Invalid
                };
            }
        }

        public string ProcessCode
        {
            get
            {
                levels.Push(4);
                PBI pbi;
                var value = ValidateFarmerCode(repository.Requests[serverResponse.session_id].Last.Value.Trim().ToString());
                try
                {
                     pbi = (PBI)repository.Data[serverResponse.session_id].PBI;
                }
                catch (Exception)
                {

                    levels.Pop();
                    levels.Push(3);
                    return IFVM.InvalidCode;
                }
                return value is null ? IFVM.LoadValueChains(pbi.farmer_name) : value.ToString();
            }
        }
        public string ProcessValueChains
        {
            get
            {
                levels.Push(4);
                repository.Data[serverResponse.session_id].Phone = repository.Requests[serverResponse.session_id].Last.Value;
                return IFVM.LoadValueChains(repository.Data[serverResponse.session_id].Phone);
            }
        }

        public string GetMonth
        {
            get
            {
                levels.Push(5);
                return IFVM.TypeMonth();
            }
        }


        public string GetWeek
        {
            get
            {
                levels.Push(10);
                repository.Data[serverResponse.session_id].Month = repository.Requests[serverResponse.session_id].Last.Value.Trim().ToString();
                return IFVM.GetWeek();
            }
        }

        public string Pay
        {
            get
            {
                levels.Push(11);
                decimal value;
                try
                {
                    if (decimal.TryParse(repository.Requests[serverResponse.session_id].Last.Value, out value) && repository.Data[serverResponse.session_id].IsCustom) ;
                    {
                        if (value < 40)
                        {
                            levels.Pop();
                            levels.Push(9);
                            return IFVM.InvalidAmount;
                        }
                        repository.Data[serverResponse.session_id].Premium = value;
                    }
                }
                catch (Exception)
                {
                    return IFVM.SelectPayMethod();
                }
                return IFVM.SelectPayMethod();
            }
        }
        public string PartialPay
        {
            get
            {
                levels.Push(17);
                return IFVM.SelectParialPayment();
            }
        }
        public string ProcessPartialPay
        {
            get
            {
               
                return repository.Requests[serverResponse.session_id].Last.Value switch
                {
                    "1" => Pay,
                    "2" => ProcessPayInBits(),
                    _ => IFVM.Invalid
                };
            }
        }


        private string ProcessPayInBits()
        {
            levels.Push(9);
            repository.Data[serverResponse.session_id].IsCustom = true;
            return IFVM.PayCustom();
        }

        public string ConfirmPay
        {
            get
            {
                levels.Push(16);
                repository.Data[serverResponse.session_id].Week = repository.Requests[serverResponse.session_id].Last.Value.Trim().ToString();
                return IFVM.ConfirmPay(ProcessAmount(), GetSubsidy(), 10); 
            }
        }

        private decimal GetSubsidy()
        {
            if (repository.Data[serverResponse.session_id].Existing)
                return ((PBI)repository.Data[serverResponse.session_id].PBI).SubsidyAmount;
            else return 0;

        }

        private decimal ValidateAMount()
        {
            var val = Convert.ToDecimal(repository.Requests[serverResponse.session_id].Last.Value);
            if (val < 40)
            {
                repository.Data[serverResponse.session_id].IsSetWeek = true;
                levels.Pop();
                levels.Push(10);
                return 0;
            }
            repository.Data[serverResponse.session_id].Premium = val;
            return val;
        }

        private decimal ProcessAmount()
        {
            repository.Data[serverResponse.session_id].Premium = 200;
            return 200;
        }
        public string ProcessMpesaConfirmation
        {
            get
            {

                return repository.Requests[serverResponse.session_id].Last.Value switch
                {
                    "1" => EnterPhone,
                    "2" => IFVM.ProcessCancel(),
                    _ => IFVM.Invalid
                };
            }
        }


        public string ProcessConfirmation
        {
            get
            {
                levels.Push(9);
                return repository.Requests[serverResponse.session_id].Last.Value switch
                {
                    "1" => FinalizeCashPayment(),
                    "2" => IFVM.ProcessCancel(),
                    _ => IFVM.Invalid
                };
            }
        }

        public string ConfirmPayOptions
        {
            get
            {
                levels.Push(9);
                return repository.Requests[serverResponse.session_id].Last.Value switch
                {
                    "1" => FinalizeCashPayment(),
                    "2" => EnterPhone,
                    _ => IFVM.Invalid
                };
            }
        }
        public string FinalizeCashPayment()
        {
            //save record
            SaveFarmerActivation();
            return IFVM.ProcessCash();
        }
        public string EnterAmount
        {
            get
            {
                levels.Push(10);
                try
                {
                    if(!repository.Data[serverResponse.session_id].IsSetWeek)
                    { }
                }
                catch (Exception)
                {
                    repository.Data[serverResponse.session_id].Week = repository.Requests[serverResponse.session_id].Last.Value.Trim().ToString();
                    return IFVM.GetAmount(GetSubsidy());
                }
                   
                return IFVM.GetAmount(GetSubsidy());
            }
        }
        public string EnterPhone
        {
            get
            {
                levels.Push(15);
                return IFVM.GetPhone(serverResponse.mobileNumber);
            }
        }

        public string EnterCustomPhone
        {
            get
            {
                levels.Push(8);
                return repository.Requests[serverResponse.session_id].Last.Value switch
                {
                    "1" => GetProcessMpesaPayment(serverResponse.mobileNumber).Result,
                    "2" => IFVM.EnterMpesaNo(),
                    _ => IFVM.Invalid
                };
            }
        }

        public async Task<string> GetProcessMpesaPayment(string phone)
        {///
            //add prompt to mpesa
             var mpesares = await Payment.SendPayment(phone, Convert.ToDecimal(repository.Data[serverResponse.session_id].Premium).ToString(), "Bima pima");
            //var mpesares =  await Payment.SendPayment(phone, 1.ToString(), "Bima pima");
            SaveFarmerActivation(mpesares);
            return await Task.FromResult(IFVM.ProcessMpesa());
        }

        public async Task<string> GetProcessMpesaPayment()
        {
            //add prompt to mpesa

            //save record
            var phone = repository.Requests[serverResponse.session_id].Last.Value.ToString();
            if (phone.StartsWith("0")) phone = "254" + phone.Substring(1);
            var payment = await Payment.SendPayment(phone, Convert.ToDecimal(repository.Data[serverResponse.session_id].Premium).ToString(), "Bima pima");
            SaveFarmerActivation(payment);
            return await Task.FromResult(IFVM.ProcessMpesa());
        }

        public string CheckVC
        {
            get
            {
                levels.Push(1);
                return IFVM.EnterVCode();
            }
        }

        public string InputNo
        {
            get
            {
                levels.Push(14);
                return IFVM.EnterFarmerPhone();
            }
        }

        private void SaveFarmerActivation(PaymentConfirmartion mpesaPayment = null)
        {
            PBI pbi;
            if (repository.Data[serverResponse.session_id].Existing)
                pbi = (PBI)repository.Data[serverResponse.session_id].PBI;
            else
                pbi = new PBI();

            var record = new FarmerActivation
            {
                VC = pbi.VC,
                PlantingMonth = repository.Data[serverResponse.session_id].Month,
                PlantingWeek = repository.Data[serverResponse.session_id].Week,
                IsExisting = repository.Data[serverResponse.session_id].Existing,
                MainPhoneNumber = (bool)repository.Data[serverResponse.session_id].Existing? serverResponse.mobileNumber: repository.Data[serverResponse.session_id].Phone,
                farmercode = pbi.farmercode,
                UniqueCode = pbi.UniqueCode,
                farmer_name = pbi.farmer_name,
                County = pbi.County,
                VCID = pbi.VCID,
                SubsidyAmount = pbi.SubsidyAmount,
                InsurancePayment = pbi.InsurancePayment,
                PremiumPaid = Convert.ToDecimal(repository.Data[serverResponse.session_id].Premium) ??default,
                Rate = 10,
                DateActivated = DateTime.Now,
                Longitude = repository.Data[serverResponse.session_id].longitude.ToString(),
                Latitude = repository.Data[serverResponse.session_id].latitude.ToString(),
                MpesaRequest = mpesaPayment
            };
            _service.InsertRecord(record);
        }

        private string ValidateFarmerCode(string code)
        {
            PBI value = _repository.GetByProperty("UniqueCode", code);
            
            if (value is null)
            {
                levels.Pop();
                levels.Push(3);
                return IFVM.InvalidCode;
            }
            repository.Data[serverResponse.session_id].PBI = value;
            return null;
        }

        private string ProcessNew()
        {
            
            repository.Data[serverResponse.session_id].Existing = false; 
            return InputNo;
        }

        private string ProcessExisting()
        {
            levels.Push(3);
            repository.Data[serverResponse.session_id].Existing = true;
            return IFVM.CollectCode();
        }



    }
}
