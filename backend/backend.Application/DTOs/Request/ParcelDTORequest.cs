using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Application.DTOs.Request
{
    public class ParcelDTORequest
    {
        public Guid WarehouseId { get; set; }

        public required string Name { get; set; }
        public float Weight { get; set; }
        public int CategoryId { get; set; }
    }
}