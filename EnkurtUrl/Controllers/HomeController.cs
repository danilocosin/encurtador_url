using SiteEnkurtUrl.Models.Response;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EnkurtUrl.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            //string apiUrl = "http://localhost:49677/api/Stats/getStats";

            string apiUrl = ConfigurationSettings.AppSettings["UrlWebApi"].ToString();

            UrlStats objUrl = new UrlStats();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    objUrl = Newtonsoft.Json.JsonConvert.DeserializeObject<UrlStats>(data);

                    ViewBag.Retorno = objUrl;
                }
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}