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
    public class UrlsController : ApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.ActionName("GetById")]
        public HttpResponseMessage GetById(int id)
        {
            try
            {
                Url url = new Url();
                Utils utils = new Utils();

                url = utils.RetrieveUrlFromDatabase(id);

                if (url.id != 0)
                {
                    var response = Request.CreateResponse(HttpStatusCode.Moved);

                    string uri = url.RealUrl;
                    var uriBuilder = new UriBuilder(uri);
                    uriBuilder.Scheme = "http";

                    response.Headers.Location = uriBuilder.Uri;
                    return response; 
                }
                else
                   return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,"Erro ao requisitar url"));
            }
        }

        [System.Web.Http.HttpDelete]
        [System.Web.Http.ActionName("Delete")]
        public IHttpActionResult Delete(string id)
        {
            UserUrlStats objUrl = new UserUrlStats();
            Utils utils = new Utils();

            try
            {
                utils.Deleteurl(id);

                return Ok(objUrl);
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.NotFound, "Erro ao apagar Url");
            }
        }

    }
}