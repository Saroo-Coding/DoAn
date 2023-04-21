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
        [HttpGet("AllUser/{id}")]
        public async Task<ActionResult> AllUser(string id)
        {
            var user = await _context.Users.Where(i => i.UserId != id)
                .Select(i => new { i.UserId, i.FullName, i.UsersInfo!.Avatar })
                .ToListAsync();
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet("Post/{id}")]
        public async Task<ActionResult> GetPost(string id) 
        {
            try
            {
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
                    liked = (_context.Likes.Where(l => l.PostId == i.PostId && l.UserId == id)).Any(),
                    //cmt = _context.Comments.Where(c => c.PostId == i.PostId).Count(),
                    share = _context.Shares.Where(s => s.PostId == i.PostId).Count(),
                }).ToListAsync();
                return Ok(post);
            }
            catch
            {
                throw;
            }
        }
        //danh sach yeu cau ket ban
        [HttpGet("FriendRequest/{id}")]
        public async Task<ActionResult> FriendRequest(string id)
        {
            try
            {
                return Ok(await _context.FriendRequests
                    .Select(i => new
                    {
                        i.ReqId,
                        i.FromUser,
                        i.ToUser,
                        i.FromUserNavigation!.FullName,
                        i.FromUserNavigation.UsersInfo!.Avatar,
                    }).Where(i => i.ToUser == id)
                    .ToListAsync());
            }
            catch { throw; }
        }
       
        [HttpGet("RequestFriend/{id}")]
        public async Task<ActionResult> RequestFriend(string id)
        {
            try
            {
                return Ok(await _context.FriendRequests
                    .Select(i => new
                    {
                        i.ReqId,
                        i.ToUser,
                        i.FromUser,
                        i.ToUserNavigation!.FullName,
                        i.ToUserNavigation.UsersInfo!.Avatar,
                    }).Where(i => i.FromUser == id)
                    .ToListAsync());
            }
            catch { throw; }
        }
        //check xem da gui yeu cau ket ban chua
        [HttpGet("CheckFriend/{me}/{you}")]
        public async Task<ActionResult> CheckFriend(string me,string you)
        {
            try
            {
                return Ok(await _context.FriendRequests
                    .Where(i => i.ToUser == you && i.FromUser == me)
                    .FirstOrDefaultAsync());
            }
            catch { throw; }
        }
        //check xem da ket ban chua
        [HttpGet("CheckAddFriend/{me}/{you}")]
        public async Task<ActionResult> CheckAddFriend(string me,string you)
        {
            try
            {
                var tao = await _context.Friends
                    .Where(i => i.UserId == me && i.AddFriend == you)
                    .ToArrayAsync();
                return Ok(tao);
            }
            catch { throw; }
        }
        //danh sach ban moi
        [HttpGet("NewFriend/{id}")]
        public async Task<ActionResult> NewFriend(string id)
        {
            try
            {
                var all = await _context.Users.Where(i => i.UserId != id).Select(i => new { i.UserId, i.FullName, i.UsersInfo!.Avatar}).ToListAsync();//all ban
                var friended = await _context.Friends.Where(i => i.AddFriend == id).Select(i => new { i.UserId, i.User.FullName, i.User.UsersInfo!.Avatar }).ToListAsync();//da ket ban
                var sent = await _context.FriendRequests.Where(i => i.FromUser == id).Select(i => new {UserId = i.ToUser, i.ToUserNavigation!.FullName, i.ToUserNavigation.UsersInfo!.Avatar }).ToListAsync();//da gui
                var received = await _context.FriendRequests.Where(i => i.ToUser == id).Select(i => new { UserId = i.FromUser, i.FromUserNavigation!.FullName, i.FromUserNavigation.UsersInfo!.Avatar }).ToListAsync();//da nhan
                var notin = friended!.Union(sent!).Union(received!).ToList();

                var exist = all.Except(notin!).ToList();

                return Ok(exist);
            }
            catch { throw; }
        }


        //POST
        [HttpPost("NewLike")]
        public async Task<ActionResult> NewLike(Like like)
        {
            try
            {
                _context.Likes.Add(like);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return CreatedAtAction("NewLike", like);
        }
        [HttpPost("NewPost")]
        public async Task<ActionResult> PostPost(Post post)
        {
            try
            {
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
        [HttpPost("Add_Friend")]
        public async Task<ActionResult> Add_Friend(FriendRequest friendRequest)
        {
            try{
                _context.FriendRequests.Add(friendRequest);
                await _context.SaveChangesAsync();
                return CreatedAtAction("Add_Friend", friendRequest);
            }
            catch { throw; }
        }
        [HttpPost("Answers_Friend/{reqId}")]
        public async Task<ActionResult> Answers_Friend(int reqId)
        {
            try { 
                var friendRequest = _context.FriendRequests.Where(i=> i.ReqId == reqId).FirstOrDefault();
                
                Friend to_friend = new Friend();
                to_friend.UserId = friendRequest!.FromUser;
                to_friend.AddFriend = friendRequest.ToUser;
                Friend from_friend = new Friend();
                from_friend.UserId = friendRequest.ToUser;
                from_friend.AddFriend = friendRequest.FromUser;

                _context.Friends.Add(to_friend);
                _context.Friends.Add(from_friend);
                _context.FriendRequests.Remove(friendRequest!);
                await _context.SaveChangesAsync();                 
                return Ok();
            }
            catch { throw; }
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
        [HttpDelete("UnHeart")]
        public async Task<ActionResult> UnHeart(Like like)
        {
            try
            {
                var req = await _context.Likes.Where(i => i.PostId == like.PostId && i.UserId == like.UserId).FirstAsync();
                if (req == null)
                {
                    return NotFound();
                }
                _context.Likes.Remove(req);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                throw;
            }
        }
        [HttpDelete("Unfriend/{id}")]
        public async Task<ActionResult> Unfriend(int id)
        {
            try
            {
                var req = await _context.FriendRequests.FindAsync(id);
                if (req == null)
                {
                    return NotFound();
                }
                _context.FriendRequests.Remove(req);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                throw;
            }
        }
        [HttpDelete("XoaBan/{me}/{you}")]
        public async Task<ActionResult> XoaBan(string me, string you)
        {
            try
            {
                var meyou = await _context.Friends.Where(i => i.UserId == me && i.AddFriend == you).FirstAsync();
                var youme = await _context.Friends.Where(i => i.UserId == you && i.AddFriend == me).FirstAsync();
                if (meyou == null && youme == null)
                {
                    return NotFound();
                }
                _context.Friends.Remove(meyou!);
                _context.Friends.Remove(youme);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch { throw; }
        }
    }
}
