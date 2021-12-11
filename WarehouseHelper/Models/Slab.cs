using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WarehouseHelper
{
    public sealed class Slab
    {
        public string SlabId { get; set; }
        public string Worker { get; set; }
        public bool Shift { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Length { get; set; }
        public DateTime Date { get; set; }
        public string Processing { get; set; }

        [NotMapped]
        public int Count { get; set; }
    }
}
