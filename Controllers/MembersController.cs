using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MyProject.Filters;
using MyProject.Models;
using MyProject.ViewModels;

namespace MyProject.Controllers
{
    public class MembersController : Controller
    {
        private readonly MyProjectContext _context;

        public MembersController(MyProjectContext context)
        {
            _context = context;
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FirstOrDefaultAsync(m => m.MemberID == id);

            if (member == null)
            {
                return NotFound();
            }

            var vmMember = new VMMember
            {
                MemberID = member.MemberID,
                PersonalID = member.PersonalID,
                AccountName = member.AccountName,
                PersonalName = member.PersonalName,
                Gender = member.Gender,
                Birthday = member.Birthday,
                Mobile = member.Mobile,
                Email = member.Email,
            };

            return View(vmMember);
        }

       
        public IActionResult Register()
        {
             return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("AccountName,AccountPassword,PasswordConfirm,PersonalName,PersonalID,Gender,Birthday,Mobile,Email")] VMMember vmMember)
        {
            if (ModelState.IsValid)
            {
                // 將 ViewModel 中的 Member 資料轉換並存入 Member 表
                var member = new Member
                {
                    MemberID = Guid.NewGuid().ToString(), // 生成新的 GUID
                    PersonalID = vmMember.PersonalID,
                    AccountName = vmMember.AccountName,
                    PersonalName = vmMember.PersonalName,
                    Gender = vmMember.Gender,
                    Birthday = vmMember.Birthday,
                    Mobile = vmMember.Mobile,
                    Email = vmMember.Email,
                };

                // 創建 MemberPassword 並存入 MemberPassword 表
                var memberPassword = new MemberPassword
                {
                    MemberID = member.MemberID, // 綁定到對應的 MemberID
                    AccountPassword = vmMember.AccountPassword
                };

                //經過 Sha256Hash 雜湊過後，會是一個64碼的16進位數字(DB、Context中的Password長度要改為64，但Model不變)
                memberPassword.AccountPassword = _context.ComputeSha256Hash(memberPassword.AccountPassword);

                _context.Add(member);
                _context.Add(memberPassword);
                await _context.SaveChangesAsync();

                TempData["RegisterMessage"] = "OK";

                return RedirectToAction("Login", "MemberLogin");
            }
            
            return View(vmMember);
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FirstOrDefaultAsync(m => m.MemberID == id);

            if (member == null)
            {
                return NotFound();
            }

            var vmMember = new VMMember
            {
                MemberID = member.MemberID,
                PersonalID = member.PersonalID,
                AccountName = member.AccountName,
                PersonalName = member.PersonalName,
                Gender = member.Gender,
                Birthday = member.Birthday,
                Mobile = member.Mobile,
                Email = member.Email,
            };

            return View(vmMember);
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("MemberID,AccountName,PersonalName,PersonalID,Gender,Birthday,Mobile,Email,AccountPassword,PasswordConfirm")] VMMember vmMember)
        {
            var member = await _context.Member.FindAsync(vmMember.MemberID);
            if (member == null)
            {
                return NotFound();
            }
            else
            {
                member.AccountName = vmMember.AccountName;
                member.PersonalID = vmMember.PersonalID;
                member.PersonalName = vmMember.PersonalName;
                member.Gender = vmMember.Gender;
                member.Birthday = vmMember.Birthday;
                member.Mobile = vmMember.Mobile;
                member.Email = vmMember.Email;
                _context.Update(member);

                var memberPassword = await _context.MemberPassword.FindAsync(member.MemberID);
                memberPassword.MemberID = member.MemberID;
                memberPassword.AccountPassword = _context.ComputeSha256Hash(vmMember.AccountPassword);
                _context.Update(memberPassword);
            }

            if (ModelState.IsValid)
            {
                try
                { 
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction("Details", new { id = member.MemberID });
            }
            return View(vmMember);
        }

        public async Task<bool> MemberInfoExists(string? AccountName, string? PersonalID,string? Mobile, string? Email)
        {
            var member = await _context.Member.Where(m => m.AccountName == AccountName || m.PersonalID == PersonalID || m.Mobile == Mobile || m.Email == Email).FirstOrDefaultAsync();

            return member == null;
        }
    }
}
