using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace WarehouseHelper
{
    public class Product
    {
        public DateTime Date { get; set; }
        [Key]
        public string SlabId { get; set; }
        public bool Shift { get; set; }
        public string Worker { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public decimal Cost { get; set; }
        public string Name { get; set; }
        public int? ContractNumber { get; set; }

        public virtual Sale Sale { get; set; }
    }
}
