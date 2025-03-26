using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyProject.Models;

namespace MyProject.Controllers;

public class HomeController : Controller
{

    private readonly MyProjectContext _context;

    public HomeController(MyProjectContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        ViewData["landmarks"] = new SelectList(_context.RouteLandmarks.Include(rl => rl.Type)
                                .Where(rl => rl.TypeID == "M").Select(rl => new { rl.LandmarkID, rl.LandmarkName }),
                                "LandmarkID", "LandmarkName");

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult GetLandmarkCoordinates(string landmarkID)
    {
        if (string.IsNullOrEmpty(landmarkID))
        {
            return Json(null);
        }

        var landmark = _context.RouteLandmarks.Where(rl => rl.LandmarkID == landmarkID)
                        .Select(rl => new { rl.Longitude, rl.Latitude }).FirstOrDefault();

        if (landmark != null)
        {
            return Json(new { lng = landmark.Longitude, lat = landmark.Latitude });
        }
        else
        {
            return Json(null);
        }
    }
}
