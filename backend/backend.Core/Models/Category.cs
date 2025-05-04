using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Core.Models
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}