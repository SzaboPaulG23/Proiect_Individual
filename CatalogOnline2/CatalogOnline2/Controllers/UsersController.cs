using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CatalogOnline2.Data;
using CatalogOnline2.Models;
using Microsoft.CodeAnalysis.Text;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Security.Cryptography;
using MimeKit;

namespace CatalogOnline2.Controllers
{
    public class UsersController : Controller
    {
        private readonly CatalogOnline2Context _context;

        public UsersController(CatalogOnline2Context context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {


            return _context.User != null ?
                        View(await _context.User.ToListAsync()) :
                        Problem("Entity set 'CatalogOnline2Context.User'  is null.");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        public IActionResult Generate()
        {
            return View();
        }


        public async Task<IActionResult> GenerateAccountExcel()
        {
            User generated = new User();
            string password = null;
            string[,] newUser = null;
            string emailText = null;
            newUser = Utils.Utils.readExcel();
            int i = 1;
            while (!string.IsNullOrEmpty(newUser[i,1]))
            {
                    string newUsername = $"{newUser[i,0].ToLower()}.{newUser[i,1].ToLower()}{newUser[i,2]}";
                    string encrypted = Utils.Utils.passwordGen(ref password);
                    generated.ID = 0;
                    generated.username = newUsername;
                    generated.password = encrypted;
                    generated.email = $"{newUser[i, 3]}";
                    generated.user_type = $"{newUser[i, 4]}";
                    emailText = $"{emailText} <p>Welcome to CatalogOnline! Your {newUser[i,4]} account details are below:<br></br>Your username is: {newUsername}<br></br>Your password is: {password} </p><br></br>";
                    if (ModelState.IsValid)
                    {
                        _context.User.Add(generated);
                        await _context.SaveChangesAsync().ConfigureAwait(true);
                    }
                    Utils.Utils.testMail(emailText, generated.email);
                    emailText = null;
                i++;
            }
            return RedirectToAction(nameof(Index));
        }


        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,username,password,email,user_type")] User user)
        {
            if (id != user.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'CatalogOnline2Context.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return (_context.User?.Any(e => e.ID == id)).GetValueOrDefault();
        }
        [HttpPost]
        public async Task<IActionResult> FileUpload(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = @"C:\Users\Paul\Desktop\Upload\TEST.xlsx";


                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }



                    string text = System.IO.File.ReadAllText(filePath);
                }
            }
 

            return RedirectToAction(nameof(Generate));
        }



    }

}
