using BimaPimaUssd.Contracts;
using BimaPimaUssd.Helpers;
using BimaPimaUssd.Models;
using BimaPimaUssd.Repository;
using System;
using System.Collections.Generic;

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

        public string _Name { get; }
        public FTMAModel(ServerResponse serverResponse, IRepository repository, IPayment paymnt, IStoreDatabaseSettings settings)
        {
            _Name = serverResponse.PhoneNumber;
            this.serverResponse = serverResponse;
            this.repository = repository;
            Payment = paymnt;
            levels = repository.levels[serverResponse.SessionId];
            _repository = new Repository<PBI>(settings, "PBIBioData");
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
            8 => ProcessMpesaPayment,
            9 => Pay,
            10 => ConfirmPay,
            11 => ConfirmPayOptions,
            12 => ProcessConfirmation,
            13 => ProcessMpesaConfirmation,
            14 => ProcessValueChains,
            15 => EnterCustomPhone,
            16 => PartialPay,
            17 => ProcessPartialPay,
            _ => IFVM.Invalid
        };


        private string PlantingMonth
        {
            get
            {
                levels.Push(1);
                return IFVM.CheckWelcome();
            }
        }
        private string CheckExisting
        {
            get
            {
                levels.Push(2);
                return IFVM.CheckExisting();
            }
        }
        private string ChooseFarmerType
        {
            get
            {
                return repository.Requests[serverResponse.SessionId].Last.Value switch
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
                var value = ValidateFarmerCode(repository.Requests[serverResponse.SessionId].Last.Value.Trim().ToString());
                var pbi = (PBI)repository.Data[serverResponse.SessionId].PBI;
                return value is null ? IFVM.LoadValueChains(pbi.farmer_name) : value.ToString();
            }
        }
        public string ProcessValueChains
        {
            get
            {
                levels.Push(4);
                return IFVM.LoadValueChains(serverResponse.PhoneNumber);
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
                levels.Push(6);
                repository.Data[serverResponse.SessionId].Month = repository.Requests[serverResponse.SessionId].Last.Value.Trim().ToString();
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
                    if (decimal.TryParse(repository.Requests[serverResponse.SessionId].Last.Value, out value) && repository.Data[serverResponse.SessionId].IsCustom) ;
                    {
                        if (value < 50)
                        {
                            levels.Pop();
                            levels.Push(9);
                            return IFVM.InvalidAmount;
                        }
                        repository.Data[serverResponse.SessionId].Premium = value;
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
               
                return repository.Requests[serverResponse.SessionId].Last.Value switch
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
            repository.Data[serverResponse.SessionId].IsCustom = true;
            return IFVM.PayCustom();
        }

        public string ConfirmPay
        {
            get
            {
                levels.Push(16);
                return IFVM.ConfirmPay(repository.Requests[serverResponse.SessionId].Last.Value switch
                {
                    "1" => ProcessAmount(),
                    _ => ValidateAMount() 
                }, GetSubsidy(), 10); 
            }
        }

        private decimal GetSubsidy()
        {
            if (repository.Data[serverResponse.SessionId].Existing)
                return ((PBI)repository.Data[serverResponse.SessionId].PBI).SubsidyAmount;
            else return 0;

        }

        private decimal ValidateAMount()
        {
            var val = Convert.ToDecimal(repository.Requests[serverResponse.SessionId].Last.Value);
            if (val < 50)
            {
                repository.Data[serverResponse.SessionId].IsSetWeek = true;
                levels.Pop();
                levels.Push(10);
                return 0;
            }
            repository.Data[serverResponse.SessionId].Premium = val;
            return val;
        }

        private decimal ProcessAmount()
        {
            repository.Data[serverResponse.SessionId].Premium = repository.Requests[serverResponse.SessionId].Last.Value.Trim().ToString();
            return 200;
        }
        public string ProcessMpesaConfirmation
        {
            get
            {

                return repository.Requests[serverResponse.SessionId].Last.Value switch
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
                return repository.Requests[serverResponse.SessionId].Last.Value switch
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
                return repository.Requests[serverResponse.SessionId].Last.Value switch
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
                    if(!repository.Data[serverResponse.SessionId].IsSetWeek)
                    { }
                }
                catch (Exception)
                {
                    repository.Data[serverResponse.SessionId].Week = repository.Requests[serverResponse.SessionId].Last.Value.Trim().ToString();
                    return IFVM.GetAmount(GetSubsidy().ToString());
                }
                   
                return IFVM.GetAmount(GetSubsidy().ToString());
            }
        }
        public string EnterPhone
        {
            get
            {
                levels.Push(15);
                return IFVM.GetPhone(serverResponse.PhoneNumber);
            }
        }

        public string EnterCustomPhone
        {
            get
            {
                levels.Push(8);
                return repository.Requests[serverResponse.SessionId].Last.Value switch
                {
                    "1" => GetProcessMpesaPayment(serverResponse.PhoneNumber),
                    "2" => IFVM.EnterMpesaNo(),
                    _ => IFVM.Invalid
                };
            }
        }

        public string GetProcessMpesaPayment(string phone)
        {///
            //add prompt to mpesa
            Payment.SendPayment(phone, Convert.ToDecimal(repository.Data[serverResponse.SessionId].Premium).ToString(), "Bima pima");
            return IFVM.ProcessMpesa();
        }
        public string ProcessMpesaPayment
        {
            get
            {
                //add prompt to mpesa
                SaveFarmerActivation();
                //save record
                Payment.SendPayment(repository.Requests[serverResponse.SessionId].Last.Value.ToString(), Convert.ToDecimal(repository.Data[serverResponse.SessionId].Premium).ToString(), "Bima pima");
                return IFVM.ProcessMpesa();
            }
        }

        private void SaveFarmerActivation()
        {
            PBI pbi;
            if (repository.Data[serverResponse.SessionId].Existing)
                pbi = (PBI)repository.Data[serverResponse.SessionId].PBI;
            else
                pbi = new PBI();

            var record = new FarmerActivation
            {
                VC = pbi.VC,
                PlantingMonth = repository.Data[serverResponse.SessionId].Month,
                PlantingWeek = repository.Data[serverResponse.SessionId].Week,
                IsExisting = repository.Data[serverResponse.SessionId].Existing,
                MainPhoneNumber = serverResponse.PhoneNumber,
                farmercode = pbi.farmercode,
                UniqueCode = pbi.UniqueCode,
                farmer_name = pbi.farmer_name,
                County = pbi.County,
                VCID = pbi.VCID,
                SubsidyAmount = pbi.SubsidyAmount,
                InsurancePayment = pbi.InsurancePayment,
                PremiumPaid = Convert.ToDecimal(repository.Data[serverResponse.SessionId].Premium) ??default,
                Rate = 10,
                DateActivated = DateTime.Now,
                Longitude = repository.Data[serverResponse.SessionId].longitude.ToString(),
                Latitude = repository.Data[serverResponse.SessionId].latitude.ToString(),
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
            repository.Data[serverResponse.SessionId].PBI = value;
            return null;
        }

        private string ProcessNew()
        {
            repository.Data[serverResponse.SessionId].Existing = false; 
            return ProcessValueChains;
        }

        private string ProcessExisting()
        {
            levels.Push(3);
            repository.Data[serverResponse.SessionId].Existing = true;
            return IFVM.CollectCode();
        }



    }
}
