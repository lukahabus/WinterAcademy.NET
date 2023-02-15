using GDi.WinterAcademy.Zadatak.API.Models;
using GDi.WinterAcademy.Zadatak.Core.Entities;
using GDi.WinterAcademy.Zadatak.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;
using static GDi.WinterAcademy.Zadatak.API.Models.NotificationsDTO;

namespace GDi.WinterAcademy.Zadatak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorsController : ControllerBase
    {
        private readonly WinterAcademyZadatakDbContext _dbContext;
        private readonly HttpClient _httpClient;

        public SensorsController(WinterAcademyZadatakDbContext dbContext, HttpClient httpClient)
        {
            _dbContext = dbContext;
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<ActionResult<List<SensorTypeModel>>> GetSensors()
        {
            var sensors = await _dbContext.Sensors
                            .Include(s => s.Type).Include(s => s.Notifications)
                            .Select(s => new SensorModel
                            (
                                s.Id,
                                s.CurrentStatus,
                                s.TypeId,
                                s.Type.Description,
                                s.Type.LowestValueExpected,
                                s.Type.HighestValueExpected,
                                s.Notifications
                                  .Select(notificationList => new NotificationModel
                                  (
                                      notificationList.Id, notificationList.Status, notificationList.DateTimeReceived, notificationList.SensorId
                                  ))
                                  .ToList()
                            ))
                            .ToListAsync();

            return Ok(sensors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SensorModel>> GetSensor(int id)
        {
            var sensor = _dbContext.Sensors.Include(s => s.Type).Include(s => s.Notifications).FirstOrDefault(s => s.Id == id);
            var notifications = sensor.Notifications.ToList();

            List<NotificationModel> notificationModels = new List<NotificationModel>();
            foreach (var notification in notifications)
            {
                notificationModels.Add
                (
                    new NotificationModel
                    (
                        notification.Id,
                        notification.Status,
                        notification.DateTimeReceived,
                        notification.SensorId
                    )
                ); ;
            }

            if (sensor == null)
            {
                return BadRequest($"The sensor with id {id} doesn't exist");
            }

            var selectedSensor = new SensorModel
            (
                sensor.Id, 
                sensor.CurrentStatus, 
                sensor.TypeId, 
                sensor.Type.Description, 
                sensor.Type.LowestValueExpected, 
                sensor.Type.HighestValueExpected,
                notificationModels
            );

            return Ok(selectedSensor);
        }

        [HttpPost]
        public async Task<ActionResult<SensorModel>> AddSensor([FromBody] SensorModel sensorModel)
        {
            var sensor = new Sensor
            {
                CurrentStatus = sensorModel.CurrentStatus,
                TypeId = sensorModel.TypeId,
            };

            _dbContext.Sensors.Add(sensor);

            await _dbContext.SaveChangesAsync();

            var sensorType = _dbContext.SensorTypes.FirstOrDefault(t => t.Id == sensor.TypeId);

            return Ok(new SensorModel
                (
                    sensor.Id, 
                    sensor.CurrentStatus, 
                    sensor.TypeId,
                    sensorType.Description,
                    sensorType.LowestValueExpected,
                    sensorType.HighestValueExpected, 
                    null
                ));
        }

        [HttpPost("check/{sensorId}")]
        public async Task<ActionResult> Check(int sensorId)
        {
            var processId = "sensor_check";
            var url = $"http://localhost:8080/engine-rest/process-definition/key/{processId}/start";
            var bodyData = new {variables = new { }, businessKey = sensorId.ToString() };
            var resp = await _httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(bodyData), Encoding.UTF8, "application/json"));
            var content = await resp.Content.ReadAsStringAsync();
            if(!resp.IsSuccessStatusCode)
            {
                throw new Exception($"DeleteConfigurationResponse exception, status code: {resp.StatusCode}, content: {content}");
            }

            return this.Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SensorModel>> UpdateSensor(int id, [FromBody] SensorModel sensorModel)
        {
            var sensor = _dbContext.Sensors.Include(s => s.Type).Include(s => s.Notifications).FirstOrDefault(s => s.Id == id);
            if (sensor is null)
                return BadRequest("Sensor doesn't exist");

            sensor.CurrentStatus = sensorModel.CurrentStatus;
            //ensure in frontend that only existing sensor types offered as choice
            sensor.TypeId = sensorModel.TypeId;

            _dbContext.Entry(sensor).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return Ok(sensorModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<SensorModel>> DeleteSensor(int id) //[FromBody] SensorModel sensorModel)
        {
            var sensor = _dbContext.Sensors.FirstOrDefault(s => s.Id == id);
            if (sensor is null)
                return BadRequest("Sensor doesn't exist");

            _dbContext.Sensors.Remove(sensor);
            _dbContext.Entry(sensor).State = EntityState.Deleted;

            await _dbContext.SaveChangesAsync();

            return Ok(sensor);
        }
    }
}
