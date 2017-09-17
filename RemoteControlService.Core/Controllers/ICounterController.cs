using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RemoteControlService.Core.Controllers
{
    public interface ICounterController
    {
        HttpResponseMessage GetCounter();
    }
}
