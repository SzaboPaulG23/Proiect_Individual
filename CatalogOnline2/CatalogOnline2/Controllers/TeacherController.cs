using CatalogOnline2.Data;
using CatalogOnline2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using CatalogOnline2.Data;
using CatalogOnline2.Models;
using CatalogOnline2.ViewModels;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using MimeKit;


namespace CatalogOnline2.Controllers
{
    public class TeacherController : Controller
    {

        private readonly CatalogOnline2Context _context;

        public TeacherController(CatalogOnline2Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var loggedInUsername = HttpContext.Session.GetString("LoggedInUsername");
            ViewData["LoggedInUsername"] = loggedInUsername;
            return View();
        }

        public IActionResult ChangePassword()
        {
            var loggedInUsername = HttpContext.Session.GetString("LoggedInUsername");
            ViewData["LoggedInUsername"] = loggedInUsername;

            var user = _context.User.SingleOrDefault(u => u.username == loggedInUsername);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var loggedInUsername = HttpContext.Session.GetString("LoggedInUsername");
            ViewData["LoggedInUsername"] = loggedInUsername;
            if (ModelState.IsValid)
            {
                var user = _context.User.SingleOrDefault(u => u.username == loggedInUsername);



                using (Aes aesAlgorithm = Aes.Create())
                {
                    string emailText = null;
                    string password = model.NewPassword;
                    string key = "jI5N0dqT6y6LfoP5tY9Fpg==";
                    string IV = "EHTXEj7z2pQ5Myzc6i31zQ==";
                    aesAlgorithm.Key = Convert.FromBase64String(key);
                    aesAlgorithm.IV = Convert.FromBase64String(IV);
                    byte[] encrypted = null;
                    emailText = $"{emailText} <p>You have succesfully changed your password!<br></br>New password is: {password}</p><br></br>";
                    Utils.Utils.testMail(emailText, user.email);
                    encrypted = Utils.Utils.EncryptStringToBytes_Aes(password, aesAlgorithm.Key, aesAlgorithm.IV);
                    password = Convert.ToBase64String(encrypted);

                    user.password = password;
                    await _context.SaveChangesAsync();

                    return RedirectToAction("index");
                }
            }

            return View(model);
        }
    }
}
