using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using reCAPTCHA.AspNetCore;
using Starter.Net.Api.AntiSpam;

namespace Starter.Net.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IRecaptchaService _recaptchaService;
        public ValuesController(IRecaptchaService recaptchaService)
        {
            _recaptchaService = recaptchaService;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            HttpContext.Items.TryGetValue("uuid", out var uuid);
            return new[] {"value1", "value2", uuid?.ToString()};
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return $"value: {id}";
        }

        // POST api/values
        [HttpPost]
        [GuardSpam]
        public async Task<string> Post()
        {
            return "{\"success\": true}";
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public ActionResult<string> Put(int id, [FromBody] string value)
        {
            return $"{id}:{value}";
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public ActionResult<string> Delete(int id)
        {
            return $"deleted: {id}";
        }
    }
}
