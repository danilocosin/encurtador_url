using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebApi.Models.Response;

namespace WebApi.Controllers
{
    public class StatsController : ApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.ActionName("Stats")]
        public IHttpActionResult getStats(int id = 0)
        {
            UrlStats objUrl = new UrlStats();
            Utils utils = new Utils();

            try
            {
                objUrl = utils.getStats(id);

                return Ok(objUrl);
            }
            catch (Exception)
            {
                return Ok(objUrl);
            }
        }
    }
}