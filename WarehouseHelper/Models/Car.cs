using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WarehouseHelper
{
    public sealed class Car
    {
        public Car()
        {
            Stones = new HashSet<Stone>();
        }


        private RelayCommand selectWarehouse;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CarId { get; set; }
        public DateTime Date { get; set; }
        public decimal Cost { get; set; }

        public ICollection<Stone> Stones { get; set; }
    }
}
