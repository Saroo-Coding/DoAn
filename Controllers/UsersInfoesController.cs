using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAn;
using DoAn.Data;

namespace DoAn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersInfoesController : ControllerBase
    {
        private readonly DoAnContext _context;

        public UsersInfoesController(DoAnContext context)
        {
            _context = context;
        }

        // GET: api/UsersInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsersInfo>>> GetUsersInfos()
        {
            return await _context.UsersInfos.ToListAsync();
        }

        // GET: api/UsersInfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsersInfo>> GetUsersInfo(int id)
        {
            var usersInfo = await _context.UsersInfos.FindAsync(id);

            if (usersInfo == null)
            {
                return NotFound();
            }

            return usersInfo;
        }

        // PUT: api/UsersInfoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsersInfo(int id, UsersInfo usersInfo)
        {
            if (id != usersInfo.UserId)
            {
                return BadRequest();
            }

            _context.Entry(usersInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersInfoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UsersInfoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UsersInfo>> PostUsersInfo(UsersInfo usersInfo)
        {
            _context.UsersInfos.Add(usersInfo);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UsersInfoExists(usersInfo.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUsersInfo", new { id = usersInfo.UserId }, usersInfo);
        }

        // DELETE: api/UsersInfoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsersInfo(int id)
        {
            var usersInfo = await _context.UsersInfos.FindAsync(id);
            if (usersInfo == null)
            {
                return NotFound();
            }

            _context.UsersInfos.Remove(usersInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsersInfoExists(int id)
        {
            return _context.UsersInfos.Any(e => e.UserId == id);
        }
    }
}
