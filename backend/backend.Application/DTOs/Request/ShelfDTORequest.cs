using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Application.DTOs.Request
{
    public class ShelfDTORequest
    {
        public float ShelfWeight { get; set; }
        public int CategoryId { get; set; }
        public Guid WarehouseId { get; set; }
    }
}