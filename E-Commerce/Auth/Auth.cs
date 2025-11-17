using E_Commerce.Services;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace E_Commerce.Auth
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly TokenService _tokenService;
        public ValuesController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public string GetToken()
        {
            var token = _tokenService.GenerateJwtToken("Ahmed");

            return token;
        }
        [HttpGet]
        public string GetAuthenticated(string token)
        {
            var validate = _tokenService.ValidateAndGetUsername(token);

            return validate;
        }
        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
