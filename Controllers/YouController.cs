using Microsoft.AspNetCore.Mvc;

namespace AsyncHttpJob.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YouController : Controller
    {
        // GET
        public JsonResult Index()
        {
            return new JsonResult(new {Flag=true,Msg="OK"});
        }
    }
}