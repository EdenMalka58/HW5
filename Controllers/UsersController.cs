using HW5.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HW5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<HW5.Models.User> Get(bool includeInActive)
        {
            return HW5.Models.User.Read(includeInActive);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost("register")]
        public int Register([FromBody] HW5.Models.User user)
        {
            return user.Register(); // 1 = success, 0 = error, -1 = unique email error
        }

        // POST api/<UsersController>
        [HttpPost("login")]
        public LoginUserResponse Login([FromBody] LoginUserRequest loginUser)
        {
            LoginUserResponse response = new LoginUserResponse();
            response.User = HW5.Models.User.Login(loginUser.Email, loginUser.Password);
            if (response.User == null)
                response.Error = 1; // 1 = user is not registered, 0 = login ok
            else response.Error = 0;
            return response;
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public int Put(int id, [FromBody] HW5.Models.User user)
        {
            return user.Update(id) ? 1 : 0; // 1 = success, 0 = error
        }

        // PUT api/<UsersController>/5
        [HttpPut("active/{id}")]
        public int SetActive(int id, [FromBody] ActiveUserRequest request)
        {
            return HW5.Models.User.UpdateActive(id, request.Active) ? 1 : 0; // 1 = success, 0 = error
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public int Delete(int id)
        {
            return HW5.Models.User.Remove(id) ? 1 : 0; // 1 = success, 0 = error
        }
    }
}
