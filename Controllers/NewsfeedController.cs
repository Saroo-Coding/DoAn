using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAn.Data;
using Microsoft.AspNetCore.Authorization;

namespace DoAn.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NewsfeedController : ControllerBase
    {
        private readonly DoAnContext _context;

        public NewsfeedController(DoAnContext context)
        {
            _context = context;
        }

        [HttpGet("GetUser")]
        public async Task<ActionResult<User>> GetUser()
        {
            string email = HttpContext.User.Identity!.Name!;

            var user = await _context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
            user!.Pass = null!;

            if (user == null)
            {
                return NotFound();
            }

            return Ok(new { Alert = user });
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.UserId)
            {
                return Ok(new { Alert = "Không đúng id" });
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(new { Alert = "Sửa thành công" });
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.UserId))
                {
                    return Ok(new {Alert = "Đã tồn tại"});
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return Ok(new { Alert = "Không tồn tại " });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { Alert = "Đã xóa " + id });
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
