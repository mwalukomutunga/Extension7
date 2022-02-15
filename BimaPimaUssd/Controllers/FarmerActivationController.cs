using BimaPimaUssd.Contracts;
using BimaPimaUssd.Models;
using BimaPimaUssd.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BimaPimaUssd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmerActivationController : ControllerBase
    {
        readonly Repository<FarmerActivation> _service;
        readonly Repository<stkCallback> _MpesaService;
        public FarmerActivationController(IStoreDatabaseSettings settings)
        {
            _service = new Repository<FarmerActivation>(settings, "FarmerActivation");
            _MpesaService = new Repository<stkCallback>(settings, "ActivationPayment70");
        }

        [HttpGet]
        public ActionResult<List<FarmerActivation>> Get() =>
            _service.Get();

        //[HttpPost("/api/callback")]
        //public IActionResult Callback(stkCallback record)

        //{
        //    _MpesaService.InsertRecord(new stkCallback());
        //    return Ok();
        //}

        [HttpPost]
        public ActionResult<FarmerActivation> Create(FarmerActivation record)
        {
            _service.InsertRecord(record);
            return CreatedAtRoute("GetFarmerActivation", new { id = record.Id.ToString() }, record);
        }

        [HttpGet("{id:length(24)}", Name = "GetFarmerActivation")]
        public ActionResult<FarmerActivation> Get(string id)
        {
            var record = _service.Get(id);

            if (record is null)
            {
                return NotFound();
            }
            return record;
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, FarmerActivation record)
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
