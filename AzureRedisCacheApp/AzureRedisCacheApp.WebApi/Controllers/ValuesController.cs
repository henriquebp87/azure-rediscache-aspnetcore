using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace AzureRedisCacheApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private IDistributedCache _cache;

        public ValuesController(IDistributedCache cache)
        {
            _cache = cache;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            //Get or Set Cache Value
            string value = _cache.GetString("CacheTime");

            if (value == null)
            {
                value = DateTime.Now.ToString();

                var options = new DistributedCacheEntryOptions();
                options.SetSlidingExpiration(TimeSpan.FromMinutes(1));
                _cache.SetString("CacheTime", value, options);
            }

            return new[] { $"Cache Time: {value}", $"Current Time: {DateTime.Now.ToString()}" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            //Get Session Value
            return Json($"Session Time: {HttpContext.Session.GetString("SessionTime")}");
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            //Set Session Value
            HttpContext.Session.SetString("SessionTime", DateTime.Now.ToString());
            return Ok(true);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
