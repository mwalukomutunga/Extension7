using BimaPimaUssd.Contracts;
using BimaPimaUssd.Helpers;
using BimaPimaUssd.Models;
using BimaPimaUssd.Repository;
using System.Collections.Generic;

namespace BimaPimaUssd.ViewModels
{
    public class ActivationViewModel
    {
        //readonly Repository<CardsSerial> _service;
        //public CardController(IStoreDatabaseSettings settings)
        //{
        //    _service = new Repository<CardsSerial>(settings, "CardsSerial");
        //}


        private readonly ServerResponse serverResponse;
        private readonly IRepository repository;
        private readonly IPayment Payment;
        private readonly Stack<int> levels;
        private readonly Repository<Activation> _repository;
        readonly Repository<CardsSerial> _service;

        public string _Name { get; }

        public ActivationViewModel(ServerResponse serverResponse, IRepository repository, IPayment paymnt, IStoreDatabaseSettings settings)
        {
            _Name = serverResponse.PhoneNumber;
            this.serverResponse = serverResponse;
            this.repository = repository;
            this.Payment = paymnt;
            levels = repository.levels[serverResponse.SessionId];
            _repository = new Repository<Activation>(settings, "Activations");
            _service = new Repository<CardsSerial>(settings, "CardsSerial");

        }
        public string MainMenu => levels.Pop() switch
        {
            0 => MpesaCodeActivation,
            1 => MpesaCodeActivation,
            _ => IFVM.Invalid
        };
        //private string PlantingMonth
        //{
        //    get
        //    {
        //        levels.Push(1);
        //        return "CON ";
        //    }
        //}

        private string MpesaCodeActivation
        {
            get
            {
                var str = repository.Requests[serverResponse.SessionId].Last.Value;
                if (string.IsNullOrEmpty(str)) return IFVM.InvalidCode;
                var value = _service.GetByProperty("VoucherCode", str);
                if (value is not null)
                {
                    //add mpesa
                    //value.Denomination
                    Payment.SendPayment(serverResponse.PhoneNumber, value.Denomination.ToString(),value.ProductName);

                    var activation = new Activation("50")
                    {
                        PhoneNumber = serverResponse.PhoneNumber,
                        Longitude = repository.Data[serverResponse.SessionId].longitude.ToString(),
                        Latitude = repository.Data[serverResponse.SessionId].latitude.ToString(),
                        ValueChain = value.ValueChain,
                        VoucherCode = value.VoucherCode,
                        SerialNumber = value.SerialNumber,
                        Denomination = value.Denomination,
                        ProductName = value.ProductName,
                        Projectname = value.Projectname,
                        PaymentAmount = decimal.Parse(value.Denomination)
                    };
                    _repository.InsertRecord(activation);
                    return IFVM.ActivationGratitude();
                }

                else return IFVM.InvalidCode;

            }
        }
        private string DirectCodeActivation
        {
            get
            {
                var str = repository.Requests[serverResponse.SessionId].Last.Value;
                if (string.IsNullOrEmpty(str)) return IFVM.InvalidCode;
                var value = _service.GetByProperty("VoucherCode", str);
                if (value is not null)
                {
                    var activation = new Activation("55")
                    {
                        PhoneNumber = serverResponse.PhoneNumber,
                        Longitude = repository.Data[serverResponse.SessionId].longitude.ToString(),
                        Latitude = repository.Data[serverResponse.SessionId].latitude.ToString(),
                        ValueChain = value.ValueChain,
                        VoucherCode = value.VoucherCode,
                        SerialNumber = value.SerialNumber,
                        Denomination = value.Denomination,
                        ProductName = value.ProductName,    
                        Projectname = value.Projectname,
                    };
                    _repository.InsertRecord(activation);
                         return IFVM.ActivationGratitude();
                }
                   
               else return IFVM.InvalidCode;

            }
        }
        private string PlantingMonth2
        {
            get
            {
                levels.Push(2);
                return IFVM.InvalidCode;
            }
        }

    }
}
