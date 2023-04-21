using DoAn.Data;
using DoAn.Models;
using Microsoft.AspNetCore.Authorization;
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
    [Route("[controller]")]
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
            refreshToken.ExpiryDate = DateTime.Now;

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
        private static string MD5Hash(string text)
        {
            MD5 md5 = MD5.Create();
            /*Chuyen string ve byte*/
            byte[] inputBytes = Encoding.ASCII.GetBytes(text);
            /*ma hoa byte*/
            byte[] result = md5.ComputeHash(inputBytes);
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }
        private bool UserExists(string email, string phone)
        {
            return _context.Users.Any(e => e.Email == email || e.Phone == phone);
        }

        //GET
        [HttpGet]
        public async Task<ActionResult> GetLogin(string? email, string? sdt, string pass)
        {
            UserWithToken? userWithToken = null;
            //sdt
            if (email == null && sdt != null)
            {
                var user = await _context.Users.Where(x => x.Phone == sdt && x.Pass == MD5Hash(pass)).FirstOrDefaultAsync();
                if (user != null)
                {
                    RefreshToken refreshToken = CreateRefreshToken();
                    user!.RefreshTokens.Add(refreshToken);
                    await _context.SaveChangesAsync();

                    userWithToken = new UserWithToken(user!);
                    userWithToken.RefreshToken = refreshToken.Token;
                }
                else
                {
                    return NotFound("Lỗi đăng nhập");
                }
                //tao token
                userWithToken!.RefreshToken = CreateAccessToken(user.UserId!);
            }
            //email
            else
            {
                var user = await _context.Users.Where(x => x.Email == email && x.Pass == MD5Hash(pass)).FirstOrDefaultAsync();
                if (user != null)
                {
                    RefreshToken refreshToken = CreateRefreshToken();
                    user!.RefreshTokens.Add(refreshToken);
                    await _context.SaveChangesAsync();

                    userWithToken = new UserWithToken(user!);
                    userWithToken.RefreshToken = refreshToken.Token;
                }
                else
                {
                    return NotFound("Lỗi đăng nhập");
                }
                //tao token
                userWithToken!.RefreshToken = CreateAccessToken(user.UserId!);
            }
            return Ok(userWithToken);
        }     

        // POST
        [HttpPost]
        public async Task<ActionResult> PostUser(User user)
        {
            try
            {
                string guid = Guid.NewGuid().ToString();
                Byte[] encode = Encoding.ASCII.GetBytes(guid);
                string uuid = "";
                for (int i = 0; i < encode.Length; i++)
                {
                    uuid += encode[i] + "";
                }

                user.UserId = uuid.Substring(0, 15);
                user.Pass = MD5Hash(user.Pass);
                user.CreatedAt = DateTime.Now;

                UsersInfo usersInfo = new UsersInfo();
                usersInfo.UserId = user.UserId;
                if(user.Sex == "Nam")
                    usersInfo.Avatar = "https://firebasestorage.googleapis.com/v0/b/doan-ad756.appspot.com/o/Sever%2Fuser_male.jpg?alt=media&token=5a082919-c61f-4b93-80ac-06267ca14b9f";
                if(user.Sex == "Nữ")
                    usersInfo.Avatar = "https://firebasestorage.googleapis.com/v0/b/doan-ad756.appspot.com/o/Sever%2Fuser_female.jpg?alt=media&token=0bf68592-c458-4175-9690-b7886e29eb96";
                usersInfo.AnhBia = "https://www.shutterstock.com/image-vector/hello-world-day-calligraphy-modern-260nw-2209558865.jpg";
                usersInfo.IsActive = true;
                usersInfo.StudyAt = "";
                usersInfo.WorkingAt = "";
                usersInfo.Favorites = "";
                usersInfo.OtherInfo = "";
                
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

        /*private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }*/
    }
}
