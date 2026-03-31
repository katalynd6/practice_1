using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tebenkova_Models.Models
{
    [Table("Products", Schema = "app")]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Артикул")]
        public string Article { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Наименование")]
        public string Name { get; set; }

        [ForeignKey("ProductType")]
        public int? ProductTypeId { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "Ед. измерения")]
        public string Unit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Количество на складе")]
        public decimal QuantityInStock { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Минимальный запас")]
        public decimal MinStockQuantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Себестоимость")]
        public decimal? CostPrice { get; set; }

        [ForeignKey("Supplier")]
        public int? SupplierId { get; set; }

        [MaxLength(100)]
        [Display(Name = "Место хранения")]
        public string Location { get; set; }

        // Навигационные свойства
        public virtual ProductType ProductType { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<SaleHistory> SalesHistories { get; set; }
        public virtual ICollection<StockReceipt> StockReceipts { get; set; }
        public virtual ICollection<StockIssue> StockIssues { get; set; }

        // Вычисляемое свойство - статус остатка
        [NotMapped]
        public string StockStatus
        {
            get
            {
                if (QuantityInStock <= 0) return "Отсутствует";
                if (QuantityInStock < MinStockQuantity) return "Меньше минимума";
                return "Норма";
            }
        }

        [NotMapped]
        public string StockStatusColor
        {
            get
            {
                if (QuantityInStock <= 0) return "Red";
                if (QuantityInStock < MinStockQuantity) return "Orange";
                return "Green";
            }
        }
    }
}
