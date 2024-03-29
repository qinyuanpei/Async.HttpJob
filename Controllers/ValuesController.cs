﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncHttpJob.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AsyncHttpJob.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return JsonConvert.SerializeObject(new HttpJobDescriptor()
            {
                Corn = "134",
                JobName = "HAHAH",
                JobParameter = new Dictionary<string, string>{{"Name","SB"}},
                HttpUrl = "LOCAL",
                HttpMethod = "post",
            });
            //return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
