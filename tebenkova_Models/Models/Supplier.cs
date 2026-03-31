using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tebenkova_Models.Models
{
    [Table("Suppliers", Schema = "app")]
    public class Supplier
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string ContactPerson { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(20)]
        public string INN { get; set; }

        public string Address { get; set; }

        [Range(0, 5)]
        public int Rating { get; set; }

        // Навигационные свойства
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<StockReceipt> StockReceipts { get; set; }
    }
}
