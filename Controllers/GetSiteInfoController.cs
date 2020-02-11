using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Dynamic;

namespace app.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetSiteInfoController : ControllerBase
    {
        private readonly IWebHostEnvironment HostingEnvironment;

        public GetSiteInfoController(IWebHostEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{platform}/{station}")]
        public string Get(string platform, string station)
        {
            if (string.IsNullOrWhiteSpace(station) || string.IsNullOrWhiteSpace(platform))
                return "Parameter Error";

            string filepath = $"{HostingEnvironment.ContentRootPath}/json/{station}.json";
            if (!System.IO.File.Exists(filepath))
                return string.Empty;

            dynamic data;
            try { data = JsonConvert.DeserializeObject<ExpandoObject>(System.IO.File.ReadAllText(filepath)); }
            catch { return "Exception Error"; }

            IDictionary<string, object> dict = data;
            if (!dict.ContainsKey(platform)) return "Parameter Error";

            decimal ver = decimal.TryParse(dict[platform].ToString(), out ver) ? ver : 0;

            ApiResponse result = new ApiResponse
            {
                ApiVersion = ver,
                HomeAddress = dict["address"].ToString(),
                LoginAddress = $"{dict["address"].ToString()}/Account/login",
                ChatAddress = $"{dict["address"].ToString()}/Program/CustomerService",
                DownloadPath = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/download/{platform}/{station}"
            };

            return JsonConvert.SerializeObject(result);
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
