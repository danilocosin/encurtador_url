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
    public class UsersController : ApiController
    {
        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("AddUrl")]
        public IHttpActionResult AddUrl(string userid, string url)
        {
            Url objUrl = new Url();
            Utils utils = new Utils();

            try
            {
                if (utils.getUserById(userid) == 1)
                {
                    objUrl.RealUrl = url;
                    objUrl.ShortenedUrl = Utils.UniqueShortUrl();
                    objUrl.CreatedBy = userid;

                    objUrl.id = utils.AddUrlToDatabase(objUrl);

                    objUrl.ShortenedUrl = Utils.PublicShortUrl(objUrl.ShortenedUrl);
                }
                else
                    return Content(HttpStatusCode.NotFound, "Usuário inexistente");

                return Ok(objUrl);
            }
            catch (Exception)
            {
                return Ok(objUrl);
            }
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.ActionName("getStatsByUser")]
        public IHttpActionResult getStatsByUser(string id)
        {
            UserUrlStats objUrl = new UserUrlStats();
            Utils utils = new Utils();

            try
            {
                objUrl = utils.getStatsByUser(id);

                if (objUrl.userid == null)
                    return Content(HttpStatusCode.NotFound, "404 Not Found");
                else
                    return Ok(objUrl);
            }
            catch (Exception)
            {
                return Ok(objUrl);
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
                utils.DeleteUser(id);

                return Ok(objUrl);
            }
            catch (Exception)
            {
                return Ok(objUrl);
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.ActionName("Put")]
        public HttpResponseMessage Put(string id)
        {
            UserUrlStats objUrl = new UserUrlStats();
            Utils utils = new Utils();

            try
            {
                if (utils.getUserById(id) == 1)
                    return new HttpResponseMessage(HttpStatusCode.Conflict);
                else
                {
                    utils.AddUser(id);
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
            }
            catch (Exception)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao cadastrar usuário"));
            }
        }
    }
}