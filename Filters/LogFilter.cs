using MyProject.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MyProject.Filters
{
    //記錄user訪問網站時的 時間、IP、Agent(瀏覽器)、Controller、Action
    public class LogFilter : IActionFilter
    {
       
        public void OnActionExecuting(ActionExecutingContext context)
        {
           //使用 HttpContext.Items 從 Session 中讀取使用者資訊，方便在不同的 View 使用
            var MemberJson = context.HttpContext.Session.GetString("User");

            if (MemberJson != null)
            {
                var UserInfo = JsonConvert.DeserializeObject<Member>(MemberJson);
                context.HttpContext.Items["User"] = UserInfo;
            }

            var AdminJson = context.HttpContext.Session.GetString("Manager");

            if (AdminJson != null)
            {
                var AdminInfo = JsonConvert.DeserializeObject<Administrator>(AdminJson);
                context.HttpContext.Items["Manager"] = AdminInfo;
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            ////抓資訊
            //var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            //var agent = context.HttpContext.Request.Headers["User-Agent"].ToString();
            //var controller = context.RouteData.Values["controller"];
            //var action = context.RouteData.Values["action"];
            //string user = "Guest";
            //var time = DateTime.Now;

            ////從Session中讀取使用者資訊
            //var memberInfoJson = context.HttpContext.Session.GetString("MemberInfo");

            //if (memberInfoJson != null)
            //{
            //   var member = JsonConvert.DeserializeObject<Member>(memberInfoJson);
            //    user = member?.MemberID + "-" + member?.Account + "-" + member?.Name;
            //}

            ////Log 格式
            //string log = $"{time}, 使用者:{user},IP:{ip},瀏覽器:{agent},Controller:{controller},Action:{action}";

            ////寫入檔案 (要先手動新增資料夾)
            //string filePath = "LogFiles/ActionLog.txt";

            //using (StreamWriter sw = new StreamWriter(filePath, true))
            //{
            //    //將Log 格式傳入
            //    sw.WriteLine(log);
            //}
        }
    }
}


//只會紀錄有發送Request給Model的動作，單純前端的操作不會記錄 (Ex：Local Storage的更動)