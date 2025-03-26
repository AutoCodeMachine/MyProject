using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyProject.Filters;
using MyProject.Models;
using MyProject.ViewModels;
using NuGet.Packaging.Signing;

namespace MyProject.Controllers
{
    public class ForumController : Controller
    {
        private readonly MyProjectContext _context;

        public ForumController(MyProjectContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string forumid)
        {
            var forumName = await _context.ForumName.FirstOrDefaultAsync(fn => fn.ForumID == forumid);

            ViewBag.ForumName = forumName?.ForumName1;

            ViewBag.ForumID = forumid;

            if (forumName == null)
            {
                return View();
            }

            var forum = await _context.Forum.Where(f => f.ForumID == forumid)
                              .OrderByDescending(f => f.PostTime).FirstOrDefaultAsync();
            if (forum == null)
            {
                ViewData["ForumMessage"] = "目前沒有貼文";
                return View();
            }

            var member = await _context.Member.FirstOrDefaultAsync(m => m.MemberID == forum.MemberID);

            var image = await _context.ForumImage.Where(i => i.PostID == forum.PostID).Select(i => i.Image).ToListAsync();

            // 創建 ViewModel
            var viewModel = new VMForum
                {
                    PostID = forum.PostID,
                    ForumID = forumName.ForumID,
                    ForumName1 = forumName.ForumName1,
                    MemberID = member.MemberID,
                    AccountName = member.AccountName,
                    PostTime = forum.PostTime,
                    PostTitle = forum.PostTitle,
                    PostContents = forum.PostContents,
                    Image = image  
            };

                // 轉換成 IEnumerable 包裝 ViewModel
                var viewModelList = new List<VMForum> { viewModel };

                return View(viewModelList);
        }
      
