using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAn;
using DoAn.Data;
using Microsoft.AspNetCore.Authorization;

namespace DoAn.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DoAnContext _context;

        public UsersController(DoAnContext context)
        {
            _context = context;
        }
        

        [Authorize]
        //tạo Authorize dùng để đăng nhập https://www.youtube.com/watch?v=6X6iONXhz2w&list=PL4WEkbdagHIQVbiTwos0E38VghMJA06OT&index=7
        //Encode to Base64 format email:pass VD:Basic ZGFuaEBnbWFpbC5jb206MTIzNDU2Nzg=
        //cách tạo json token https://www.youtube.com/watch?v=rn2gp5VNGKI&list=PL4WEkbdagHIQVbiTwos0E38VghMJA06OT&index=8
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


        // GET: api/Users/5
        [HttpGet("ShortUser/{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.Select(i => new { i.UserId, i.FullName, i.Phone, i.Email, i.AvatarUrl })
                .Where(i => i.UserId == id)
                .ToListAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
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

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
