using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDi.WinterAcademy.Zadatak.Core.Entities
{
    public class Sensor
    {
        public long Id { get; set; }
        public float CurrentStatus { get; set; }
        public SensorType Type { get; set; }
        public long TypeId { get; set; }
        public List<Notification> Notifications { get; set; }

    }
}
