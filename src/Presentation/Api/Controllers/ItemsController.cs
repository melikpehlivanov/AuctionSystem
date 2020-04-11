namespace Api.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Application.Items.Queries.GetItemDetail;
    using Application.Models.Item;
    using Microsoft.AspNetCore.Mvc;

    public class ItemsController : BaseController
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            var result = await this.Mediator.Send(new GetItemDetailQuery { Id = id });
            return Ok(result);
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
