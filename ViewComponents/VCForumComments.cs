using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Models;

namespace MyProject.ViewComponents
{
    public class VCForumComments:ViewComponent
    {
        private readonly MyProjectContext _context;

        public VCForumComments(MyProjectContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string postid)
        {
            var forumComments = await _context.ForumComments.Include(fc => fc.Member)
                                .Where(f => f.PostID == postid).OrderBy(f => f.PostTime).ToListAsync();

            return View(forumComments);
        }
    }
}

