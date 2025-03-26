using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyProject.Filters;
using MyProject.Models;

namespace MyProject.Controllers
{
    public class ManagerForumNameController : Controller
    {
        private readonly MyProjectContext _context;

        public ManagerForumNameController(MyProjectContext context)
        {
            _context = context;
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ForumName.ToListAsync());
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        public IActionResult Create()
        {
            return View();
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ForumID,ForumName1")] ForumName forumName)
        {
            if (ModelState.IsValid)
            {
                _context.Add(forumName);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(forumName);
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumName = await _context.ForumName.FindAsync(id);
            if (forumName == null)
            {
                return NotFound();
            }
            return View(forumName);
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ForumID,ForumName1")] ForumName forumName)
        {
            if (id != forumName.ForumID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(forumName);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ForumNameExists(forumName.ForumID))
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
            return View(forumName);
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumName = await _context.ForumName
                .FirstOrDefaultAsync(m => m.ForumID == id);
            if (forumName == null)
            {
                return NotFound();
            }

            return View(forumName);
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var forumName = await _context.ForumName.FindAsync(id);
            if (forumName != null)
            {
                _context.ForumName.Remove(forumName);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ForumNameExists(string id)
        {
            return _context.ForumName.Any(e => e.ForumID == id);
        }
    }
}
