using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.Models
{
    public class Shelf
    {
        public Guid Id { get; set; }
        public float ShelfWeight { get; set; }
        public IEnumerable<Parcel> Parcels { get; set; } = new List<Parcel>();

        public required Category Category { get; set; }
        public int CategoryId { get; set; }

        public Guid WarehouseId { get; set; }
    }
}