        public async Task<IActionResult> Details(string postid, string forumid)
        {
            ViewBag.ForumID = forumid;

            if (postid == null)
            {
                return NotFound();
            }

            var forum = await _context.Forum.Include(f => f.ForumName).Include(f => f.Member)
                        .FirstOrDefaultAsync(m => m.PostID == postid);

            var member = await _context.Member.FirstOrDefaultAsync(m => m.MemberID == forum.MemberID);

            var image = await _context.ForumImage
                        .Where(i => i.PostID == forum.PostID).Select(i => i.Image).ToListAsync();

            var forumName = await _context.ForumName.FirstOrDefaultAsync(fn => fn.ForumID == forum.ForumID);

            if (forum == null)
            {
                return NotFound();
            }
            else
            {
                var viewModel = new VMForum
                {
                    PostID = forum.PostID,
                    ForumID = forum.ForumID,
                    ForumName1 = forumName.ForumName1,
                    MemberID = forum.MemberID,
                    AccountName = member.AccountName,
                    PostTime = forum.PostTime,
                    PostTitle = forum.PostTitle,
                    PostContents = forum.PostContents,
                    Image = image
                };
                return View(viewModel);
            }
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        public IActionResult Create(string forumid)
        {
            ViewBag.ForumID = forumid;

            return View();
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostID,ForumID,MemberID,PostTime,PostTitle,PostContents,Image,ImageType")] VMForum vmForum, IFormFile? newPhoto)
        {
            ViewBag.ForumID = vmForum.ForumID;

            var forum = new Forum
            {
                PostID = vmForum.PostID,
                ForumID = vmForum.ForumID,
                MemberID = vmForum.MemberID,
                PostTime = vmForum.PostTime,
                PostTitle = vmForum.PostTitle,
                PostContents = vmForum.PostContents,
            };
            _context.Add(forum);

            // 允許的副檔名
            var allowedExtensions = new[] { ".jpg", ".png", ".gif" };

            // 取得上傳的檔案
            var files = Request.Form.Files.ToList();

            // 檢查是否有上傳檔案，並且檔案數量不超過 99 張
            if (files.Any())
            {
                if (files.Count > 99)
                {
                    ViewData["Message"] = "一次最多只能上傳 99 張圖片。";
                    return View();
                }

                // 取得目錄路徑，這裡假設你將檔案儲存在 "wwwroot/uploads" 目錄
                string uploadDirectory = Path.Combine("wwwroot", "ForumImages");

                // 確保目錄存在
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                // 用來儲存圖片檔案名稱的列表
                List<string> imageNames = new List<string>();

                // 目前檔案的編號從 1 開始
                int fileIndex = 1;

                foreach (var file in files)
                {
                    // 取得副檔名 (小寫處理，以避免大小寫問題)
                    string extension = Path.GetExtension(file.FileName).ToLower();

                    // 確保副檔名符合允許的格式
                    if (allowedExtensions.Contains(extension))
                    {
                        

                        // 產生檔案名稱，格式為 PostID + 編號 + 副檔名
                        string fileName = $"{vmForum.PostID}{fileIndex:00}{extension}"; // 格式化為兩位數字編號

                        // 儲存檔案名稱到 imageNames 列表中
                        imageNames.Add(fileName);


                        // 產生儲存路徑
                        string filePath = Path.Combine(uploadDirectory, fileName);

                        // 儲存檔案
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var forumImage = new ForumImage
                        {
                            PostID = vmForum.PostID,
                            Image = fileName,  // 儲存檔案名稱
                            ImageType = extension  // 儲存檔案副檔名
                        };

                        _context.Add(forumImage);

                        // 增加編號
                        fileIndex++;
                    }
                    else
                    {
                        ViewData["Message"] = "檔案格式錯誤，只能上傳 JPG、PNG 或 GIF。";
                        return View(vmForum);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { ViewBag.ForumID });
            }
            return View(vmForum);
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        public async Task<IActionResult> Edit(string postid, string forumid)
        {
            ViewBag.ForumID = forumid;

            if (postid == null)
            {
                return NotFound();
            }

            var forum = await _context.Forum.FirstOrDefaultAsync(f => f.PostID == postid);
            if (forum == null)
            {
                return NotFound();
            }

           var forumImage = await _context.ForumImage.Where(i => i.PostID == postid).Select(i => i.Image).ToListAsync();

            var vmForum = new VMForum
            {
                PostID = forum.PostID,
                ForumID = forum.ForumID,
                MemberID = forum.MemberID,
                PostTime = forum.PostTime,
                PostTitle = forum.PostTitle,
                PostContents = forum.PostContents,
                Image = forumImage
            };

            return View(vmForum);
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("PostID,ForumID,MemberID,PostTime,PostTitle,PostContents,Image,ImageType")] VMForum editForum)
        {
            ViewBag.ForumID = editForum.ForumID;

            var newForum = await _context.Forum.FirstOrDefaultAsync(f => f.PostID == editForum.PostID);

            if (newForum == null)
            {
                return NotFound();
            }
            else
            {
                newForum.PostTitle = editForum.PostTitle;
                newForum.PostContents = editForum.PostContents;
                _context.Update(newForum);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ForumExists(editForum.PostID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new {postid = editForum.PostID, forumid = ViewBag.ForumID });
            }
            return View(editForum);
        }

        [ServiceFilter(typeof(LoginStatusFilter))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteForum(string postid, string forumid)
        {
            ViewBag.ForumID = forumid;

            var forum = await _context.Forum.FindAsync(postid);
            if (forum != null)
            {
                var forumImages = await _context.ForumImage.Where(i => i.PostID == postid).ToListAsync();
                foreach (var forumImage in forumImages)
                {
                    _context.ForumImage.Remove(forumImage);
                }

                var forumComments = await _context.ForumComments.Where(c => c.PostID == postid).ToListAsync();
                foreach (var forumComment in forumComments)
                {
                    _context.ForumComments.Remove(forumComment);
                }

                var imageNames = forumImages.Select(i => i.Image).ToList();
                foreach (var imageName in imageNames)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ForumImages", imageName ?? string.Empty);
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }
                _context.Forum.Remove(forum);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new {ViewBag.ForumID});
        }

        private bool ForumExists(string id)
        {
            return _context.Forum.Any(e => e.PostID == id);
        }

        public string GetPostID()
        {
            var todayPrefix = DateTime.Now.ToString("yyyyMMdd");
            var lastPost = _context.Forum
                .Where(f => f.PostID.StartsWith(todayPrefix)) // 只取今天的貼文
                .OrderByDescending(f => f.PostID)
                .FirstOrDefault();

            if (lastPost != null && long.TryParse(lastPost.PostID, out long lastID))
            {
                return (lastID + 1).ToString();
            }

            return todayPrefix + "0001"; // 如果今天還沒有貼文，從 0001 開始
        }
    }
}



