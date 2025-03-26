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
    public class ManagerForumController : Controller
    {
        private readonly MyProjectContext _context;

        public ManagerForumController(MyProjectContext context)
        {
            _context = context;
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        public async Task<IActionResult> Index(string forumid)
        {
            ViewData["ManagerForumName"] = new SelectList(_context.ForumName, "ForumID", "ForumName1");

            var myProjectContext = _context.Forum.Include(f => f.ForumName).Include(f => f.Member).AsQueryable();

            if (!string.IsNullOrEmpty(forumid))
            {
                myProjectContext = myProjectContext.Where(f => f.ForumID == forumid);
            }
            return View(await myProjectContext.ToListAsync());
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forum = await _context.Forum
                .Include(f => f.ForumName)
                .Include(f => f.Member)
                .FirstOrDefaultAsync(m => m.PostID == id);
            if (forum == null)
            {
                return NotFound();
            }

            return View(forum);
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forum = await _context.Forum
                .Include(f => f.ForumName)
                .Include(f => f.Member)
                .FirstOrDefaultAsync(m => m.PostID == id);
            if (forum == null)
            {
                return NotFound();
            }

            return View(forum);
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var forum = await _context.Forum.FindAsync(id);
            if (forum != null)
            {
                _context.Forum.Remove(forum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ForumExists(string id)
        {
            return _context.Forum.Any(e => e.PostID == id);
        }
    }
}
