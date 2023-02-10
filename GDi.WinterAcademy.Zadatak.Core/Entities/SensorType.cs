using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDi.WinterAcademy.Zadatak.Core.Entities
{
    public class SensorType
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public float LowestValueExpected { get; set; }
        public float HighestValueExpected { get; set; }
        public List<Sensor> Sensors { get; set; }

    }
}
