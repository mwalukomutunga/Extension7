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

        public string _Name { get; }
        public FTMAModel(ServerResponse serverResponse, IRepository repository, IPayment paymnt, IStoreDatabaseSettings settings)
        {
            _Name = serverResponse.PhoneNumber;
            this.serverResponse = serverResponse;
            this.repository = repository;
            Payment = paymnt;
            levels = repository.levels[serverResponse.SessionId];
            _repository = new Repository<PBI>(settings, "PBIBioData");
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
                return value is null ? IFVM.LoadValueChains(serverResponse.PhoneNumber) : value.ToString();
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
                return IFVM.GetWeek();
            }
        }

        public string Pay
        {
            get
            {
              //  repository.Data[serverResponse.SessionId].Premium = repository.Requests[serverResponse.SessionId].Last.Value.Trim().ToString();
                levels.Push(11);
                return IFVM.SelectPayMethod();
            }
        }
        public string ConfirmPay
        {
            get
            {
                levels.Push(9);
                return IFVM.ConfirmPay(repository.Requests[serverResponse.SessionId].Last.Value switch
                {
                    "1" => 200,
                    _ => Convert.ToDecimal(repository.Requests[serverResponse.SessionId].Last.Value)
                },0,10);

                //return repository.Requests[serverResponse.SessionId].Last.Value switch
                //{
                //    "1" => CashConfirmPay(),
                //    "2" => MpesaConfirmPay(),
                //    _ => IFVM.Invalid
                //};
            }
        }
        //public string CashConfirmPay()
        //{
        //    levels.Push(12);
        //    return IFVM.ConfirmPay(Convert.ToDecimal(repository.Data[serverResponse.SessionId].Premium??0), 0);
        //}
        //public string MpesaConfirmPay()
        //{
        //    levels.Push(13);
        //    return IFVM.ConfirmPay(Convert.ToDecimal(repository.Data[serverResponse.SessionId].Premium ?? 0), 0);
        //}
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
                    "1" => IFVM.ProcessCash(),
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
                    "1" => IFVM.ProcessCash(),
                    "2" => EnterPhone,
                    _ => IFVM.Invalid
                };
            }
        }
        public string EnterAmount
        {
            get
            {
                levels.Push(10);
                return IFVM.GetAmount();
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
            Payment.SendPayment(phone, 1.ToString(), "Bima pima");
            return IFVM.ProcessMpesa();
        }
        public string ProcessMpesaPayment
        {
            get
            {///
                //add prompt to mpesa
                Payment.SendPayment(repository.Requests[serverResponse.SessionId].Last.Value.ToString(), 1.ToString(), "Bima pima");
                return IFVM.ProcessMpesa();
            }
        }

        private string ValidateFarmerCode(string code)
        {
            PBI value = _repository.GetByProperty("farmercode", code);
            repository.Data[serverResponse.SessionId].Value = value;
            if (value is null)
            {
                levels.Pop();
                levels.Push(3);
                return IFVM.InvalidCode;
            }
            return null;
        }

        private string ProcessNew()
        {
            return ProcessValueChains;
        }

        private string ProcessExisting()
        {
            levels.Push(3);
            return IFVM.CollectCode();
        }



    }
}
