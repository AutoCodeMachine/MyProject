using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyProject.Models;

namespace MyProject.Controllers
{
    public class RouteDetailsController : Controller
    {
        private readonly MyProjectContext _context;
        private readonly IConfiguration _configuration;

        public RouteDetailsController(MyProjectContext context, IConfiguration configuration)
        {
            _context = context;

            //IConfiguration 物件會自動讀取 appsettings.json，並讓我們可以輕鬆存取設定值
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["RouteID"] = new SelectList(_context.RouteInformation, "RouteID", "RouteName");
            ViewData["LandmarkType"] = new SelectList(_context.RouteLandmarkType, "TypeID", "TypeName");


            return View(await _context.RouteDetail.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> GetRoute(string routeIDStart, string landmarkIDStart, string routeIDGoal, string landmarkIDGoal)
        {
            if (routeIDStart == null || landmarkIDStart == null || routeIDGoal == null || landmarkIDGoal == null)
            {
                ViewData["ErrorMessage"] = "請選擇起點或終點。";
            }

            string connectionString = _configuration.GetConnectionString("MyProjectConnection");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("RouteSearch", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RouteIDStart", routeIDStart);
                    cmd.Parameters.AddWithValue("@LandmarkIDStart", landmarkIDStart);
                    cmd.Parameters.AddWithValue("@RouteIDGoal", routeIDGoal);
                    cmd.Parameters.AddWithValue("@LandmarkIDGoal", landmarkIDGoal);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        StringBuilder jsonResult = new StringBuilder();
                        bool hasRows = false;
                        while (await reader.ReadAsync())
                        {
                            hasRows = true;
                            if (!reader.IsDBNull(0))
                            {
                                jsonResult.Append(reader.GetString(0)); // 假設 JSON 字串在第一欄
                            }
                        }
                        if (!hasRows)
                        {
                            jsonResult.Append("[]");
                        }
                        return Json(jsonResult.ToString());
                    }
                }
            }


            /*string jsonResult = "[]";*/ // 預設空 JSON 陣列
            //using (SqlConnection conn = new SqlConnection(connectionString))
            //{
            //    await conn.OpenAsync();
            //    using (SqlCommand cmd = new SqlCommand("RouteSearch", conn))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@RouteIDStart", routeIDStart);
            //        cmd.Parameters.AddWithValue("@LandmarkIDStart", landmarkIDStart);
            //        cmd.Parameters.AddWithValue("@RouteIDGoal", routeIDGoal);
            //        cmd.Parameters.AddWithValue("@LandmarkIDGoal", landmarkIDGoal);

            //        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            //        {
            //            if (await reader.ReadAsync())
            //            {
            //                jsonResult = reader.GetString(0); // SQL 直接回傳 JSON
            //            }
            //        }
            //    }
            //}
            //return Json(jsonResult); // 直接回傳 JSON 給前端
        }

        [HttpGet]
        public JsonResult GetLandmarks(string? routeID, string? typeID)
        {
            if (string.IsNullOrEmpty(routeID) && string.IsNullOrEmpty(typeID))
            {
                return Json(new { success = false, message = "請提供有效的步道與地標類型。" });
            }

            var landmarks = _context.RouteDetail
                .Where(rd => rd.RouteID == routeID && rd.Landmark.TypeID == typeID)
                .Select(rd => new
                {
                    value = rd.Landmark.LandmarkID,
                    text = rd.Landmark.LandmarkName
                })
                .ToList();

            return Json(landmarks);
        }

        



        private bool RouteDetailExists(string id)
        {
            return _context.RouteDetail.Any(e => e.RouteID == id);
        }
    }
}
