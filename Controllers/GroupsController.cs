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
                    i.NameGroup,
                    i.Avatar
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
                    Admin = _context.GroupMembers.Where(m => m.GroupId == i.GroupId && m.Position == "admin").Select(m => new { m.UserId, m.User!.FullName, m.User.UsersInfo!.Avatar }).ToList(),
                    Mod = _context.GroupMembers.Where(m => m.GroupId == i.GroupId && m.Position == "mod").Select(m => new { m.UserId, m.User!.FullName, m.User.UsersInfo!.Avatar }).ToList(),
                    Member = _context.GroupMembers.Where(m => m.GroupId == i.GroupId ).Select(m => new { m.UserId, m.User!.FullName, m.User.UsersInfo!.Avatar }).ToList(),
                    Join = _context.JoinGroupReqs.Where(m => m.GroupId == i.GroupId ).Select(m => new { m.UserId, m.User!.FullName, m.User.UsersInfo!.Avatar }).ToList(),
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
                var post = await _context.GroupPosts.OrderByDescending(i => i.DatePost)
                .Where( i => i.GroupId == id)
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
                    liked = (_context.LikeGroupPosts.Where(l => l.PostId == i.PostId && l.UserId == i.UserId)).Any(),
                    cmt = _context.CmtGroupPosts.Where(c => c.PostId == i.PostId).Count(),
                    share = _context.SharePostGroups.Where(s => s.PostId == i.PostId).Count(),
                }).ToListAsync();
                return Ok(post);
            }
            catch
            {
                throw;
            }
        }

        //POST
        [HttpPost("NewShare")]
        public async Task<ActionResult> NewShare(SharePostGroup share)
        {
            try
            {
                share.DateShare = DateTime.Now;
                /*var user = await _context.GroupPosts.Where(i => i.PostId == share.PostId).Select(i => new { i.UserId, i.GroupId }).FirstAsync();
                if (user.UserId != share.UserId)
                {
                    PostNotify notify = new PostNotify();
                    notify.FromUser = share.UserId;
                    notify.ToUser = user.UserId;
                    notify.PostId = share.PostId;
                    notify.Content = "đã chia sẻ bài viết của bạn";
                    notify.Status = 0;
                    notify.Date = DateTime.Now;
                    _context.PostNotifies.Add(notify);
                }*/
                _context.SharePostGroups.Add(share);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return Ok();
        }
        [HttpPost("NewGroupPost")]
        public async Task<ActionResult> NewGroupPost(GroupPost post)
        {
            try
            {
                post.DatePost = DateTime.Now;
                _context.GroupPosts.Add(post);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return CreatedAtAction("NewGroupPost", post);
        }
        [HttpPost("NewLike")]
        public async Task<ActionResult> NewLike(LikeGroupPost like)
        {
            try
            {
                /*var user = await _context.GroupPosts.Where(i => i.PostId == like.PostId).Select(i => new { i.UserId, i.GroupId }).FirstAsync();
                if (user.UserId != like.UserId)
                {
                    PostNotify notify = new PostNotify();
                    notify.FromUser = like.UserId;
                    notify.ToUser = user.UserId;
                    notify.PostId = like.PostId;
                    notify.Content = "đã thả tim bài viết của bạn";
                    notify.Status = 0;
                    notify.Date = DateTime.Now;
                    _context.PostNotifies.Add(notify);
                }*/
                _context.LikeGroupPosts.Add(like);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return Ok();
        }
        [HttpPost("NewCmt")]
        public async Task<ActionResult> NewCmt(CmtGroupPost cmt)
        {
            try
            {
               /* var user = await _context.GroupPosts.Where(i => i.PostId == cmt.PostId).Select(i => new { i.UserId, i.GroupId }).FirstAsync();
                if (user.UserId != cmt.UserId)
                {
                    PostNotify notify = new PostNotify();
                    notify.FromUser = cmt.UserId;
                    notify.ToUser = user.UserId;
                    notify.PostId = cmt.PostId;
                    notify.Content = "đã bình luận: " + cmt.Content;
                    notify.Status = 0;
                    notify.Date = DateTime.Now;
                    _context.PostNotifies.Add(notify);
                }*/
                _context.CmtGroupPosts.Add(cmt);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return CreatedAtAction("NewCmt", cmt);
        }
        [HttpPost("JoinReq")]
        public async Task<ActionResult> JoinReq(JoinGroupReq req)
        {
            try
            {
                _context.JoinGroupReqs.Add(req);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return CreatedAtAction("JoinReq", req);
        }
        [HttpPost("NewMember/{id}")]
        public async Task<ActionResult> NewMember(int id)
        {
            try
            {
                var req = _context.JoinGroupReqs.Where(m => m.ReqId == id).FirstOrDefault();
                GroupMember mb = new GroupMember();
                mb.GroupId = req!.GroupId;
                mb.UserId = req.UserId;
                mb.Position = "member";
                _context.GroupMembers.Add(mb);
                _context.JoinGroupReqs.Remove(req);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return Ok();
        }

        //DELETE
        [HttpDelete("DeleteShare/{id}")]
        public async Task<ActionResult> DeleteShare(int id)
        {
            try
            {
                var req = await _context.SharePostGroups.FindAsync(id);
                if (req == null)
                {
                    return NotFound();
                }
                _context.SharePostGroups.Remove(req);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                throw;
            }
        }
        [HttpDelete("LeaveGroup")]
        public async Task<ActionResult> LeaveGroup(GroupMember mb)
        {
            try
            {
                var post = await _context.GroupMembers.Where(m => m.GroupId == mb.GroupId && m.UserId == mb.UserId).FirstAsync();
                if (post == null)
                {
                    return NotFound();
                }

                _context.GroupMembers.Remove(post);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                throw;
            }
        }
        [HttpDelete("DeleteJoinReq")]
        public async Task<ActionResult> DeleteJoinReq(JoinGroupReq req)
        {
            try
            {
                var post = await _context.JoinGroupReqs.Where(m => m.GroupId == req.GroupId && m.UserId == req.UserId).FirstAsync();
                if (post == null)
                {
                    return NotFound();
                }
                _context.JoinGroupReqs.Remove(post);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                throw;
            }
        }
        [HttpDelete("DeletePost/{id}")]
        public async Task<ActionResult> DeletePost(int id)
        {
            try
            {
                if (_context.GroupPosts == null)
                {
                    return NotFound();
                }
                var post = await _context.GroupPosts.FindAsync(id);
                if (post == null)
                {
                    return NotFound();
                }

                _context.GroupPosts.Remove(post);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                throw;
            }
        }
        [HttpDelete("UnHeart")]
        public async Task<ActionResult> UnHeart(LikeGroupPost like)
        {
            try
            {
                var req = await _context.LikeGroupPosts.Where(i => i.PostId == like.PostId && i.UserId == like.UserId).FirstAsync();
                if (req == null)
                {
                    return NotFound();
                }
                _context.LikeGroupPosts.Remove(req);
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
