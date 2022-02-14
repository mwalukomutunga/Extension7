using BimaPimaUssd.Contracts;
using BimaPimaUssd.Models;
using BimaPimaUssd.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace BimaPimaUssd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UssdController : ControllerBase
    {

        public IRepository _repository { get; }
        public IStoreDatabaseSettings Settings { get; }
        private readonly IPayment _payment;

        public UssdController(IRepository repository, IPayment payment, IStoreDatabaseSettings settings)
        {
            _repository = repository;
            _payment = payment;
            Settings = settings;
        }

        [HttpPost()]
        public async Task<IActionResult> PostAsync([FromForm] ServerResponse serverResponse)
        {
            serverResponse.Text = serverResponse.Text is null ? "" : serverResponse.Text;
            await ProcessSession(serverResponse);
            if(!string.IsNullOrEmpty(serverResponse.latitude))
            {
                _repository.Data[serverResponse.SessionId].latitude = serverResponse.latitude;
            }
            if (!string.IsNullOrEmpty(serverResponse.longitude))
            {
                _repository.Data[serverResponse.SessionId].longitude = serverResponse.longitude;
            }
            string response = await ProcessBima(serverResponse);
            return Ok(response);
        }

        private Task ProcessSession(ServerResponse serverResponse)
        {
            if (!_repository.levels.ContainsKey(serverResponse.SessionId))
            {
                _repository.Data.Add(serverResponse.SessionId, new ExpandoObject());
                _repository.levels.Add(serverResponse.SessionId, new System.Collections.Generic.Stack<int>());
                _repository.levels[serverResponse.SessionId].Push(0);
            }
            var lastInput = serverResponse.Text.Split("*").Last();
            if (!_repository.Requests.ContainsKey(serverResponse.SessionId)) _repository.Requests.Add(serverResponse.SessionId, new System.Collections.Generic.LinkedList<string>());
             _repository.Requests[serverResponse.SessionId].AddLast(lastInput);
            return Task.CompletedTask;
        }
        private Task<string> ProcessFTMA(ServerResponse serverResponse)
        {
            try
            {
                //var menu = new FTMAModel(serverResponse, _repository, _messager, Settings);
               // return Task.FromResult(menu.MainMenu);
               return Task.FromResult("");
            }
            catch (System.Exception e)
            {

               return Task.FromResult("END " + e.Message);
            }
        }
        private Task<string> ProcessBima(ServerResponse serverResponse)
        {
            try
            {
                var menu = new FTMAModel(serverResponse, _repository, _payment, Settings);
                return Task.FromResult(menu.MainMenu);
            }
            catch (System.Exception e)
            {

                return Task.FromResult("END " + e.Message);
            }
        }

    }
}
