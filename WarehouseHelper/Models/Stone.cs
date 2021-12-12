using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WarehouseHelper.VeiwModel;

#nullable disable

namespace WarehouseHelper
{
    public class Stone : ICloneable
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

        public object Clone()
        {
            Stone stone = new Stone()
            {
                Height = this.Height,
                Width = this.Width,
                Length = this.Length,
                PricePerCube = this.PricePerCube,
                IsSawn = this.IsSawn,
                Type = this.Type,
                CarId = this.CarId
            };
            return stone;
        }

        public virtual Car Car { get; set; }
    }
}
