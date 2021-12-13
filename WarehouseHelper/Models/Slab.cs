using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WarehouseHelper
{
    public sealed class Slab : ICloneable
    {
        public string SlabId { get; set; }
        public string Worker { get; set; }
        public bool Shift { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Length { get; set; }
        public DateTime? Date { get; set; }
        public string Processing { get; set; }

        public object Clone()
        {
            return new Slab() {
                Worker = this.Worker,
                Shift = this.Shift,
                Length = this.Length,
                Width = this.Width,
                Height = this.Height,
                Date = this.Date,
                Count = this.Count,
                Processing = this.Processing
            };
        }

        [NotMapped]
        public int Count { get; set; }
    }
}
