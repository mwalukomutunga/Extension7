using BimaPimaUssd.Contracts;
using BimaPimaUssd.Models;
using BimaPimaUssd.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BimaPimaUssd.Controllers
{
//    [Route("api/[controller]")]
//    [ApiController]
   
//    public class ActivationController : ControllerBase
//    {
//        readonly Repository<Activation> _service;
//        public ActivationController(IStoreDatabaseSettings settings)
//        {
//            _service = new Repository<Activation>(settings, "Activations");
//        }

//        [HttpGet]
//        public ActionResult<List<Activation>> Get() =>
//            _service.Get();



//        [HttpPost]
//        public ActionResult<Activation> Create(Activation record)
//        {
//            _service.InsertRecord(record);
//            return CreatedAtRoute("GetActivation", new { id = record.Id?.ToString() }, record);
//        }

//        [HttpGet("{id:length(24)}", Name = "GetActivation")]
//        public ActionResult<Activation> Get(string id)
//        {
//            var record = _service.Get(id);

//            if (record is null)
//            {
//                return NotFound();
//            }
//            return record;
//        }

//        [HttpPut("{id:length(24)}")]
//        public IActionResult Update(string id, Activation record)
//        {
//            var book = _service.Get(id);

//            if (book is null)
//            {
//                return NotFound();
//            }

//            _service.Update(id, record);

//            return NoContent();
//        }

//        [HttpDelete("{id:length(24)}")]
//        public IActionResult Delete(string id)
//        {
//            var record = _service.Get(id);

//            if (record is null)
//            {
//                return NotFound();
//            }
//            _service.Remove(id);

//            return NoContent();
//        }
//    }
}
