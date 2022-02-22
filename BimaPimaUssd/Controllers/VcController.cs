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
    public class VcController : ControllerBase
    {
        readonly Repository<VC> _service;
        public VcController(IStoreDatabaseSettings settings)
        {
            _service = new Repository<VC>(settings, "VCData700");
        }

        [HttpGet]
        public ActionResult<List<VC>> Get() =>
            _service.Get();



        [HttpPost]
        public ActionResult<VC> Create(VC record)
        {
            _service.InsertRecord(record);
            return CreatedAtRoute("GetVC", new { id = record.Id?.ToString() }, record);
        }

        [HttpGet("{id:length(24)}", Name = "GetVC")]
        public ActionResult<VC> Get(string id)
        {
            var record = _service.Get(id);

            if (record is null)
            {
                return NotFound();
            }
            return record;
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, VC record)
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
