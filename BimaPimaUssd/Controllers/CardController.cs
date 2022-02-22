using BimaPimaUssd.Contracts;
using BimaPimaUssd.Models;
using BimaPimaUssd.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BimaPimaUssd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CardController : ControllerBase
    {
        readonly Repository<CardsSerial> _service;
        readonly Repository<Body> _MpesaService;
        public CardController(IStoreDatabaseSettings settings)
        {
            _service = new Repository<CardsSerial>(settings, "CardsSerial");
            _MpesaService = new Repository<Body>(settings, "ActivationPayment");
        }

        [HttpGet]
        public ActionResult<List<CardsSerial>> Get() =>
            _service.Get();

        [HttpPost("/api/callback")]
        public async Task<IActionResult> CallbackAsync(HttpResponseMessage response)

        {
            var res = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Body>(res);
           if(result != null)
                _MpesaService.InsertRecord(result);
            //HttpResponseMessage response = await client.PostAsync(url, data);
            //var res = await response.Content.ReadAsStringAsync();
            //var newObject = response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<T>(res) : default;

            return Ok();
        }

        [HttpPost("Pol")]
        public IActionResult CreateMany(List<CardsSerial> records)
        {
            _service.InsertMany(records);
          return  Ok();
        }

        [HttpPost]
        public ActionResult<CardsSerial> Create(CardsSerial record)
        {
            _service.InsertRecord(record);
            return CreatedAtRoute("GetCardsSerial", new { id = record.Id.ToString() }, record);
        }

        [HttpGet("{id:length(24)}", Name = "GetCardsSerial")]
        public ActionResult<CardsSerial> Get(string id)
        {
            var record = _service.Get(id);

            if (record is null)
            {
                return NotFound();
            }
            return record;
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, CardsSerial record)
        {
            var book = _service.Get(id);

            if (book is null)
            {
                return NotFound();
            }

            _service.Update(id, record);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var record = _service.Get(id);

            if (record is null)
            {
                return NotFound();
            }
            _service.Remove(id);

            return NoContent();
        }
    }
}
