using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RRshop.Data;
using RRshop.Models;
using RRshop.ViewModels;
using System.Security.Claims;
using System.Text;
using RRshop.Services;

namespace RRshop.Controllers
{
    public class AccountController : Controller
    {
        private readonly rrshopContext _context;
        private readonly IMapper _mapper;

        public AccountController(rrshopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: AccountController
        [Authorize]
        public async Task<IActionResult> Index()
        {
            int _id = int.Parse(s: HttpContext.User.FindFirst("id").Value);
            User model = await _context.Users.FirstOrDefaultAsync(m => m.Id == _id);

            if (model == null)
            {
                return NotFound();
            }

            if (model.Role == Roles.Root)
            {
                var tgbot = HttpContext.RequestServices.GetService<INotifyService>();
                tgbot.SecretKey = GetRandomString();
                ViewBag.bot_key = tgbot.SecretKey;
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register() => View();



        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            User newUser = _mapper.Map<User>(viewModel);
            newUser.Role = Roles.SUser;

            try
            {
                _context.Add(newUser);
                await _context.SaveChangesAsync();

                User? userDb = await _context.Users.FirstOrDefaultAsync(usr => usr.Phone == newUser.Phone);
                var principal = GetClaimsPrincipalDefault(userDb);
                await HttpContext.SignInAsync(principal);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Ошибка регистрации ", e.Message);
                return View(viewModel);
            }

        }


        public IActionResult Login(string returnUrl) => View();


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);
            User logUser = _mapper.Map<User>(viewModel);

            try
            {
                User? userDB = await _context.Users.FirstOrDefaultAsync(db => db.Phone == logUser.Phone);
                if (userDB is null)
                {
                    ModelState.AddModelError("", "Пользователь не найден");
                    return View(viewModel);
                }

                if (userDB.Password == logUser.Password)
                {

                    var principal = GetClaimsPrincipalDefault(userDB);
                    await HttpContext.SignInAsync(principal);

                    return Redirect("/Account");
                }

                throw new Exception("Неверный пароль");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(viewModel);
            }
        }


        private ClaimsPrincipal GetClaimsPrincipalDefault(User newUser)
        {
            var claims = new List<Claim>
            {
                new Claim("id", newUser.Id.ToString(), ClaimValueTypes.Integer),
                new Claim(ClaimTypes.MobilePhone, newUser.Phone),
                new Claim(ClaimTypes.Role, newUser.Role)
            };

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimPrincipal = new ClaimsPrincipal(claimIdentity);

            return claimPrincipal;
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        
        private string GetRandomString()
        {
            var code = new char[4];
            Random r = new Random();
            var result = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                result.Append((char) r.NextInt64(97, 122));
            }
            
            return result.ToString();
        }
    }
}
