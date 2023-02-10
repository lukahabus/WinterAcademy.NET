using GDi.WinterAcademy.Zadatak.API.Models;
using GDi.WinterAcademy.Zadatak.Core.Entities;
using GDi.WinterAcademy.Zadatak.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static GDi.WinterAcademy.Zadatak.API.Models.NotificationsDTO;

namespace GDi.WinterAcademy.Zadatak.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorTypesController : ControllerBase
    {
        private readonly WinterAcademyZadatakDbContext _dbContext;

        public SensorTypesController(WinterAcademyZadatakDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<SensorTypeModel>>> GetSensorTypes()
        {
            var sensorTypes = await _dbContext.SensorTypes
                            .Select(s => new SensorTypeModel
                            (
                                s.Id,
                                s.Description,
                                s.LowestValueExpected,
                                s.HighestValueExpected
                            ))
                            .ToListAsync();

            return Ok(sensorTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SensorTypeModel>> GetSensorType(int id)
        {
            var sensorType = _dbContext.SensorTypes.FirstOrDefault(s => s.Id == id);

            if (sensorType == null)
            {
                return BadRequest($"The sensor type with id {id} doesn't exist");
            }

            var selectedSensorType = new SensorTypeModel
            (
                sensorType.Id,
                sensorType.Description,
                sensorType.LowestValueExpected,
                sensorType.HighestValueExpected
            );

            return Ok(selectedSensorType);
        }

        [HttpPost]
        public async Task<ActionResult<SensorTypeModel>> AddSensorType([FromBody] SensorTypeModel sensorTypeModel)
        {
            var existingSensorType = _dbContext.SensorTypes.FirstOrDefault(s => s.Description == sensorTypeModel.Description);
            if (existingSensorType is not null)
                return BadRequest("Sensor type with this description already exists");

            var sensorType = new SensorType
            {
                Description = sensorTypeModel.Description,
                LowestValueExpected = sensorTypeModel.LowestValueExpected,
                HighestValueExpected = sensorTypeModel.HighestValueExpected
            };

            _dbContext.SensorTypes.Add(sensorType);

            await _dbContext.SaveChangesAsync();

            return Ok(new SensorTypeModel
            (
                sensorType.Id,
                sensorType.Description,
                sensorType.LowestValueExpected,
                sensorType.HighestValueExpected
            ));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SensorTypeModel>> UpdateSensorType(int id, [FromBody] SensorTypeModel sensorTypeModel)
        {
            var sensorType = _dbContext.SensorTypes.FirstOrDefault(s => s.Id == id);
            if (sensorType is null)
                return BadRequest("Sensor type doesn't exist");

            sensorType.Description = sensorTypeModel.Description;
            sensorType.LowestValueExpected = sensorTypeModel.LowestValueExpected;
            sensorType.HighestValueExpected = sensorTypeModel.HighestValueExpected;

            _dbContext.Entry(sensorType).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            return Ok(sensorTypeModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<SensorTypeModel>> DeleteSensorType(int id)
        {
            var sensorType = _dbContext.SensorTypes.FirstOrDefault(s => s.Id == id);
            if (sensorType is null)
                return BadRequest("Sensor type doesn't exist");

            _dbContext.SensorTypes.Remove(sensorType);
            _dbContext.Entry(sensorType).State = EntityState.Deleted;

            await _dbContext.SaveChangesAsync();

            return Ok(sensorType);
        }
    }
}
