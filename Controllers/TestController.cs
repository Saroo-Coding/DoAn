using DoAn.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DoAn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly DoAnContext _context;

        public TestController(DoAnContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var kq = _context.Users.Join(_context.UsersRelas,u => u.Id,r => r.Id,(u,r) => new { r.Follower,  u.Id }).ToList();
            return Ok(/*_context.Users.Select(i => new {i.Id,i.FullName}).ToList()*/kq);
        }

        [HttpPost]
        public IActionResult Post(User user) 
        {
            _context.Users.Add(user);
            return Ok();
        }
    }
}
