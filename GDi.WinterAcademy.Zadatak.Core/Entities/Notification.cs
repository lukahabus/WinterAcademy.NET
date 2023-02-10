using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDi.WinterAcademy.Zadatak.Core.Entities
{
    public class Notification
    {
        public long Id { get; set; }
        public float Status { get; set; }
        public DateTime DateTimeReceived { get; set; }
        public Sensor Sensor { get; set; }
        public long SensorId { get; set; }
    }
}
