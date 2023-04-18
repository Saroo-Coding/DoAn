using DoAn.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Controllers
{
    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly DoAnContext _context;

        public GroupsController(DoAnContext context)
        {
            _context = context;
        }

        //GET
        [HttpGet("Groups")]
        public async Task<ActionResult> Groups()
        {
            var group = await _context.Groups
                .Select(i => new {
                    i.GroupId,
                    i.StatusGroup,
                    i.NameGroup,
                    i.Avatar,
                    i.CoverImage,
                    i.Intro,
                }).ToListAsync();

            if (group == null)
            {
                return NotFound();
            }
            return Ok(group);
        }
        [HttpGet("Groups/{id}")]
        public async Task<ActionResult> Groups(int id)
        {
            var group = await _context.Groups.Where(i => i.GroupId == id)
                .Select(i => new {
                    i.GroupId,
                    i.StatusGroup,
                    i.NameGroup,
                    i.Avatar,
                    i.CoverImage,
                    i.Intro,
                    Member = _context.GroupMembers.Where(m => m.GroupId == i.GroupId).Select(m => new { m.UserId }).ToList(),
                }).FirstOrDefaultAsync();

            if (group == null)
            {
                return NotFound();
            }
            return Ok(group);
        }
        [HttpGet("GroupPosts/{id}")]
        public async Task<ActionResult> GroupPosts(int id)
        {
            try
            {
                var post = await _context.GroupPosts
                .Select(i => new {
                    i.UserId,
                    i.User!.FullName,
                    i.User.UsersInfo!.Avatar,
                    i.PostId,
                    i.Content,
                    i.Img1,
                    i.Img2,
                    i.Img3,
                    datepost = i.DatePost.ToString("dd-MM-yyyy"),
                    /*comment = _context.Comments.Select(i => new { i.CmId, i.PostId, i.User!.FullName, i.User.UsersInfo!.Avatar, i.Content }).Where(c => c.PostId == i.PostId).ToList(),
                    like = _context.Likes.Where(l => l.PostId == i.PostId).Count(),
                    liked = (_context.Likes.Where(l => l.PostId == i.PostId && l.UserId == id)).Any(),
                    cmt = _context.Comments.Where(c => c.PostId == i.PostId).Count(),
                    share = _context.Shares.Where(s => s.PostId == i.PostId).Count(),*/
                }).ToListAsync();
                return Ok(post);
            }
            catch
            {
                throw;
            }
        }
    }
}
