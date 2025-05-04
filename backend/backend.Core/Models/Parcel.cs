using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.Models
{
    public class Parcel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public float Weight { get; set; }

        public required Category Category { get; set; }
        public int CategoryId { get; set; }

        public Guid ShelfId { get; set; }
    }
}