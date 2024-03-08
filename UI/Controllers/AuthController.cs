using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UI.Models;
using UI.Services;

namespace UI.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthorizationService _authService;

        IConfiguration _configuration;
        public AuthController(AuthorizationService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel request)
        {

            if (request.Username != "1")
            {

                ViewBag.Message = "Kullanıcı adı veya şifre yanlış";
                return View(request);

            }

            if (request.Password != "1")
            {
                ViewBag.Message = "Kullanıcı adı veya şifre yanlış";
                return View(request);


            }
            var key = _configuration["TokenOptions:SecurityKey"];

            var token = CreateToken(key);
            HttpContext.Session.SetString("Authorization", $"Bearer {token!}");

            _authService.UpdateAccessToken(token);

            return RedirectToAction("Index", "Home");
        }

        private static string CreateToken(string key)
        {
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var securityKey = new SymmetricSecurityKey(keyBytes);

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            List<Claim> claims = new();

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7083",
                audience: "https://localhost:7083",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

    }
}
