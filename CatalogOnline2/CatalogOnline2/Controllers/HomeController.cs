using CatalogOnline2.Data;
using CatalogOnline2.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;

namespace CatalogOnline2.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly CatalogOnline2Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;



        public HomeController(ILogger<HomeController> logger, CatalogOnline2Context context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {

            if (_context.User != null)
            {
                var userList = GetUserList().Result;
                var user = userList.FirstOrDefault(u => u.username == "admin");
            }
            else
            {
                Problem("Entity set 'CatalogOnline2Context.User'  is null.");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(User credentials)
        {
            if (credentials.username == "admin" && credentials.password == "admin")
            {
                HttpContext.Session.SetString("LoggedInUsername", "admin");
                return RedirectToAction("Index", "Users");

            }

            using (Aes aesAlgorithm = Aes.Create())
            {
                var userList = GetUserList().Result;
                var user = userList.FirstOrDefault();
                string key = "jI5N0dqT6y6LfoP5tY9Fpg==";
                string IV = "EHTXEj7z2pQ5Myzc6i31zQ==";
                aesAlgorithm.Key = Convert.FromBase64String(key);
                aesAlgorithm.IV = Convert.FromBase64String(IV);

                string encr = credentials.password;
                byte[] password = Utils.Utils.EncryptStringToBytes_Aes(encr, aesAlgorithm.Key, aesAlgorithm.IV);
                encr = Convert.ToBase64String(password);

                foreach (var loginCheck in userList)
                {

                    if (credentials.username == loginCheck.username && encr == loginCheck.password)
                    {
                        HttpContext.Session.SetString("LoggedInUsername", loginCheck.username);
                        if (loginCheck.user_type == "user")
                        {
                            return RedirectToAction("Index", "Student");
                        }
                        if (loginCheck.user_type == "teacher")
                        {
                            return RedirectToAction("Index", "Teacher");
                        }


                    }
                }
                ViewBag.Message = String.Format("Username sau parola gresite");
                return View("Index");
            }
        }

        private async Task<List<User>> GetUserList()
        {
            return await _context.User.ToListAsync();

        }

        public IActionResult UsersShortcut()
        {
            return RedirectToAction("Index", "Users");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}