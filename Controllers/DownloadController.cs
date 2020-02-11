using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Dynamic;

namespace app.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DownloadController : Controller
    {
        private readonly IWebHostEnvironment HostingEnvironment;

        public DownloadController(IWebHostEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
        }
        [HttpGet("{platform}/{station}")]
        // GET: /<controller>/
        public IActionResult Index(string platform, string station)
        {
            string filepath = $"{HostingEnvironment.ContentRootPath}/json/{station}.json";
            if (!System.IO.File.Exists(filepath))
                return View("Error");

            dynamic data;
            try { data = JsonConvert.DeserializeObject<ExpandoObject>(System.IO.File.ReadAllText(filepath)); }
            catch { return View("Error"); }

            IDictionary<string, object> dict = data;
            string src = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/app/{station}/{station}_512.png";

            ViewBag.logo = src;

            switch (platform.ToLower())
            {
                case "ios":
                    string iosurl = dict.ContainsKey("iosurl") ? dict["iosurl"].ToString() : null;
                    if (!string.IsNullOrWhiteSpace(iosurl))
                    { Response.Redirect(iosurl); }

                    string path_plist = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/app/{station}/manifest.plist";
                    ViewData["link"] = $"itms-services://?action=download-manifest&url={path_plist}";
                    break;

                case "android":
                    string path_apk = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/app/{station}/{station}.apk";
                    ViewData["link"] = path_apk;
                    break;
            }
            
            return View();
        }
    }
}
