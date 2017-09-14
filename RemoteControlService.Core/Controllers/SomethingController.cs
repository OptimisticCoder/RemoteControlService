using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace RemoteControlService.Core.Controllers
{
    public class SomethingController : ApiController
    {
        [Route("something/test")]
        [HttpGet]
        public HttpResponseMessage Test()
        {
            return new HttpResponseMessage
            {
                Content = new StringContent("{ \"status\": \"ok\" }", Encoding.UTF8, "application/json")
            };
        }
    }
}
