using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAn.Data;
using Microsoft.AspNetCore.Authorization;

namespace DoAn.Controllers
{
    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class NewsfeedController : ControllerBase
    {
        private readonly DoAnContext _context;

        public NewsfeedController(DoAnContext context)
        {
            _context = context;
        }
        //GET
        [HttpGet("Post")]
        public async Task<ActionResult> GetPost() 
        {
            try
            {
                var rand = new Random();
                var post = await _context.Posts
                .Select(i => new{   
                    i.UserId,
                    i.User!.FullName,
                    i.User.UsersInfo!.Avatar,
                    i.PostId,
                    i.Content,
                    i.Image1,
                    i.Image2,
                    i.Image3,
                    i.AccessModifier,
                    datepost = i.DatePost.ToString("dd-MM-yyyy"),
                    comment = _context.Comments.Select(i => new { i.CmId, i.PostId, i.User!.FullName, i.User.UsersInfo!.Avatar, i.Content }).Where(c => c.PostId == i.PostId).ToList(),
                    like = _context.Likes.Where(l => l.PostId == i.PostId).Count(),
                    cmt = _context.Comments.Where(c => c.PostId == i.PostId).Count(),
                    share = _context.Shares.Where(s => s.PostId == i.PostId).Count(),
                }).ToListAsync();
                return Ok(post);
            }
            catch
            {
                throw;
            }
        }

        //POST
        [HttpPost("NewPost")]
        public async Task<ActionResult> PostPost(Post post)
        {
            try
            {
                post.Image1 = "Khong";
                post.Image2 = "Khong";
                post.Image3 = "Khong";
                post.DatePost = DateTime.Now;

                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return CreatedAtAction("PostPost",post);
        }
        [HttpPost("NewCmt")]
        public async Task<ActionResult> Cmt(Comment comment)
        {
            try
            {
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return CreatedAtAction("Cmt", comment);
        }

        //DELETE
        [HttpDelete("XoaPost/{id}")]
        public async Task<ActionResult> XoaPost(int id)
        {
            try
            {
                if (_context.Posts == null)
                {
                    return NotFound();
                }
                var post = await _context.Posts.FindAsync(id);
                if (post == null)
                {
                    return NotFound();
                }

                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                throw;
            }
        }
    }
}
