using DoAn.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Controllers
{
    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DoAnContext _context;

        public AccountController(DoAnContext context)
        {
            _context = context;
        }

        //GET 
        [HttpGet("IsMe/{id}")]
        public async Task<ActionResult> Get(string id)
        {
            var allGroup = _context.Groups.Select(g => new { g.GroupId, g.NameGroup, g.Avatar }).ToList();
            var groups = _context.GroupMembers.Where(m => m.UserId == id).Select(m => new { m.GroupId, m.Group!.NameGroup, m.Group.Avatar }).ToList();
            var notInGroups = allGroup.Except(groups).ToList();
            var user = await _context.Users
                .Where(i => i.UserId == id)
                .Select(i => new { i.UserId, i.FullName, i.Phone, i.Email,
                    i.UsersInfo!.Avatar,i.UsersInfo!.AnhBia ,i.Sex, i.UsersInfo.StudyAt, i.UsersInfo.WorkingAt, i.UsersInfo.Favorites
                    , i.UsersInfo.OtherInfo, i.BirthDay,
                    friend = _context.Friends.Where(f => f.UserId == i.UserId).Count(),
                    groups,
                    notInGroups,
                }).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("MyPost/{id}")]
        public async Task<ActionResult> GetMyPost(string id)
        {
            var user = await _context.Posts.OrderByDescending(i => i.DatePost).Where(i => i.UserId == id)
                .Select(i => new {
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
                    like = _context.Likes.Where(l => l.PostId == i.PostId).Select(i => new {i.UserId}).ToList(),
                    share = _context.Shares.Where(s => s.PostId == i.PostId).Count(),
                }).ToListAsync();

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet("MyPostInGroup/{id}")]
        public async Task<ActionResult> GetMyPostInGroup(string id)
        {
            var user = await _context.GroupPosts.Where(i => i.UserId == id)
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
                    comment = _context.CmtGroupPosts.Select(i => new { i.CmtId, i.PostId, i.User!.FullName, i.User.UsersInfo!.Avatar, i.Content }).Where(c => c.PostId == i.PostId).ToList(),
                    like = _context.LikeGroupPosts.Where(l => l.PostId == i.PostId).Count(),
                    liked = (_context.LikeGroupPosts.Where(l => l.PostId == i.PostId && l.UserId == id)).Any(),
                    cmt = _context.CmtGroupPosts.Where(c => c.PostId == i.PostId).Count(),
                    share = _context.SharePostGroups.Where(s => s.PostId == i.PostId).Count(),
                }).ToListAsync();

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        
        [HttpGet("LikeMyPost/{id}")]
        public async Task<ActionResult> GetLikePost(int id) 
        {
            var user = await _context.Likes.Where(i => i.PostId == id)
                .Select(i => new { i.User!.UserId, i.User.FullName,i.User.UsersInfo!.Avatar})
                .ToListAsync();

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("CommentMyPost/{id}")]
        public async Task<ActionResult> GetComment(int id)
        {
            var user = await _context.Comments.Where(i => i.PostId == id)
                .Select(i => new { i.User!.UserId, i.User.FullName, i.User.UsersInfo!.Avatar, i.Content})
                .ToListAsync();

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("MyFriend/{id}")]
        public async Task<ActionResult> GetFriend(string id)
        {
            var user = await _context.Friends.Where(i => i.UserId == id)
                .Select(i => new {i.FriendId, i.AddFriendNavigation!.UserId, i.AddFriendNavigation.FullName, i.AddFriendNavigation.UsersInfo!.Avatar })
                .ToListAsync();
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        
        [HttpGet("MyFollow/{id}")]
        public async Task<ActionResult> GetFollow(string id)
        {
            var user = await _context.UsersRelas.Where(i => i.UserId == id)
                .Select(i => new { i.FollwingNavigation.UserId, i.FollwingNavigation.FullName, i.FollwingNavigation.UsersInfo!.Avatar })
                .ToListAsync();

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        //Post
        [HttpPut("EditImage/{id}")]
        public async Task<IActionResult> EditImage(string id, UsersInfo usersInfo)
        {
            if (id != usersInfo.UserId)
            {
                return BadRequest();
            }
            
            UsersInfo infor = await _context.UsersInfos.SingleAsync(i => i.UserId == id);

            if (usersInfo.Avatar != null && infor.Avatar != usersInfo.Avatar)
            {
                infor.Avatar = usersInfo.Avatar;
            }
            if (usersInfo.AnhBia != null && infor.AnhBia != usersInfo.AnhBia)
            {
                infor.AnhBia = usersInfo.AnhBia;
            }
            if (usersInfo.OtherInfo != null && infor.OtherInfo != usersInfo.OtherInfo)
            {
                infor.OtherInfo = usersInfo.OtherInfo;
            }
            
            _context.Entry(infor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch {throw;}

            return NoContent();
        }
    }
}
