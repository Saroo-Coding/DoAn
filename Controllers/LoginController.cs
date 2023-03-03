using DoAn.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DoAn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DoAnContext _context;

        public LoginController(DoAnContext context)
        {
            _context = context;
        }

        // GET      

        // POST
        [HttpPost]
        public async Task<ActionResult> PostUser(User user)
        {
            _context.Users.Add(user);
            try
            {
                user.FullName = _context.Users.First().FullName;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.UserId))
                {
                    return Ok(new { Alert = "Đã tồn tại" });
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        private bool UserExists(int id)
        {
            //return _context.Users.Any(e => e.Email == email || e.Phone == sdt);
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
