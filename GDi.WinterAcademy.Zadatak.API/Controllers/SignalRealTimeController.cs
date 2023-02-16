using GDi.WinterAcademy.Zadatak.API.Models.Requests;
using GDi.WinterAcademy.Zadatak.API.Models.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace GDi.WinterAcademy.Zadatak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignalRealTimeController : ControllerBase
    {
        private IHubContext<AppHub> _hub;

        public SignalRealTimeController(IHubContext<AppHub> hub) 
        { 
            _hub = hub;
        }

        [HttpPost("notify")]
        public async Task<ActionResult> Notify([FromBody] NotifyRequest request)
        {
            await _hub.Clients.All.SendAsync("camundaMessageHub", request);
            return this.Ok(request.SensorID);
        }

        [HttpGet]
        public async Task<ActionResult> CheckFunctionality()
        {
            var sensorFunctionality = false;
            Random random = new Random();
            if(random.NextDouble() < 0.5)
            {
                sensorFunctionality = true;
            }
            return this.Ok(sensorFunctionality);
        }
    }
}
