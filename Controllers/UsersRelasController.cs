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
    public class UsersRelasController : ControllerBase
    {
        private readonly DoAnContext _context;

        public UsersRelasController(DoAnContext context)
        {
            _context = context;
        }

        // GET: api/UsersRelas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsersRela>>> GetUsersRelas()
        {
            return await _context.UsersRelas.ToListAsync();
        }

        // GET: api/UsersRelas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsersRela>> GetUsersRela(int id)
        {
            var usersRela = await _context.UsersRelas.FindAsync(id);

            if (usersRela == null)
            {
                return NotFound();
            }

            return usersRela;
        }

        // PUT: api/UsersRelas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsersRela(int id, UsersRela usersRela)
        {
            if (id != usersRela.Id)
            {
                return BadRequest();
            }

            _context.Entry(usersRela).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersRelaExists(id))
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

        // POST: api/UsersRelas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UsersRela>> PostUsersRela(UsersRela usersRela)
        {
            _context.UsersRelas.Add(usersRela);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UsersRelaExists(usersRela.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUsersRela", new { id = usersRela.Id }, usersRela);
        }

        // DELETE: api/UsersRelas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsersRela(int id)
        {
            var usersRela = await _context.UsersRelas.FindAsync(id);
            if (usersRela == null)
            {
                return NotFound();
            }

            _context.UsersRelas.Remove(usersRela);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsersRelaExists(int id)
        {
            return _context.UsersRelas.Any(e => e.Id == id);
        }
    }
}
