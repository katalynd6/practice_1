using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tebenkova_Models.Models
{
    [Table("StockReceipts", Schema = "app")]
    public class StockReceipt
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Дата поступления")]
        public DateTime ReceiptDate { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Количество")]
        public decimal Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Цена закупки")]
        public decimal Price { get; set; }

        [ForeignKey("Supplier")]
        public int? SupplierId { get; set; }

        [MaxLength(50)]
        [Display(Name = "Номер документа")]
        public string DocumentNumber { get; set; }

        [MaxLength(500)]
        [Display(Name = "Примечание")]
        public string Notes { get; set; }

        // Вычисляемое поле - общая сумма
        [NotMapped]
        [Display(Name = "Сумма")]
        public decimal TotalAmount => Quantity * Price;

        // Навигационные свойства
        public virtual Product Product { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
