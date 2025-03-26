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
    public class ForumCommentsController : Controller
    {
        private readonly MyProjectContext _context;

        public ForumCommentsController(MyProjectContext context)
        {
            _context = context;
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        public IActionResult Create(string postid, string forumid)
        {
            ViewBag.PostID = postid;
            ViewBag.ForumID = forumid;

            return View();
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentID,PostID,ForumID,MemberID,PostTime,PostContents")] ForumComments forumComments)
        {
            forumComments.PostTime = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(forumComments);
                await _context.SaveChangesAsync();
                return Json(forumComments);
            }
            return Json(forumComments);
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(string commentid, string postid, string forumid)
        {
            var forumComments = await _context.ForumComments.FindAsync(commentid);
            if (forumComments != null)
            {
                _context.ForumComments.Remove(forumComments);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Forum", new {PostID = postid, ForumID = forumid });
        }

        private bool ForumCommentsExists(string id)
        {
            return _context.ForumComments.Any(e => e.CommentID == id);
        }

        public string GetCommentID()
        {
            var todayPrefix = DateTime.Now.ToString("yyyyMMdd");
            var lastPost = _context.ForumComments
                .Where(f => f.CommentID.StartsWith(todayPrefix)) // 只取今天的貼文
                .OrderByDescending(f => f.CommentID)
                .FirstOrDefault();

            if (lastPost != null && long.TryParse(lastPost.CommentID, out long lastID))
            {
                return (lastID + 1).ToString();
            }

            return todayPrefix + "0001"; // 如果今天還沒有貼文，從 0001 開始
        }

        public IActionResult GetCommentsByViewComponent(string postid)
        {
            return ViewComponent("VCForumComments", new { PostID = postid });
        }
    }
}
