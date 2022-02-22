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
    public class PaymentController : ControllerBase
    {
        readonly Repository<stkCallback> _service;
        public PaymentController(IStoreDatabaseSettings settings)
        {
            _service = new Repository<stkCallback>(settings, "ActivationPayment");
        }

        [HttpGet]
        public ActionResult<List<stkCallback>> Get() =>
            _service.Get();



        [HttpPost]
        public ActionResult<stkCallback> Create(stkCallback record)
        {
            _service.InsertRecord(record);
            return CreatedAtRoute("GetstkCallback", new { id = record.Id?.ToString() }, record);
        }

        [HttpGet("{id:length(24)}", Name = "GetstkCallback")]
        public ActionResult<stkCallback> Get(string id)
        {
            var record = _service.Get(id);

            if (record is null)
            {
                return NotFound();
            }
            return record;
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, stkCallback record)
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
