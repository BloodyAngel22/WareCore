using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.Models
{
    public class Warehouse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public IEnumerable<Shelf> Shelves { get; set; } = new List<Shelf>();
    }
}