using DoAn.Data;
using DoAn.Models;
using MessagePack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DoAn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DoAnContext _context;
        private readonly JWTSettings _jwtsettings;

        public LoginController(DoAnContext context, IOptions<JWTSettings> jwtsettings)
        {
            _context = context;
            _jwtsettings = jwtsettings.Value;
        }

        private RefreshToken CreateRefreshToken()
        {
            RefreshToken refreshToken = new RefreshToken();
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                refreshToken.Token = Convert.ToBase64String(random);
            }
            refreshToken.ExpiryDate = DateTime.UtcNow.AddMonths(6);

            return refreshToken;
        }
        private string CreateAccessToken(string userid)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtsettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, Convert.ToString(userid))
                }),
                Expires = DateTime.UtcNow.AddMonths(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // GET
        [HttpGet]
        public async Task<ActionResult> GetLogin(string email, string pass)
        {
            User user = await _context.Users.Where(x => x.Email == email && x.Pass == pass).FirstOrDefaultAsync();
            
            UserWithToken? userWithToken = null;

            if (user != null)
            {
                RefreshToken refreshToken = CreateRefreshToken();
                user!.RefreshTokens.Add(refreshToken);
                await _context.SaveChangesAsync();

                userWithToken = new UserWithToken(user!);
                userWithToken.RefreshToken = refreshToken.Token;
            }
                
            if (user == null)
            {
                return NotFound("Lỗi đăng nhập");
            }
            //tao token
            userWithToken!.RefreshToken = CreateAccessToken(user.UserId!);

            return Ok(userWithToken);
        }

        // POST
        [HttpPost]
        public async Task<ActionResult> PostUser([FromForm]User user)
        {
            try
            {
                user.UserId = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("/", "_").Replace("+", "-").Substring(0, 15); 
                user.CreatedAt = DateTime.Now;

                UsersInfo usersInfo = new UsersInfo();
                usersInfo.UserId = user.UserId;
                usersInfo.Avatar = "https://cdn1.iconfinder.com/data/icons/user-pictures/100/unknown-512.png";
                usersInfo.Sex = "Không";
                usersInfo.IsActive = true;
                usersInfo.StudyAt = "";
                usersInfo.WorkingAt = "";
                usersInfo.Favorites = "";
                usersInfo.OtherInfo = "";
                usersInfo.DateOfBirth = new DateTime(2001,1,1);

                _context.UsersInfos.Add(usersInfo);
                _context.Users.Add(user);
                if (UserExists(user.Email, user.Phone))
                {
                    return Ok(new { Alert = "Đã tồn tại" });
                }
                else
                {
                    await _context.SaveChangesAsync();
                }
                
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return Ok(user);
        }

        private bool UserExists(string email, string phone)
        {
            return _context.Users.Any(e => e.Email == email || e.Phone == phone);
        }
    }
}
