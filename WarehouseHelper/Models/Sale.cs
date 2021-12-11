using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

#nullable disable

namespace WarehouseHelper
{
    public class Sale
    {
        public Sale()
        {
            Products = new HashSet<Product>();
        }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        [Key]
        public int ContractNumber { get; set; }
        public DateTime Date { get; set; }
        public decimal Cost { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
