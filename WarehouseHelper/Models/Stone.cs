using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WarehouseHelper
{
    public class Stone
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StoneId { get; set; }
        public int CarId { get; set; }
        public decimal PricePerCube { get; set; }
        public float Width { get; set; }
        public float Length { get; set; }
        public float Height { get; set; }
        public string Type { get; set; }
        public bool IsSawn { get; set; }

        public virtual Car Car { get; set; }
    }
}
