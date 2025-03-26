using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyProject.Filters;
using MyProject.Models;
using NuGet.Protocol;

namespace MyProject.Controllers
{
    public class AdminLoginController : Controller
    {
        private readonly MyProjectContext _context;

        public AdminLoginController(MyProjectContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Administrator admin)
        {
            var result = await _context.Administrator.Where(a => a.AdminID == admin.AdminID && a.AdminPassword == admin.AdminPassword).FirstOrDefaultAsync();

            if (result != null)
            {
                HttpContext.Session.SetString("Manager", result.ToJson());

                return RedirectToAction("Index", "Home");
            }
            else  //如果帳密不正確,回到登入頁面,並告知帳密錯誤
            {
                ViewData["Message"] = "帳號或密碼錯誤";
            }
            return View(result);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Manager");
            return RedirectToAction("Index", "Home");
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        public IActionResult Create()
        {
            return View();
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Administrator admin)
        {
            var adminNo = _context.Administrator.Max(a => a.AdminNo);
            var newAdminNo = int.Parse(adminNo.Substring(1)) + 1;

            admin.AdminNo = "A" + newAdminNo.ToString().PadLeft(3, '0');

            if (ModelState.IsValid)
            {
                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(admin);
        }

        private bool AdminExists(string id)
        {
            return _context.Administrator.Any(a => a.AdminID == id);
        }
    }
}
