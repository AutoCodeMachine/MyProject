using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MyProject.Models;
using MyProject.ViewModels;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace MyProject.Controllers
{
    public class MemberLoginController : Controller
    {
        private readonly MyProjectContext _context;

        public MemberLoginController(MyProjectContext context)
        {
            _context = context;
        }


        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(VMMember vmMember)
        {
            var memberinfo = await _context.Member
                .Where(m => (m.AccountName == vmMember.AccountName || m.Mobile == vmMember.Mobile || m.Email == vmMember.Email))
                .FirstOrDefaultAsync();

            var result = await _context.MemberPassword
                .Where(m => m.MemberID == memberinfo.MemberID && m.AccountPassword == _context.ComputeSha256Hash(vmMember.AccountPassword))
                .FirstOrDefaultAsync();

            if (result != null)
            {
                HttpContext.Session.SetString("User", memberinfo.ToJson());

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
            HttpContext.Session.Remove("User");
            return RedirectToAction("Index", "Home");
        }

        //驗證用戶輸入的是 AccountName、Mobile 還是 Email
        public class MemberCheckResult
        {
            public bool Exists { get; set; }  // 是否找到該用戶
            public string? MatchedField { get; set; }  // 匹配的字段 (AccountName, Mobile, Email)
            public string? MatchedValue { get; set; }  // 匹配的值
        }

        public async Task<MemberCheckResult> CheckMemberExists(string id)
        {
            // 查詢資料庫，檢查每個字段
            var member = await _context.Member
                .Where(m => m.AccountName == id || m.Mobile == id || m.Email == id)
                .FirstOrDefaultAsync();

            if (member != null)
            {
                // 如果找到對應的用戶，返回匹配的字段和對應的值
                if (member.AccountName == id)
                {
                    return new MemberCheckResult
                    {
                        Exists = true,
                        MatchedField = "AccountName",
                        MatchedValue = member.AccountName
                    };
                }
                else if (member.Mobile == id)
                {
                    return new MemberCheckResult
                    {
                        Exists = true,
                        MatchedField = "Mobile",
                        MatchedValue = member.Mobile
                    };
                }
                else if (member.Email == id)
                {
                    return new MemberCheckResult
                    {
                        Exists = true,
                        MatchedField = "Email",
                        MatchedValue = member.Email
                    };
                }
            }
            return new MemberCheckResult
            {
                Exists = false,
                MatchedField = string.Empty,
                MatchedValue = string.Empty
            };
        }

        [HttpGet]
        public async Task<IActionResult> MemberExists(string id)
        {
            var result = await CheckMemberExists(id);
            return Json(result);
        }

    }
}
