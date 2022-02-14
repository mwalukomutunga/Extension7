using BimaPimaUssd.Contracts;
using BimaPimaUssd.Models;
using BimaPimaUssd.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Dynamic;

namespace BimaPimaUssd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CardController : ControllerBase
    {
        readonly Repository<CardsSerial> _service;
        readonly Repository<stkCallback> _MpesaService;
        public CardController(IStoreDatabaseSettings settings)
        {
            _service = new Repository<CardsSerial>(settings, "CardsSerial");
            _MpesaService = new Repository<stkCallback>(settings, "ActivationPayment");
        }

        [HttpGet]
        public ActionResult<List<CardsSerial>> Get() =>
            _service.Get();

        [HttpPost("/api/callback")]
        public IActionResult Callback(stkCallback record)
        
        {
            _MpesaService.InsertRecord(new stkCallback());            
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
