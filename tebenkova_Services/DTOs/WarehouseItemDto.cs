using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tebenkova_Services.DTOs
{
    public class WarehouseItemDto
    {
        public string ProductName { get; set; }
        public int CurrentStock { get; set; }
        public int MinStock { get; set; }
        public string Status { get; set; }
        public string StatusColor { get; set; }
    }
}
