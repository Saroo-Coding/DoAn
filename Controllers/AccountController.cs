﻿using DoAn.Data;
using DoAn.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DoAn.Controllers
{
    [Authorize]
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
            var user = await _context.Users
                .Where(i => i.UserId == id)
                .Select(i => new { i.UserId, i.FullName, i.Phone, i.Email,
                    i.UsersInfo!.Avatar, i.UsersInfo!.Sex, i.UsersInfo.StudyAt, i.UsersInfo.WorkingAt, i.UsersInfo.Favorites
                    , i.UsersInfo.OtherInfo, i.UsersInfo.DateOfBirth}).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("MyPost/{id}")]
        public async Task<ActionResult> GetMyPost(string id)
        {
            var user = await _context.Posts.Where(i => i.UserId == id)
                .Select(i => new { i.PostId, i.Content, i.Image1, i.Image2, i.Image3, i.AccessModifier, i.DatePost })
                .ToListAsync();

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
                .Select(i => new { i.User.UserId, i.User.FullName,i.User.UsersInfo!.Avatar})
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
                .Select(i => new { i.User.UserId, i.User.FullName, i.User.UsersInfo!.Avatar, i.Content})
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
                .Select(i => new { i.AddFriendNavigation.UserId, i.AddFriendNavigation.FullName, i.AddFriendNavigation.UsersInfo!.Avatar })
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

        //[HttpGet("FollowMe/{id}")]
    
        //Post
    }
}
