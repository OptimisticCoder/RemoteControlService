using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace RemoteControlService.Core.Controllers
{
    public class CounterController : ApiController, ICounterController
    {
        private readonly IApplication _application;

        public CounterController(IApplication app)
        {
            _application = app;
        }

        [Route("counter/get")]
        [HttpGet]
        public HttpResponseMessage GetCounter()
        {
            var count = _application.GetCount();

            return new HttpResponseMessage
            {
                Content = new StringContent("{ \"count\": " + count + " }", Encoding.UTF8, "application/json")
            };
        }
    }
}
