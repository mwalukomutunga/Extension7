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
    public class FarmerController : ControllerBase
    {
        readonly Repository<User> _service;
        public FarmerController(IStoreDatabaseSettings settings)
        {
            _service = new Repository<User>(settings, "Farmer");
        }

        [HttpGet]
        public ActionResult<List<User>> Get() =>
            _service.Get();



        [HttpPost]
        public ActionResult<User> Create(User record)
        {
            _service.InsertRecord(record);
            return CreatedAtRoute("GetUser", new { id = record.Id?.ToString() }, record);
        }

        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public ActionResult<User> Get(string id)
        {
            var record = _service.Get(id);

            if (record is null)
            {
                return NotFound();
            }
            return record;
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, User record)
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
