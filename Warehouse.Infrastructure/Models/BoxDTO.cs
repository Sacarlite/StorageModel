using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Infrastructure.Models
{
    public class BoxDTO
    {
        public int Id { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Depth { get; set; }
        public double Weight { get; set; }
        public DateTime? ProductionDate { get; set; }
        public DateTime? ExpirationDateOverride { get; set; }
    }
}
