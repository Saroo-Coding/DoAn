using DoAn.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace DoAn.Handlers
{
    public class BasicAuthentication : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly DoAnContext _context;

        public BasicAuthentication(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, DoAnContext context) : base(options, logger, encoder, clock)
        {
            _context = context;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Authorization header was not found");
            try
            {
                var autherizationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var bytes = Convert.FromBase64String(autherizationHeaderValue.Parameter);
                string[] credentials = Encoding.UTF8.GetString(bytes).Split(":");
                string email = credentials[0];
                string password = credentials[1];

                User user = _context.Users.Where(u => u.Email == email && u.Pass == password).FirstOrDefault();

                if (user == null)
                {
                    AuthenticateResult.Fail("Sai tài khoản hoặc mật khẩu !!!");
                }
                else
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, user.Email) };
                    var indentity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(indentity);
                    var ticket = new AuthenticationTicket(principal,Scheme.Name);

                    return AuthenticateResult.Success(ticket);
                }
            }
            catch (Exception)
            {
                return AuthenticateResult.Fail("Đã có lỗi xảy ra !!!");
            }
            
            return AuthenticateResult.Fail("Hãy đăng nhập lại !!!");
        }
    }
}
