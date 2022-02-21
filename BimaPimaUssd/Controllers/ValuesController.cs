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
    public class PbiController : ControllerBase
    {
        readonly Repository<PBI> _service;
        public PbiController(IStoreDatabaseSettings settings)
        {
            _service = new Repository<PBI>(settings, "PBIData");
        }

        [HttpGet]
        public ActionResult<List<PBI>> Get() =>
            _service.Get();


        [HttpPost("populate")]
        public ActionResult<PBI> Post(List<PBI> records)
        {
            _service.InsertMany(records); return Ok();
        }


        [HttpPost]
        public ActionResult<PBI> Create(PBI record)
        {
            _service.InsertRecord(record);
            return CreatedAtRoute("GetPBI", new { id = record.Id?.ToString() }, record);
        }

        [HttpGet("{id:length(24)}", Name = "GetPBI")]
        public ActionResult<PBI> Get(string id)
        {
            var record = _service.Get(id);

            if (record is null)
            {
                return NotFound();
            }
            return record;
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, PBI record)
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
        //[HttpDelete("all")]
        //public IActionResult DeleteAll()
        //{
        //    var record = _service.de();

        //    return Ok();
        //}
    }
}
