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
            var kq = _context.Users.Join(_context.UsersRelas,u => u.UserId,r => r.UserId,(u,r) => new { r.UserId,  u.FullName }).ToList();
            return Ok(_context.Users.Select(i => new {i.UserId,i.FullName}).ToList());
        }


        [HttpPost]
        public IActionResult Post(User user) 
        {
            var newuser = new User
            {
                //UserId = Guid.NewGuid(),
                UserId = user.UserId,
                FullName = user.FullName,
                Phone= user.Phone,
                Email= user.Email,
                AvatarUrl= user.AvatarUrl,
            };
            _context.Users.Add(newuser);
            _context.SaveChanges();

            return Ok( new {Data = newuser});
        }

    }
}
